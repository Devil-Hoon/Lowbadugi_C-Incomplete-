using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

public enum PacketID
{
	S_BroadcastPlayerBatting = 1,
	S_BroadcastPlayerChangeTurn = 2,
	S_BroadcastPlayerChangingCards = 3,
	S_PlayerDie = 4,
	S_GameStartWait = 5,
	S_BroadcastGameStart = 6,
	S_BroadcastDeckList = 7,
	S_BroadcastCardGive = 8,
	S_DuplicateLogin = 9,
	S_EnterLobby = 10,
	S_LeaveLobby = 11,
	S_BroadcastEnterRoom = 12,
	S_BroadcastLeaveRoom = 13,
	S_LobbyPlayerList = 14,
	S_RoomPlayerList = 15,
	C_DuplicateCheck = 16,
	C_EnterLobby = 17,
	C_EnterRoom = 18,
	C_LeaveLobby = 19,
	C_LeaveRoom = 20,
	C_Batting = 21,
	C_CardChange = 22,
	C_GameStart = 23,
	
}

public interface IPacket
{
	ushort Protocol { get; }
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}


public class S_BroadcastPlayerBatting : IPacket
{
	public int sessionId;
	public string playerId;
	public int BBing;
	public int Harf;
	public int Full;
	public int Call;
	public bool isBBing;
	public bool isHarf;
	public bool isFull;
	public bool isCheck;
	
