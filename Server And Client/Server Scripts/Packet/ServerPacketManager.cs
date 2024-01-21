using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ServerPacketManager
{
    static ServerPacketManager _instance = new ServerPacketManager();
    public static ServerPacketManager Instance { get { return _instance; } }

    public const int sizeOfPacketSize = 2;
    public const int sizeOfPacketID = 2;

    ServerPacketManager()
    {
        Register();
    }

    Dictionary<PacketID, Func<ClientSession, ArraySegment<byte>, ClientPacket>> _makeFunc = new Dictionary<PacketID, Func<ClientSession, ArraySegment<byte>, ClientPacket>>();
    Dictionary<PacketID, Action<ClientSession, ClientPacket>> _handler = new Dictionary<PacketID, Action<ClientSession, ClientPacket>>();

    public void Register()
    {
        

        RegisterPair(PacketID.C_RequestLogin, MakePacket<C_RequestLogin>, PacketHandler.C_RequestLogin_Handler);
        
        RegisterPair(PacketID.C_RequestMove, MakePacket<C_RequestMove>, PacketHandler.C_RequestMove_Handler);
        
        RegisterPair(PacketID.C_RequestHeal, MakePacket<C_RequestHeal>, PacketHandler.C_RequestHeal_Handler);

        RegisterPair(PacketID.C_RequestStop, MakePacket<C_RequestStop>, PacketHandler.C_RequestStop_Handler);

        RegisterPair(PacketID.C_MakeBubble, MakePacket<C_MakeBubble>, PacketHandler.C_MakeBubble_Handler);
        
        RegisterPair(PacketID.C_MakeBubble360, MakePacket<C_MakeBubble360>, PacketHandler.C_MakeBubble360_Handler);
        
        RegisterPair(PacketID.C_RequestJump, MakePacket<C_RequestJump>, PacketHandler.C_RequestJump_Handler); 
        
        RegisterPair(PacketID.C_RequestDead, MakePacket<C_RequestDead>, PacketHandler.C_RequestDead_Handler);
        
        RegisterPair(PacketID.C_SendChat, MakePacket<C_SendChat>, PacketHandler.C_SendChat_Handler);






    }

    //makeFunc , handler에 한번에 추가하는 함수
    private void RegisterPair(PacketID packetID, Func<ClientSession, ArraySegment<byte>, ClientPacket> makeFunc, Action<ClientSession, ClientPacket> handler)
    {

        _makeFunc.Add(packetID, makeFunc);
        _handler.Add(packetID, handler);
    }
    public void OnRecvPacket(ClientSession session, ArraySegment<byte> buffer)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Console.WriteLine((PacketID)id + " 패킷 받음");

        Func<ClientSession, ArraySegment<byte>, ClientPacket> func = null;
        if (_makeFunc.TryGetValue((PacketID)id, out func))
        {
            ClientPacket packet = func.Invoke(session, buffer);


            HandlePacket(session, packet);
        }
    }

    T MakePacket<T>(ClientSession session, ArraySegment<byte> buffer) where T : ClientPacket, new()
    {
        T pkt = new T();
        pkt.Read(buffer);
        return pkt;
    }

    public void HandlePacket(ClientSession session, ClientPacket packet)
    {
        Action<ClientSession, ClientPacket> action = null;
        if (_handler.TryGetValue(packet.ID, out action))
            action.Invoke(session, packet);


    }

}

