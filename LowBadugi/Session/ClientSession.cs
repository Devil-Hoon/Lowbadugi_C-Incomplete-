using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace LowBadugi
{
	class ClientSession : PacketSession
	{
		public int SessionId { get; set; }
		public Lobby lobby { get; set; }
		public GameRoom Room { get; set; }
		public string PlayerId { get; set; }
		public string PlayerName { get; set; }
		public int GP { get; set; }

		public bool IsPlay { get; set; }

		public bool IsBatting { get; set; }
		public bool IsChange { get; set; }
		public bool IsControl { get; set; }

		public bool IsDie { get; set; }
		public bool IsCall { get; set; }

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}
		public override void OnConnected(EndPoint endPoint)
		{

			Console.WriteLine($"OnConnected : {endPoint}");

			//Program.Lobby.Push(() => Program.Lobby.Enter(this));
			//Program.Room.Push(() => Program.Room.Enter(this));
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			SessionManager.Instance.Remove(this);
			if (Room != null)
			{
				GameRoom room = Room;
				room.Push(() => room.Leave(this));
				Room = null;
			}
			Program.Lobby.Push(() => Program.Lobby.Leave(this));
			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{

		}
	}
}
