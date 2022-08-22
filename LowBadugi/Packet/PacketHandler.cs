using LowBadugi;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_GameStartHandler(PacketSession session, IPacket packet)
	{
		ClientSession clientSession = session as ClientSession;

		C_GameStart pkt = packet as C_GameStart;

		GameRoom room = clientSession.Room;

		room.Push(() => room.GameStart());
	}
	public static void C_DuplicateCheckHandler(PacketSession session, IPacket packet)
	{
		ClientSession clientSession = session as ClientSession;

		C_DuplicateCheck pkt = packet as C_DuplicateCheck;

		Program.Lobby.Push(() => Program.Lobby.DuplicateCheck(clientSession, pkt));
	}
	public static void C_EnterLobbyHandler(PacketSession session, IPacket packet)
	{
		ClientSession clientSession = session as ClientSession;

		C_EnterLobby pkt = packet as C_EnterLobby;

		clientSession.PlayerId = pkt.playerId;
		clientSession.PlayerName = pkt.playerName;
		clientSession.GP = pkt.gp;
		Program.Lobby.Push(() => Program.Lobby.Enter(clientSession));
	}
	public static void C_EnterRoomHandler(PacketSession session, IPacket packet)
	{
		ClientSession clientSession = session as ClientSession;

		C_EnterRoom pkt = packet as C_EnterRoom;

		clientSession.PlayerId = pkt.playerId;
		clientSession.PlayerName = pkt.playerName;
		clientSession.GP = pkt.gp;
		
		if (Program.Rooms.Count < 1)
		{
			GameRoom room = new GameRoom();
			room.BBing = 1000;
			Program.Rooms.Add(room);
			room.Push(() => room.Enter(clientSession));
			Console.WriteLine("New Room Create");
		}
		else
		{
			List<GameRoom> temps = new List<GameRoom>();
			bool check = false;
			foreach (GameRoom r in Program.Rooms)
			{
				if (!r.IsMax && r.CurrentPlayer != 0)
				{
					check = true;
					temps.Add(r);
				}
			}

			if (check)
			{
				Random r = new Random();
				int num = r.Next(temps.Count);


				GameRoom temp = temps[num];
				temp.Push(() => temp.Enter(clientSession));
				int roomNum = Program.Rooms.IndexOf(temp);
				Console.WriteLine($"{clientSession.PlayerId} has Enter Room {roomNum}");
			}
			else
			{
				GameRoom room = new GameRoom();
				room.BBing = 1000;
				Program.Rooms.Add(room);
				room.Push(() => room.Enter(clientSession));
				Console.WriteLine("New Room Create");
			}
		}
	}
	public static void C_LeaveLobbyHandler(PacketSession session, IPacket packet)
	{
		ClientSession clientSession = session as ClientSession;

		C_LeaveLobby pkt = packet as C_LeaveLobby;
		if (clientSession.lobby == null)
		{
			return;
		}
		Program.Lobby.Push(() => Program.Lobby.Leave(clientSession));
	}
	public static void C_CardChangeHandler(PacketSession session, IPacket packet)
	{
		ClientSession clientSession = session as ClientSession;
		C_CardChange pkt = packet as C_CardChange;

		if (clientSession.Room == null)
		{
			return;
		}

		S_BroadcastPlayerChangingCards change = new S_BroadcastPlayerChangingCards();

		GameRoom room = clientSession.Room;
		clientSession.IsControl = true;
		change.playerId = clientSession.PlayerId;
		change.sessionId = clientSession.SessionId;
		for (int i = 0; i < pkt.Cardss.Count; i++)
		{
			S_BroadcastPlayerChangingCards.Cards temp = new S_BroadcastPlayerChangingCards.Cards();
			temp.cardName = pkt.Cardss[i].cardName;
			temp.cardNum = pkt.Cardss[i].cardNum;
			temp.originNum = pkt.Cardss[i].cardOriginNum;

			change.Cardss.Add(temp);
		}

		room.Broadcast(change.Write());
		float time = Environment.TickCount + 400;
		room.Push(() => room.RoomFlushDelay(() => room.MultiCardGive(clientSession, pkt.Cardss.Count), time));

	}
	public static void C_BattingHandler(PacketSession session, IPacket packet)
	{

	}
	public static void C_LeaveRoomHandler(PacketSession session, IPacket packet)
	{
		ClientSession clientSession = session as ClientSession;
		if (clientSession.Room == null)
		{
			return;
		}
		GameRoom room = clientSession.Room;
		room.Push(() => room.Leave(clientSession));
	}
}