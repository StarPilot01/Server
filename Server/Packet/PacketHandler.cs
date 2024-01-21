using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_ChatHandler(PacketSession session, IPacket packet)
	{
		//C_Chat chatPacket = packet as C_Chat;
		//ClientSession clientSession = session as ClientSession;
        //
		//if (clientSession.Room == null)
		//	return;
        //
		//GameRoom room = clientSession.Room;
		//room.Push(
		//	() => room.Broadcast(clientSession, chatPacket.chat)
		//);
	}
    public static void C_RequestMove_Handler(PacketSession session, IPacket packet)
	{
		C_RequestMove reqPacket = packet as C_RequestMove;

        S_B_Move movePacket = new S_B_Move();

        movePacket.id = reqPacket.id;
        movePacket.isRight = reqPacket.isRight;

        ClientSession mySession = SessionManager.Instance.Find(reqPacket.id);
        
        mySession.Room.Broadcast(movePacket);
    }
    
    public static void C_MakeBubble_Handler(PacketSession session, IPacket packet)
	{
        C_MakeBubble reqPacket = packet as C_MakeBubble;

        S_B_MakeBubble makeBubblePacket = new S_B_MakeBubble();

        float diff = 0.5f;

        makeBubblePacket.id = reqPacket.id;
        makeBubblePacket.posX = reqPacket.isRight == true ? reqPacket.posX + diff : reqPacket.posX - diff;
        makeBubblePacket.posY = reqPacket.posY;
        makeBubblePacket.isRight = reqPacket.isRight;



        ClientSession mySession = SessionManager.Instance.Find(reqPacket.id);
        
        mySession.Room.Broadcast(makeBubblePacket);
    }
    public static void C_MakeBubble360_Handler(PacketSession session, IPacket packet)
	{
        C_MakeBubble360 reqPacket = packet as C_MakeBubble360;

        S_B_MakeBubble360 makeBubblePacket = new S_B_MakeBubble360();

        

        makeBubblePacket.id = reqPacket.id;
        



        ClientSession mySession = SessionManager.Instance.Find(reqPacket.id);
        
        mySession.Room.Broadcast(makeBubblePacket);
    }
    
    public static void C_RequestJump_Handler(PacketSession session, IPacket packet)
	{
        C_RequestJump reqPacket = packet as C_RequestJump;

        S_B_Jump jumpPacket = new S_B_Jump();

        jumpPacket.id = reqPacket.id;
        jumpPacket.jumpForce = 270;

        

        ClientSession mySession = SessionManager.Instance.Find(reqPacket.id);
        
        mySession.Room.Broadcast(jumpPacket);
    }
    
    public static void C_RequestStop_Handler(PacketSession session, IPacket packet)
	{
        C_RequestStop reqPacket = packet as C_RequestStop;

        S_B_Stop stopPacket = new S_B_Stop();

        stopPacket.id = reqPacket.id;
       

        ClientSession mySession = SessionManager.Instance.Find(reqPacket.id);
        
        mySession.Room.Broadcast(stopPacket);
    }
    
    public static void C_RequestHeal_Handler(PacketSession session, IPacket packet)
	{
        C_RequestHeal reqPacket = packet as C_RequestHeal;

        S_B_Heal healPacket = new S_B_Heal();

        healPacket.id = reqPacket.id;
       

        ClientSession mySession = SessionManager.Instance.Find(reqPacket.id);

        //mySession.Room.Broadcast(healPacket);
        mySession.Send(healPacket.Write());
    }
    public static void C_RequestDead_Handler(PacketSession session, IPacket packet)
	{
        C_RequestDead reqPacket = packet as C_RequestDead;

        S_B_NotifyDeadPlayer deadPlayerPacket = new S_B_NotifyDeadPlayer();

        deadPlayerPacket.id = reqPacket.id;
       

        ClientSession mySession = SessionManager.Instance.Find(reqPacket.id);
        
        mySession.Room.Broadcast(deadPlayerPacket);
    }
    public static void C_SendChat_Handler(PacketSession session, IPacket packet)
	{
        C_SendChat reqPacket = packet as C_SendChat;

        S_B_SendChat sendPacket = new S_B_SendChat();

        sendPacket.id = reqPacket.id;
        sendPacket.strLength = reqPacket.strLength;
        sendPacket.chatStr = reqPacket.chatStr;
       

        ClientSession mySession = SessionManager.Instance.Find(reqPacket.id);
        
        mySession.Room.Broadcast(sendPacket);
    }


    public static void C_RequestLogin_Handler(PacketSession session, IPacket packet)
    {
        C_RequestLogin reqPacket = packet as C_RequestLogin;

        ClientSession newSession = SessionManager.Instance.Find(reqPacket.id);

        S_SendInitialInfo initialPacket = new S_SendInitialInfo();


        //플레이어 정보 랜덤으로 만들기
        newSession.Player.PosX = Program.random.Next(-5, 5);
        newSession.Player.PosY = -4.5f;
        newSession.Player.SpeedPerSec = 6;


        initialPacket.id = newSession.SessionId;
        initialPacket.startPosX = newSession.Player.PosX;
        initialPacket.startPosY = newSession.Player.PosY;
        initialPacket.speedPerSec = newSession.Player.SpeedPerSec;
        initialPacket.seed = Program.Seed;

        List<PlayerInfo> playerInfos = new List<PlayerInfo>();

        List<ClientSession> clientSessions = newSession.Room.GetAllSession();

        PlayerInfo playerInfo;
        foreach(ClientSession getSession in clientSessions )
        {
            if(getSession.SessionId == newSession.SessionId)
            {
                continue;
            }
            playerInfo.id = getSession.SessionId;
            playerInfo.posX = getSession.Player.PosX;
            playerInfo.posY = getSession.Player.PosY;
            playerInfo.speedPerSec = getSession.Player.SpeedPerSec;
            playerInfos.Add(playerInfo);

        }

        

        initialPacket.playerListSize = playerInfos.Count;
        initialPacket.playerList = playerInfos;

        session.Send(initialPacket.Write());



       
        newSession.Player.Name = reqPacket.nameStr;

        

        //방 인원들에게 전달
        newSession.Room.BroadcastNewJoinedPlayer(newSession);
    }
}
