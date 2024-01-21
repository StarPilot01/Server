using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ClientPacketManager
{
    static ClientPacketManager _instance = new ClientPacketManager();
    public static ClientPacketManager Instance { get { return _instance; } }

    public const int sizeOfPacketSize = 2;
    public const int sizeOfPacketID = 2;

    ClientPacketManager()
    {
        Register();
    }

    Dictionary<PacketID, Func<ServerSession, ArraySegment<byte>, ServerPacket>> _makeFunc = new Dictionary<PacketID, Func<ServerSession, ArraySegment<byte>, ServerPacket>>();
    Dictionary<PacketID, Action<ServerSession, ServerPacket>> _handler = new Dictionary<PacketID, Action<ServerSession, ServerPacket>>();

    public void Register()
    {
        RegisterPair(PacketID.S_AllowChoosingNickname, MakePacket<S_AllowChoosingNickname>, ClientPacketHandler.S_AllowChoosingNickname_Handler);



        RegisterPair(PacketID.S_SendInitialInfo, MakePacket<S_SendInitialInfo>, ClientPacketHandler.S_SendInitialInfo_Handler);  
        RegisterPair(PacketID.S_B_NewPlayerEnterGame, MakePacket<S_B_NewPlayerEnterGame>, ClientPacketHandler.S_B_NewPlayerEnterGame_Handler); 
         RegisterPair(PacketID.S_B_Move, MakePacket<S_B_Move>, ClientPacketHandler.S_B_Move_Handler); 
         RegisterPair(PacketID.S_B_Stop, MakePacket<S_B_Stop>, ClientPacketHandler.S_B_Stop_Handler); 
         RegisterPair(PacketID.S_B_Jump, MakePacket<S_B_Jump>, ClientPacketHandler.S_B_Jump_Handler); 
         RegisterPair(PacketID.S_B_MakeBubble, MakePacket<S_B_MakeBubble>, ClientPacketHandler.S_B_MakeBubble_Handler); 
        
        RegisterPair(PacketID.S_B_NotifyExitedPlayer, MakePacket<S_B_NotifyExitedPlayer>, ClientPacketHandler.S_B_NotifyExitedPlayer_Handler); 
        RegisterPair(PacketID.S_B_GameStart, MakePacket<S_B_GameStart>,ClientPacketHandler.S_B_GameStart_Handler);  
        
        RegisterPair(PacketID.S_B_SendChat, MakePacket<S_B_SendChat>, ClientPacketHandler.S_B_SendChat_Handler); 
        
        RegisterPair(PacketID.S_B_MakeBubble360, MakePacket<S_B_MakeBubble360>, ClientPacketHandler.S_B_MakeBubble360_Handler); 
        RegisterPair(PacketID.S_B_Heal, MakePacket<S_B_Heal>, ClientPacketHandler.S_B_Heal_Handler); 

        RegisterPair(PacketID.S_B_Kick, MakePacket<S_B_Kick>, ClientPacketHandler.S_B_Kick_Handler); 

         RegisterPair(PacketID.S_B_NotifyDeadPlayer, MakePacket<S_B_NotifyDeadPlayer>, ClientPacketHandler.S_B_NotifyDeadPlayer_Handler); 
        
        
        

       

    }

    //makeFunc , handler에 한번에 추가하는 함수
    private void RegisterPair(PacketID packetID , Func<ServerSession, ArraySegment<byte>, ServerPacket> makeFunc, Action<ServerSession, ServerPacket> handler)
    {
        
        _makeFunc.Add(packetID, makeFunc);
        _handler.Add(packetID, handler);
    }
    public void OnRecvPacket(ServerSession session, ArraySegment<byte> buffer)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        GameManager.PrintLog((PacketID)id + " 패킷 받음");

        Func<ServerSession, ArraySegment<byte>, ServerPacket> func = null;
        if (_makeFunc.TryGetValue((PacketID)id, out func))
        {
            ServerPacket packet = func.Invoke(session, buffer);


            HandlePacket(session, packet);
        }
    }

    T MakePacket<T>(ServerSession session, ArraySegment<byte> buffer) where T : ServerPacket, new()
    {
        T pkt = new T();
        pkt.Read(buffer);
        return pkt;
    }

    public void HandlePacket(ServerSession session, ServerPacket packet)
    {
        Action<ServerSession, ServerPacket> action = null;
        if (_handler.TryGetValue(packet.ID, out action))
            action.Invoke(session, packet);
        
        
    }

}

