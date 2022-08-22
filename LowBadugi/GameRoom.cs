using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerCore;

namespace LowBadugi
{
	class GameRoom : IJobQueue
	{
		private object _lock = new object();
		public int LimitGP = 0;
		public int CurrentPlayer { get; set; } = 0;
		public int MaxPlayer { get; set; } = 5;

		public bool IsStart { get; set; } = false;
		public bool IsMax { get; set; } = false;

		public bool GameEnd { get; set; } = false;
		List<ClientSession> _sessions = new List<ClientSession>();
		JobQueue _jobQueue = new JobQueue();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
		List<S_BroadcastDeckList.Deck> Deck = new List<S_BroadcastDeckList.Deck>();

		List<ClientSession> _players = new List<ClientSession>();

		public int Phase { get; set; } = 0;

		public int CurrentBatting { get; set; } = -1;
		public int CurrentChange { get; set; } = -1;

		public int Full { get; set; } = 0;
		public int Haft { get; set; } = 0;
		public int BBing { get; set; } = 0;

		public int BatMoney { get; set; } = 0;
		public int allInBat { get; set; } = 0;
		public int Call { get; set; } = 0;
		public int MainPot { get; set; } = 0;
		public int SubPot1 { get; set; } = 0;
		public int SubPot2 { get; set; } = 0;
		public bool[] AllInCheck { get; set; } = new bool[5];
		public int LastCallNumber { get; set; } = -1;
		public bool IsBBing { get; set; } = false;
		public bool IsHarf { get; set; } = false;
		public bool IsFull { get; set; } = false;
		public bool IsCheck { get; set; } = false;

		private int batTurn = 0;
		public void Push(Action job)
		{
			_jobQueue.Push(job);
		}

		public void Flush()
		{
			foreach (ClientSession s in _sessions)
			{
				s.Send(_pendingList);
			}

			_pendingList.Clear();
		}

		public void Broadcast(ArraySegment<byte> segment)
		{
			_pendingList.Add(segment);
		}

		public void Enter(ClientSession session)
		{
			_sessions.Add(session);
			session.Room = this;
			CurrentPlayer++;
			S_RoomPlayerList list = new S_RoomPlayerList();
			foreach (ClientSession s in _sessions)
			{
				list.Players.Add(new S_RoomPlayerList.Player()
				{
					isSelf = (s == session),
					sessionId = s.SessionId,
					playerId = s.PlayerId,
					playerName = s.PlayerName,
					gp = s.GP,
				}); ;
			}
			session.Send(list.Write());

			S_BroadcastEnterRoom enter = new S_BroadcastEnterRoom();
			enter.sessionId = session.SessionId;
			enter.playerId = session.PlayerId;
			enter.playerName = session.PlayerName;
			enter.gp = session.GP;

			Broadcast(enter.Write());

			Push(() => CheckPlayerCount());
		}

		public void Leave(ClientSession session)
		{
			int roomNum = Program.Rooms.IndexOf(this);
			_sessions.Remove(session);
			CurrentPlayer--;
			S_BroadcastLeaveRoom leave = new S_BroadcastLeaveRoom();
			leave.sessionId = session.SessionId;
			session.Room = null;
			session.Send(leave.Write());
			Broadcast(leave.Write());

			Console.WriteLine($"{session.PlayerId} has leave Room {roomNum}");
			if (_sessions.Count == 0)
			{
				_pendingList.Clear();
				Program.Rooms.Remove(this);
				Console.WriteLine($"Room {roomNum} has Closed");
			}
		}


		public void CheckPlayerCount()
		{
			if (CurrentPlayer < 3)
			{
				return;
			}
			else if (CurrentPlayer == 3 && !IsStart)
			{
				S_GameStartWait wait = new S_GameStartWait();
				wait.start = true;
				wait.sessionId = _sessions[0].SessionId;

				_sessions[0].Send(wait.Write());
			}
			else if (CurrentPlayer == 4 && !IsStart)
			{

				S_GameStartWait wait = new S_GameStartWait();
				wait.start = false;
				wait.sessionId = _sessions[0].SessionId;

				_sessions[0].Send(wait.Write());

				//S_BroadcastGameStart start = new S_BroadcastGameStart();
				//start.isStart = true;
				//Broadcast(start.Write());

				float time = Environment.TickCount + 2000;
				Push(() => RoomFlushDelay(() => GameStart(), time));
			}
			else if (CurrentPlayer == 5 && !IsStart)
			{
				//S_BroadcastGameStart start = new S_BroadcastGameStart();
				//start.isStart = true;
				//Broadcast(start.Write());

				float time = Environment.TickCount + 2000;
				Push(() => RoomFlushDelay(() => GameStart(), time));
			}
		}
		
		//public void CurrentPlayerCountCheck()
		//{
		//	if (CurrentPlayer < 3)
		//	{
		//		Push(() => CurrentPlayerCountCheck());
		//		return;
		//	}
		//	else
		//	{
		//		if (!IsStart && CurrentPlayer == 3)
		//		{
		//			S_GameStartWait wait = new S_GameStartWait();
		//			wait.start = false;
		//			wait.sessionId = _sessions[0].SessionId;

		//			_sessions[0].Send(wait.Write());
		//			S_BroadcastGameStart start = new S_BroadcastGameStart();
		//			start.isStart = false;
		//			Broadcast(start.Write());
		//			//_sessions[0].Send()
		//		}
		//	}
		//}

		public void GameStart()
		{
			if (IsStart)
			{
				return;
			}
			IsStart = true;

			foreach (ClientSession session in _sessions)
			{
				session.IsPlay = true;
				_players.Add(session);
			}

			Push(() => CreateDeck());
		}

