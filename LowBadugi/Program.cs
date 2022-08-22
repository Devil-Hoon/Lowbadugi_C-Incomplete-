using System;
using System.Collections.Generic;
using System.Net;
using ServerCore;

namespace LowBadugi
{
	class Program
	{
		static Listener _listener = new Listener();
		public static Lobby Lobby = new Lobby();
		public static List<GameRoom> Rooms = new List<GameRoom>();

		public static List<S_BroadcastDeckList.Deck> Decks = new List<S_BroadcastDeckList.Deck>();
		public static void DeleteRoom(GameRoom room)
		{
			Rooms.Remove(room);
		}
		static void FlushLobby()
		{
			Lobby.Push(() => Lobby.Flush());
			JobTimer.Instance.Push(FlushLobby, 100);
		}
		static void FlushRoom()
		{
			foreach (GameRoom room in Rooms)
			{
				room.Push(() => room.Flush());
			}
			JobTimer.Instance.Push(FlushRoom, 100);
		}
		static void Main(string[] args)
		{
			OriginDeck();
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, 8000);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening....");

			JobTimer.Instance.Push(FlushLobby);
			JobTimer.Instance.Push(FlushRoom);

			while (true)
			{
				JobTimer.Instance.Flush();
			}
		}

		static void OriginDeck()
		{
			for (int i = 0; i < 52; i++)
			{
				if (i < 13)
				{
					S_BroadcastDeckList.Deck temp = new S_BroadcastDeckList.Deck();
					temp.originNum = i;
					temp.shape = 0;
					temp.number = i % 13 + 1;
					if (i % 13 == 0)
					{
						temp.cName = "Spade Ace";
					}
					else if (i % 13 == 10)
					{
						temp.cName = "Spade Jack";
					}
					else if (i % 13 == 11)
					{
						temp.cName = "Spade Queen";
					}
					else if (i % 13 == 12)
					{
						temp.cName = "Spade King";
					}
					else
					{
						temp.cName = "Spade" + (i % 13 + 1);
					}
					Decks.Add(temp);
				}
				else if (i < 26)
				{
					S_BroadcastDeckList.Deck temp = new S_BroadcastDeckList.Deck();
					temp.originNum = i;
					temp.shape = 0;
					temp.number = i % 13 + 1;
					if (i % 13 == 0)
					{
						temp.cName = "Diamond Ace";
					}
					else if (i % 13 == 10)
					{
						temp.cName = "Diamond Jack";
					}
					else if (i % 13 == 11)
					{
						temp.cName = "Diamond Queen";
					}
					else if (i % 13 == 12)
					{
						temp.cName = "Diamond King";
					}
					else
					{
						temp.cName = "Diamond" + (i % 13 + 1);
					}
					Decks.Add(temp);
				}
				else if (i < 39)
				{
					S_BroadcastDeckList.Deck temp = new S_BroadcastDeckList.Deck();
					temp.originNum = i;
					temp.shape = 0;
					temp.number = i % 13 + 1;
					if (i % 13 == 0)
					{
						temp.cName = "Heart Ace";
					}
					else if (i % 13 == 10)
					{
						temp.cName = "Heart Jack";
					}
					else if (i % 13 == 11)
					{
						temp.cName = "Heart Queen";
					}
					else if (i % 13 == 12)
					{
						temp.cName = "Heart King";
					}
					else
					{
						temp.cName = "Heart" + (i % 13 + 1);
					}
					Decks.Add(temp);

				}
				else
				{
					S_BroadcastDeckList.Deck temp = new S_BroadcastDeckList.Deck();
					temp.originNum = i;
					temp.shape = 0;
					temp.number = i % 13 + 1;
					if (i % 13 == 0)
					{
						temp.cName = "Clover Ace";
					}
					else if (i % 13 == 10)
					{
						temp.cName = "Clover Jack";
					}
					else if (i % 13 == 11)
					{
						temp.cName = "Clover Queen";
					}
					else if (i % 13 == 12)
					{
						temp.cName = "Clover King";
					}
					else
					{
						temp.cName = "Clover" + (i % 13 + 1);
					}
					Decks.Add(temp);
				}
			}
		}
	}
}
