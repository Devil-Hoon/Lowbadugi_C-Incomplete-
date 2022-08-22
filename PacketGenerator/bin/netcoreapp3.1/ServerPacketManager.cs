using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();
	public void Register()
	{
		_makeFunc.Add((ushort)PacketID.C_DuplicateCheck, MakePacket<C_DuplicateCheck>);
		_handler.Add((ushort)PacketID.C_DuplicateCheck, PacketHandler.C_DuplicateCheckHandler);
		_makeFunc.Add((ushort)PacketID.C_EnterLobby, MakePacket<C_EnterLobby>);
		_handler.Add((ushort)PacketID.C_EnterLobby, PacketHandler.C_EnterLobbyHandler);
		_makeFunc.Add((ushort)PacketID.C_EnterRoom, MakePacket<C_EnterRoom>);
		_handler.Add((ushort)PacketID.C_EnterRoom, PacketHandler.C_EnterRoomHandler);
		_makeFunc.Add((ushort)PacketID.C_LeaveLobby, MakePacket<C_LeaveLobby>);
		_handler.Add((ushort)PacketID.C_LeaveLobby, PacketHandler.C_LeaveLobbyHandler);
		_makeFunc.Add((ushort)PacketID.C_LeaveRoom, MakePacket<C_LeaveRoom>);
		_handler.Add((ushort)PacketID.C_LeaveRoom, PacketHandler.C_LeaveRoomHandler);
		_makeFunc.Add((ushort)PacketID.C_Batting, MakePacket<C_Batting>);
		_handler.Add((ushort)PacketID.C_Batting, PacketHandler.C_BattingHandler);
		_makeFunc.Add((ushort)PacketID.C_CardChange, MakePacket<C_CardChange>);
		_handler.Add((ushort)PacketID.C_CardChange, PacketHandler.C_CardChangeHandler);
		_makeFunc.Add((ushort)PacketID.C_GameStart, MakePacket<C_GameStart>);
		_handler.Add((ushort)PacketID.C_GameStart, PacketHandler.C_GameStartHandler);

	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
	{
		ushort count = 0;
		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
		if (_makeFunc.TryGetValue(id, out func))
		{
			IPacket packet = func.Invoke(session, buffer);
			if (onRecvCallback != null)
			{
				onRecvCallback.Invoke(session, packet);
			}
			else
			{
				HandlePacket(session, packet);
			}
		}
	}

	T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{
		T pkt = new T();
		pkt.Read(buffer);
		return pkt;
	}

	public void HandlePacket(PacketSession session, IPacket packet)
	{
		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(packet.Protocol, out action))
		{
			action.Invoke(session, packet);
		}
	}
}