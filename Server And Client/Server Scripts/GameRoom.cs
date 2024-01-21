using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
	class GameRoom : IJobQueue
	{
		List<ClientSession> _sessions = new List<ClientSession>();
		JobQueue _jobQueue = new JobQueue();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        //int _seed;

       

		public void Push(Action job)
		{
			_jobQueue.Push(job);
		}

		public void Flush()
		{
			
			foreach (ClientSession s in _sessions)
				s.Send(_pendingList);

            if(_pendingList.Count > 0 )
            {
			    Console.WriteLine($"Flushed {_pendingList.Count} items");
            }
			
            _pendingList.Clear();
		}

		public void BroadcastChat(ClientSession session, string chat)
		{
			//S_Chat packet = new S_Chat();
			//packet.playerId = session.SessionId;
			//packet.chat =  $"{chat} I am {packet.playerId}";
			//ArraySegment<byte> segment = packet.Write();
            //
			//_pendingList.Add(segment);			
		}

		public void Enter(ClientSession session)
		{
			_sessions.Add(session);
			session.Room = this;


            //아이디 입력하라는 패킷 보내기, 

            S_AllowChoosingNickname packet = new S_AllowChoosingNickname();
            packet.id = session.SessionId;
            session.Send(packet.Write());
		}

		public void Leave(ClientSession session)
		{
			_sessions.Remove(session);
		}

        public List<ClientSession> GetAllSession()
        {
            return _sessions;
        }

        public void BroadcastNewJoinedPlayer(ClientSession session)
        {
            //B
            S_B_NewPlayerEnterGame packet = new S_B_NewPlayerEnterGame();
            packet.playerInfo.id = session.SessionId;
            packet.playerInfo.posX = session.Player.PosX;
            packet.playerInfo.posY = session.Player.PosY;
            packet.playerInfo.speedPerSec = session.Player.SpeedPerSec;

            //받는 쪽에서 자기가 알아서 거름
            Broadcast(packet);






        }

        public void Broadcast(IPacket packet)
        {
            foreach (ClientSession session in _sessions)
            {
                session.Send(packet.Write());
            }
        }
        public void Broadcast_E(ClientSession mySession, IPacket packet)
        {
            foreach (ClientSession session in _sessions)
            {
                if(session.SessionId == mySession.SessionId)
                {
                    continue;
                }

                session.Send(packet.Write());
            }
        }
	}
}
