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
		_makeFunc.Add((ushort)PacketID.S_BroadcastPlayerBatting, MakePacket<S_BroadcastPlayerBatting>);
		_handler.Add((ushort)PacketID.S_BroadcastPlayerBatting, PacketHandler.S_BroadcastPlayerBattingHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastPlayerChangeTurn, MakePacket<S_BroadcastPlayerChangeTurn>);
		_handler.Add((ushort)PacketID.S_BroadcastPlayerChangeTurn, PacketHandler.S_BroadcastPlayerChangeTurnHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastPlayerChangingCards, MakePacket<S_BroadcastPlayerChangingCards>);
		_handler.Add((ushort)PacketID.S_BroadcastPlayerChangingCards, PacketHandler.S_BroadcastPlayerChangingCardsHandler);
		_makeFunc.Add((ushort)PacketID.S_PlayerDie, MakePacket<S_PlayerDie>);
		_handler.Add((ushort)PacketID.S_PlayerDie, PacketHandler.S_PlayerDieHandler);
		_makeFunc.Add((ushort)PacketID.S_GameStartWait, MakePacket<S_GameStartWait>);
		_handler.Add((ushort)PacketID.S_GameStartWait, PacketHandler.S_GameStartWaitHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastGameStart, MakePacket<S_BroadcastGameStart>);
		_handler.Add((ushort)PacketID.S_BroadcastGameStart, PacketHandler.S_BroadcastGameStartHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastDeckList, MakePacket<S_BroadcastDeckList>);
		_handler.Add((ushort)PacketID.S_BroadcastDeckList, PacketHandler.S_BroadcastDeckListHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastCardGive, MakePacket<S_BroadcastCardGive>);
		_handler.Add((ushort)PacketID.S_BroadcastCardGive, PacketHandler.S_BroadcastCardGiveHandler);
		_makeFunc.Add((ushort)PacketID.S_DuplicateLogin, MakePacket<S_DuplicateLogin>);
		_handler.Add((ushort)PacketID.S_DuplicateLogin, PacketHandler.S_DuplicateLoginHandler);
		_makeFunc.Add((ushort)PacketID.S_EnterLobby, MakePacket<S_EnterLobby>);
		_handler.Add((ushort)PacketID.S_EnterLobby, PacketHandler.S_EnterLobbyHandler);
		_makeFunc.Add((ushort)PacketID.S_LeaveLobby, MakePacket<S_LeaveLobby>);
		_handler.Add((ushort)PacketID.S_LeaveLobby, PacketHandler.S_LeaveLobbyHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnterRoom, MakePacket<S_BroadcastEnterRoom>);
		_handler.Add((ushort)PacketID.S_BroadcastEnterRoom, PacketHandler.S_BroadcastEnterRoomHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastLeaveRoom, MakePacket<S_BroadcastLeaveRoom>);
		_handler.Add((ushort)PacketID.S_BroadcastLeaveRoom, PacketHandler.S_BroadcastLeaveRoomHandler);
		_makeFunc.Add((ushort)PacketID.S_LobbyPlayerList, MakePacket<S_LobbyPlayerList>);
		_handler.Add((ushort)PacketID.S_LobbyPlayerList, PacketHandler.S_LobbyPlayerListHandler);
		_makeFunc.Add((ushort)PacketID.S_RoomPlayerList, MakePacket<S_RoomPlayerList>);
		_handler.Add((ushort)PacketID.S_RoomPlayerList, PacketHandler.S_RoomPlayerListHandler);

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