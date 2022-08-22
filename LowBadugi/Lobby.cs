using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LowBadugi
{
	class Lobby : IJobQueue
	{
		List<ClientSession> _sessions = new List<ClientSession>();
		JobQueue _jobQueue = new JobQueue();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

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

		public bool IsSessionLogined(ClientSession session)
		{
			foreach (ClientSession s in _sessions)
			{
				if (s.SessionId == session.SessionId)
				{
					return false;
				}
			}
			return true;
		}
		public void DuplicateCheck(ClientSession session, C_DuplicateCheck packet)
		{
			S_DuplicateLogin login = new S_DuplicateLogin();
			login.sessionId = session.SessionId;
			login.playerId = packet.playerId;
			login.loginedSessionId = -1;
			foreach (ClientSession s in _sessions)
			{
				if (s.PlayerId == packet.playerId && s.SessionId != session.SessionId)
				{
					login.sessionId = session.SessionId;
					login.playerId = packet.playerId;
					login.loginedSessionId = s.SessionId;
				}
			}

			session.Send(login.Write());
		}

		public bool Duplicate(ClientSession session, out int sessionId)
		{
			foreach (ClientSession s in _sessions)
			{
				if (s.PlayerId == session.PlayerId)
				{
					sessionId = s.SessionId;
					return false;
				}
			}
			sessionId = -1;
			return true;
		}
		public void Enter(ClientSession session)
		{
			session.lobby = this;
			_sessions.Add(session);

			S_EnterLobby enter = new S_EnterLobby();
			enter.sessionId = session.SessionId;
			enter.playerId = session.PlayerId;
			enter.playerName = session.PlayerName;
			enter.gp = session.GP;

			session.Send(enter.Write());
		}

		public void Leave(ClientSession session)
		{
			_sessions.Remove(session);

			S_LeaveLobby leave = new S_LeaveLobby();
			leave.sessionId = session.SessionId;
			session.Send(leave.Write());
		}
		//public void Move(ClientSession session, C_Move packet)
		//{
		//	session.PosX = packet.posX;
		//	session.PosY = packet.posY;
		//	session.PosZ = packet.posZ;

		//	S_BroadcastMove move = new S_BroadcastMove();
		//	move.playerId = session.SessionId;
		//	move.posX = session.PosX;
		//	move.posY = session.PosY;
		//	move.posZ = session.PosZ;
		//	Broadcast(move.Write());
		//}
	}
}