	public ushort Protocol { get { return (ushort)PacketID.S_BroadcastPlayerBatting; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	this.BBing = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	this.Harf = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	this.Full = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	this.Call = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	this.isBBing = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isHarf = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isFull = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isCheck = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastPlayerBatting), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
	Array.Copy(BitConverter.GetBytes(this.BBing), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	Array.Copy(BitConverter.GetBytes(this.Harf), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	Array.Copy(BitConverter.GetBytes(this.Full), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	Array.Copy(BitConverter.GetBytes(this.Call), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	Array.Copy(BitConverter.GetBytes(this.isBBing), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isHarf), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isFull), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isCheck), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_BroadcastPlayerChangeTurn : IPacket
{
	public int sessionId;
	public string playerId;
	
	public ushort Protocol { get { return (ushort)PacketID.S_BroadcastPlayerChangeTurn; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastPlayerChangeTurn), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_BroadcastPlayerChangingCards : IPacket
{
	public int sessionId;
	public string playerId;
	public class Cards
	{
		public int originNum;
		public string cardName;
		public int cardNum;
	
		public void Read(ArraySegment<byte> segment, ref ushort count)
		{
			this.originNum = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort cardNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.cardName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, cardNameLen);
		count += cardNameLen;
		this.cardNum = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		}
	
		public bool Write(ArraySegment<byte> segment, ref ushort count)
		{
			bool success = true;
			Array.Copy(BitConverter.GetBytes(this.originNum), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort cardNameLen = (ushort)Encoding.Unicode.GetBytes(this.cardName, 0, this.cardName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(cardNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += cardNameLen;
		Array.Copy(BitConverter.GetBytes(this.cardNum), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
			return success;
		}
	}
	public List<Cards> Cardss = new List<Cards>();
	
	public ushort Protocol { get { return (ushort)PacketID.S_BroadcastPlayerChangingCards; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	this.Cardss.Clear();
	ushort CardsLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	for (int i = 0; i < CardsLen; i++)
	{
		Cards Cards = new Cards();
		Cards.Read(segment, ref count);
		Cardss.Add(Cards);
	}
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastPlayerChangingCards), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
	Array.Copy(BitConverter.GetBytes((ushort)this.Cardss.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	foreach (Cards Cards in this.Cardss)
	{
		Cards.Write(segment, ref count);
	}
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_PlayerDie : IPacket
{
	public int sessionId;
	public string playerId;
	public bool isDie;
	
	public ushort Protocol { get { return (ushort)PacketID.S_PlayerDie; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	this.isDie = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_PlayerDie), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
	Array.Copy(BitConverter.GetBytes(this.isDie), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_GameStartWait : IPacket
{
	public int sessionId;
	public bool start;
	
	public ushort Protocol { get { return (ushort)PacketID.S_GameStartWait; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	this.start = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_GameStartWait), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	Array.Copy(BitConverter.GetBytes(this.start), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_BroadcastGameStart : IPacket
{
	public bool isStart;
	
	public ushort Protocol { get { return (ushort)PacketID.S_BroadcastGameStart; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.isStart = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastGameStart), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.isStart), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_BroadcastDeckList : IPacket
{
	public class Deck
	{
		public int originNum;
		public string cName;
		public int shape;
		public int number;
	
		public void Read(ArraySegment<byte> segment, ref ushort count)
		{
			this.originNum = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort cNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.cName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, cNameLen);
		count += cNameLen;
		this.shape = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.number = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		}
	
		public bool Write(ArraySegment<byte> segment, ref ushort count)
		{
			bool success = true;
			Array.Copy(BitConverter.GetBytes(this.originNum), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort cNameLen = (ushort)Encoding.Unicode.GetBytes(this.cName, 0, this.cName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(cNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += cNameLen;
		Array.Copy(BitConverter.GetBytes(this.shape), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.number), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
			return success;
		}
	}
	public List<Deck> Decks = new List<Deck>();
	
	public ushort Protocol { get { return (ushort)PacketID.S_BroadcastDeckList; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.Decks.Clear();
	ushort DeckLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	for (int i = 0; i < DeckLen; i++)
	{
		Deck Deck = new Deck();
		Deck.Read(segment, ref count);
		Decks.Add(Deck);
	}
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastDeckList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)this.Decks.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	foreach (Deck Deck in this.Decks)
	{
		Deck.Write(segment, ref count);
	}
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_BroadcastCardGive : IPacket
{
	public int sessionId;
	public string playerId;
	
	public ushort Protocol { get { return (ushort)PacketID.S_BroadcastCardGive; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastCardGive), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_DuplicateLogin : IPacket
{
	public int sessionId;
	public string playerId;
	public int loginedSessionId;
	
	public ushort Protocol { get { return (ushort)PacketID.S_DuplicateLogin; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	this.loginedSessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_DuplicateLogin), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
	Array.Copy(BitConverter.GetBytes(this.loginedSessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_EnterLobby : IPacket
{
	public int sessionId;
	public string playerId;
	public string playerName;
	public int gp;
	
	public ushort Protocol { get { return (ushort)PacketID.S_EnterLobby; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
	count += playerNameLen;
	this.gp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_EnterLobby), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
	ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerNameLen;
	Array.Copy(BitConverter.GetBytes(this.gp), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_LeaveLobby : IPacket
{
	public int sessionId;
	
	public ushort Protocol { get { return (ushort)PacketID.S_LeaveLobby; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_LeaveLobby), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_BroadcastEnterRoom : IPacket
{
	public int sessionId;
	public string playerId;
	public string playerName;
	public int gp;
	
	public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnterRoom; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
	count += playerNameLen;
	this.gp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastEnterRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
	ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
	ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerNameLen;
	Array.Copy(BitConverter.GetBytes(this.gp), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_BroadcastLeaveRoom : IPacket
{
	public int sessionId;
	
	public ushort Protocol { get { return (ushort)PacketID.S_BroadcastLeaveRoom; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastLeaveRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_LobbyPlayerList : IPacket
{
	public class Player
	{
		public bool isSelf;
		public int sessionId;
		public string playerId;
		public string playerName;
		public int gp;
	
		public void Read(ArraySegment<byte> segment, ref ushort count)
		{
			this.isSelf = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
		count += playerIdLen;
		ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
		count += playerNameLen;
		this.gp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		}
	
		public bool Write(ArraySegment<byte> segment, ref ushort count)
		{
			bool success = true;
			Array.Copy(BitConverter.GetBytes(this.isSelf), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += playerIdLen;
		ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += playerNameLen;
		Array.Copy(BitConverter.GetBytes(this.gp), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
			return success;
		}
	}
	public List<Player> Players = new List<Player>();
	
	public ushort Protocol { get { return (ushort)PacketID.S_LobbyPlayerList; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.Players.Clear();
	ushort PlayerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	for (int i = 0; i < PlayerLen; i++)
	{
		Player Player = new Player();
		Player.Read(segment, ref count);
		Players.Add(Player);
	}
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_LobbyPlayerList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)this.Players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	foreach (Player Player in this.Players)
	{
		Player.Write(segment, ref count);
	}
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class S_RoomPlayerList : IPacket
{
	public class Player
	{
		public bool isSelf;
		public int sessionId;
		public string playerId;
		public string playerName;
		public int gp;
	
		public void Read(ArraySegment<byte> segment, ref ushort count)
		{
			this.isSelf = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
		count += playerIdLen;
		ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
		count += playerNameLen;
		this.gp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		}
	
		public bool Write(ArraySegment<byte> segment, ref ushort count)
		{
			bool success = true;
			Array.Copy(BitConverter.GetBytes(this.isSelf), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += playerIdLen;
		ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += playerNameLen;
		Array.Copy(BitConverter.GetBytes(this.gp), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
			return success;
		}
	}
	public List<Player> Players = new List<Player>();
	
	public ushort Protocol { get { return (ushort)PacketID.S_RoomPlayerList; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.Players.Clear();
	ushort PlayerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	for (int i = 0; i < PlayerLen; i++)
	{
		Player Player = new Player();
		Player.Read(segment, ref count);
		Players.Add(Player);
	}
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_RoomPlayerList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)this.Players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	foreach (Player Player in this.Players)
	{
		Player.Write(segment, ref count);
	}
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class C_DuplicateCheck : IPacket
{
	public string playerId;
	
	public ushort Protocol { get { return (ushort)PacketID.C_DuplicateCheck; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_DuplicateCheck), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class C_EnterLobby : IPacket
{
	public string playerId;
	public string playerName;
	public int gp;
	
	public ushort Protocol { get { return (ushort)PacketID.C_EnterLobby; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
	count += playerNameLen;
	this.gp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterLobby), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
	ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerNameLen;
	Array.Copy(BitConverter.GetBytes(this.gp), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class C_EnterRoom : IPacket
{
	public string playerId;
	public string playerName;
	public int gp;
	
	public ushort Protocol { get { return (ushort)PacketID.C_EnterRoom; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		ushort playerIdLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerIdLen);
	count += playerIdLen;
	ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
	count += playerNameLen;
	this.gp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		ushort playerIdLen = (ushort)Encoding.Unicode.GetBytes(this.playerId, 0, this.playerId.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerIdLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerIdLen;
	ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
	Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	count += playerNameLen;
	Array.Copy(BitConverter.GetBytes(this.gp), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class C_LeaveLobby : IPacket
{
	
	
	public ushort Protocol { get { return (ushort)PacketID.C_LeaveLobby; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_LeaveLobby), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class C_LeaveRoom : IPacket
{
	
	
	public ushort Protocol { get { return (ushort)PacketID.C_LeaveRoom; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_LeaveRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class C_Batting : IPacket
{
	public bool isControl;
	public bool isBBing;
	public bool isHarf;
	public bool isFull;
	public bool isCall;
	public bool isCheck;
	public bool isDie;
	public bool isAllIn;
	public int batting;
	
	public ushort Protocol { get { return (ushort)PacketID.C_Batting; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.isControl = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isBBing = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isHarf = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isFull = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isCall = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isCheck = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isDie = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.isAllIn = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.batting = BitConverter.ToInt32(segment.Array, segment.Offset + count);
	count += sizeof(int);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Batting), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.isControl), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isBBing), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isHarf), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isFull), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isCall), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isCheck), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isDie), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.isAllIn), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes(this.batting), 0, segment.Array, segment.Offset + count, sizeof(int));
	count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class C_CardChange : IPacket
{
	public bool isControl;
	public class Cards
	{
		public int cardOriginNum;
		public string cardName;
		public int cardNum;
	
		public void Read(ArraySegment<byte> segment, ref ushort count)
		{
			this.cardOriginNum = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort cardNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.cardName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, cardNameLen);
		count += cardNameLen;
		this.cardNum = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		}
	
		public bool Write(ArraySegment<byte> segment, ref ushort count)
		{
			bool success = true;
			Array.Copy(BitConverter.GetBytes(this.cardOriginNum), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort cardNameLen = (ushort)Encoding.Unicode.GetBytes(this.cardName, 0, this.cardName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(cardNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += cardNameLen;
		Array.Copy(BitConverter.GetBytes(this.cardNum), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
			return success;
		}
	}
	public List<Cards> Cardss = new List<Cards>();
	
	public ushort Protocol { get { return (ushort)PacketID.C_CardChange; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.isControl = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	this.Cardss.Clear();
	ushort CardsLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
	count += sizeof(ushort);
	for (int i = 0; i < CardsLen; i++)
	{
		Cards Cards = new Cards();
		Cards.Read(segment, ref count);
		Cardss.Add(Cards);
	}
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_CardChange), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.isControl), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
	Array.Copy(BitConverter.GetBytes((ushort)this.Cardss.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
	count += sizeof(ushort);
	foreach (Cards Cards in this.Cardss)
	{
		Cards.Write(segment, ref count);
	}
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}
public class C_GameStart : IPacket
{
	public bool start;
	
	public ushort Protocol { get { return (ushort)PacketID.C_GameStart; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.start = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
	count += sizeof(bool);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_GameStart), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.start), 0, segment.Array, segment.Offset + count, sizeof(bool));
	count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}