		public void GamePlaying()
		{
			if (!GameEnd)
			{
				Push(() => GamePlaying());
			}
		}
		public void CreateDeck()
		{
			int[] list = Enumerable.Range(0, 52).ToArray();
			int randRange = 51;

			for (int i = 0; i < 52; i++)
			{
				Random rand = new Random();
				
				int idx = rand.Next(0, randRange + 1); ;

				S_BroadcastDeckList.Deck temp = new S_BroadcastDeckList.Deck();
				temp = Program.Decks[list[idx]];
				Deck.Add(temp);
				list[idx] = list[randRange--];
			}

			S_BroadcastDeckList deck = new S_BroadcastDeckList();
			deck.Decks = Deck;

			Broadcast(deck.Write());

			float time = Environment.TickCount + 500;
			Push(() => RoomFlushDelay(() => MainCardGive(), time));
		}

		public void MainCardGive()
		{
			for (int i = 0; i < 4; i++)
			{
				foreach (ClientSession s in _players)
				{
					Push(() => CardGive(s));
				}
			}

			float time = Environment.TickCount + 200;
			Push(() => RoomFlushDelay(() => PlayerBatting(_players[0]), time));
		}

		public void PlayerBattingTimer(ClientSession s, float time)
		{
			if (CurrentBatting != s.SessionId)
			{
				return;
			}
			if (Environment.TickCount < time)
			{
				Push(() => PlayerBattingTimer(s, time));
				return;
			}

			s.IsDie = true;

			Push(() => NextBatting());
			//if (!NextBatting())
			//{
			//	Phase++;
			//	if (Phase > 2)
			//	{

			//	}
			//	else
			//	{
			//		foreach (ClientSession session in _players)
			//		{
			//			session.IsBatting = false;
			//		}
			//		Push(() => PlayerCardChange(_players[0]));
			//	}
			//}
		}

		public void NextBatting()
		{
			int nextTurn = (CurrentBatting + 1) % _players.Count;

			if (nextTurn == LastCallNumber)
			{
				Phase++;
				if (Phase > 2)
				{

				}
				else
				{
					foreach (ClientSession session in _players)
					{
						session.IsBatting = false;
					}
					Push(() => PlayerCardChange(_players[0]));
				}
				return;
			}

			Push(() => PlayerBatting(_players[nextTurn]));
			//foreach (ClientSession session in _players)
			//{
			//	if (session.IsPlay && session.IsBatting)
			//	{
			//		continue;
			//	}
			//	else if (session.IsPlay && !session.IsBatting && !session.IsDie)
			//	{
			//		Push(() => PlayerBatting(session));
			//		next = true;
			//	}
			//}
		}
		public void PlayerBatting(ClientSession s)
		{
			if (s.IsPlay && !s.IsDie)
			{
				CurrentBatting = s.SessionId;

				S_BroadcastPlayerBatting batting = new S_BroadcastPlayerBatting();
				batting.sessionId = s.SessionId;
				batting.playerId = s.PlayerId;
				batting.BBing = BBing;
				batting.Call = Call;
				batting.Harf = Haft;
				batting.Full = Full;
				batting.isBBing = IsBBing;
				batting.isCheck = IsCheck;
				batting.isHarf = IsHarf;
				batting.isFull = IsFull;

				Broadcast(batting.Write());

				float time = Environment.TickCount + 5000;
				Push(() => PlayerBattingTimer(s, time));
			}
			else if (s.IsDie)
			{
				CurrentBatting = s.SessionId;

				Push(() => NextBatting());
			}
		}

		public void PlayerChangeTimer(ClientSession s, float time)
		{
			if (CurrentChange != s.SessionId || s.IsControl)
			{
				return;
			}
			if (Environment.TickCount < time)
			{
				Push(() => PlayerChangeTimer(s, time));
				return;
			}

			Push(() => NextChange());
		}

		public void NextChange()
		{
			foreach (ClientSession session in _players)
			{
				if (session.IsPlay && session.IsChange)
				{
					continue;
				}
				else if (session.IsPlay && !session.IsChange)
				{
					Push(() => PlayerCardChange(session));
					return;
				}
			}
			foreach (ClientSession session in _players)
			{
				session.IsChange = false;
				session.IsControl = false;
			}

			float time = Environment.TickCount + 1000;
			Push(() => RoomFlushDelay(() => PlayerBatting(_players[0]), time));
		}
		public void PlayerCardChange(ClientSession s)
		{
			if (s.IsPlay && !s.IsChange)
			{
				s.IsChange = true;

				CurrentChange = s.SessionId;

				S_BroadcastPlayerChangeTurn change = new S_BroadcastPlayerChangeTurn();

				change.playerId = s.PlayerId;
				change.sessionId = s.SessionId;

				Broadcast(change.Write());

				float time = Environment.TickCount + 7000;
				Push(() => PlayerChangeTimer(s, time));
			}
		}

		public void MultiCardGive(ClientSession session, int count)
		{
			for (int i = 0; i < count; i++)
			{
				Push(() => CardGive(session));
			}

			float time = Environment.TickCount + 200;
			Push(() => RoomFlushDelay(() => NextChange(), time));
		}
		public void CardGive(ClientSession session)
		{
			S_BroadcastCardGive give = new S_BroadcastCardGive();

			Deck.Remove(Deck[0]);

			give.sessionId = session.SessionId;
			give.playerId = session.PlayerId;

			Broadcast(give.Write());
		}

		public void RoomFlushDelay(Action job, float time)
		{
			if (Environment.TickCount < time)
			{
				Push(() => RoomFlushDelay(job, time));
				return;
			}

			job.Invoke();
		}
	}

	
}
