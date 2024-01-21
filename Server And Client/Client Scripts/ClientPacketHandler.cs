using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

class ClientPacketHandler
{

    public static void S_SendInitialInfo_Handler(ServerSession session, IPacket packet)
    {
        S_SendInitialInfo pkt = packet as S_SendInitialInfo;

       

        GameManager.Instance.PushJobQueue(
            () =>
                {
                    
                    PlayerInfo myPlayer;
                    myPlayer.id = pkt.id;
                    myPlayer.posX = pkt.startPosX;
                    myPlayer.posY = pkt.startPosY;
                    myPlayer.speedPerSec = pkt.speedPerSec;


                    BubbleDragonManager.Instance.Instantiate_MyDragon(myPlayer);

                    BubbleDragonManager.Instance.Instantiate_OtherDragon(pkt.playerList);

                    UnityEngine.Random.InitState(pkt.seed);

                }
            );

    }
    
    public static void S_B_NotifyDeadPlayer_Handler(ServerSession session, IPacket packet)
    {
        S_B_NotifyDeadPlayer pkt = packet as S_B_NotifyDeadPlayer;

       

        GameManager.Instance.PushJobQueue(
            () =>
                {
                   BubbleDragonManager.Instance.KickById(pkt.id);
                   



                }
            );

    }
    public static void S_AllowChoosingNickname_Handler(ServerSession session, IPacket packet)
    {
        S_AllowChoosingNickname pkt = packet as S_AllowChoosingNickname;

       

        GameManager.Instance.PushJobQueue(
            () =>
                {
                   
                    GameManager.Instance._id = pkt.id;

                    

                }
            );

    }
    
    
    public static void S_B_Move_Handler(ServerSession session, IPacket packet)
    {
        S_B_Move pkt = packet as S_B_Move;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {
                    MoveDir dir = pkt.isRight ? MoveDir.Right : MoveDir.Left;

                    BubbleDragonManager.Instance.SetMoveDirByid(pkt.id, dir);

                    Debug.Log(dir);

                }
            );

    } 
    public static void S_B_GameStart_Handler(ServerSession session, IPacket packet)
    {
        S_B_GameStart pkt = packet as S_B_GameStart;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {
                    GameManager.Instance.StartGame();
                    UIManager.Instance.ShowMiddleText("");
                    

                }
            );

    } 
    
    public static void S_B_SendChat_Handler(ServerSession session, IPacket packet)
    {
        S_B_SendChat pkt = packet as S_B_SendChat;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {
                    UIManager.Instance.ShowSpeechBubbleText(pkt.id, pkt.chatStr);


                    
                    Chat.Instance.AddChat(pkt.id, pkt.chatStr);
                    

                }
            );

    }
    
    public static void S_B_Kick_Handler(ServerSession session, IPacket packet)
    {
        S_B_Kick pkt = packet as S_B_Kick;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {
                    if(pkt.id == GameManager.Instance._id)
                    {
                        Debug.Log("킥당함");
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }
                    else
                    {
                        BubbleDragonManager.Instance.KickById(pkt.id);
                    }

                }
            );

    }
    public static void S_B_MakeBubble_Handler(ServerSession session, IPacket packet)
    {
        S_B_MakeBubble pkt = packet as S_B_MakeBubble;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {
                    BubbleDragonManager.Instance.MakeBubbleById(pkt.id, pkt.posX, pkt.posY, pkt.isRight);

                    

                }
            );

    }
    
    public static void S_B_MakeBubble360_Handler(ServerSession session, IPacket packet)
    {
        S_B_MakeBubble360 pkt = packet as S_B_MakeBubble360;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {
                    BubbleDragonManager.Instance.MakeBubble360ById(pkt.id);

                    

                }
            );

    }
    
    public static void S_B_Heal_Handler(ServerSession session, IPacket packet)
    {
        S_B_Heal pkt = packet as S_B_Heal;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {
                    //BubbleDragonManager.Instance.HealAllDragonExceptMe(pkt.id);
                    BubbleDragonManager.Instance.HealDragon(pkt.id);

                    

                }
            );

    }
    public static void S_B_Jump_Handler(ServerSession session, IPacket packet)
    {
        S_B_Jump pkt = packet as S_B_Jump;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {


                    BubbleDragonManager.Instance.JumpById(pkt.id, pkt.jumpForce);

                    

                }
            );

    }
    public static void S_B_NotifyExitedPlayer_Handler(ServerSession session, IPacket packet)
    {
        S_B_NotifyExitedPlayer pkt = packet as S_B_NotifyExitedPlayer;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {


                    BubbleDragonManager.Instance.DestroyDragon(pkt.id);

                    

                }
            );

    }
    
    public static void S_B_Stop_Handler(ServerSession session, IPacket packet)
    {
        S_B_Stop pkt = packet as S_B_Stop;

        

        GameManager.Instance.PushJobQueue(
            () =>
                {
                   

                    BubbleDragonManager.Instance.SetMoveDirByid(pkt.id, MoveDir.Stop);



                }
            );

    }
    
    public static void S_B_NewPlayerEnterGame_Handler(ServerSession session, IPacket packet)
    {
        S_B_NewPlayerEnterGame pkt = packet as S_B_NewPlayerEnterGame;

        
        


        GameManager.Instance.PushJobQueue(
            () =>
                {
                    if (pkt.playerInfo.id == GameManager.Instance._id)
                    {
                        return;
                    }

                    BubbleDragonManager.Instance.Instantiate_OtherDragon(pkt.playerInfo);



                }
            );

    }
    


}
