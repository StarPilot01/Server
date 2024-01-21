using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

public enum PacketID : ushort
{

    //0 ~ 4999 클라 -> 서버 
    //5000 이상  서버 -> 클라
    //S : 서버 -> 클라 패킷
    //C : 클라 -> 서버 패킷
    //S_B : 모두에게 BroadCast
    //S_B_E : 나는 제외하고 모두에게 BroadCast


    C_EnterGame = 0,
    C_RequestMove,
    C_RequestJump,
    C_RequestStop,
    C_MakeBubble,
    C_MakeBubble360,
    C_SendPos,
    C_SendChat,
    C_RequestHeal,
    C_RequestRegister,
    C_RequestLogin,
    C_RequestSalt,
    C_RequestDead,
    C_SelectRole,
    C_CheckAvailableID,
    //----------- 넘사벽 -------------
    S_SendInitialInfo = 5000,
    S_AllowChoosingNickname,
    S_B_NewPlayerEnterGame,
    S_RequestPos,
    S_BroadcastLeaveRoom,
    S_B_Move,
    S_B_Jump,
    S_B_Stop,
    S_B_MakeBubble,
    S_B_MakeBubble360,
    S_B_Heal,
    S_B_SendChat,
    S_B_NotifyDeadPlayer,



    S_B_GameStart,
    S_B_NotifyExitedPlayer,
    S_B_GameOver,
    S_AllowRegistration,
    S_AllowLogin,
    S_InvalidID,
    S_ValidID,
    S_DenyRegistration, Reason_DenyRegistration_AlreadyExistedID,
    S_DenyLogin, Reason_DenyLogin_IDNotFound, Reason_DenyLogin_PWIncorrect,
    S_SendSalt,
    S_B_Kick




}

public enum TransmissionTarget : ushort
{
    ToClient = 0,
    ToServer = 1
}

public interface IPacket
{

    TransmissionTarget Target { get; }
    PacketID ID { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();

    public bool CheckPacket(PacketID id);
}

//서버로 전달 되는 패킷
public interface ServerPacket : IPacket
{
    TransmissionTarget IPacket.Target => TransmissionTarget.ToServer;

    bool IPacket.CheckPacket(PacketID id)
    {
        if ((ushort)id >= 5000)
        {
            return true;
        }

        return false;
    }

}

//클라로 전달되는 패킷
public interface ClientPacket : IPacket
{
    TransmissionTarget IPacket.Target => TransmissionTarget.ToClient;

    bool IPacket.CheckPacket(PacketID id)
    {
        if ((ushort)id >= 0 && (ushort)id < 5000)
        {
            return true;
        }

        return false;
    }
}








//S : 서버 -> 클라 패킷
//C : 클라 -> 서버 패킷






class C_EnterGame : ClientPacket
{
    PacketID IPacket.ID => PacketID.C_EnterGame;


    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);



    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);

        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        //처음 2바이트는 패킷 사이즈인데 아직 안 정해졌으니 나중에 채우기
        writeCount += sizeof(ushort);
        //패킷 넘버 채우기
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterGame), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);






        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");




        return segment;
    }
}

public class C_RequestMove : ClientPacket
{
    public PacketID ID => PacketID.C_RequestMove;

    public int id;
    public bool isRight;
    public C_RequestMove()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);

        this.isRight = segment.ToBool(ref readCount);
    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + sizeof(bool);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_RequestMove), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(isRight), 0, segment.Array, segment.Offset + writeCount, sizeof(bool));
        writeCount += sizeof(bool);

        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}

public class C_RequestStop : ClientPacket
{
    public PacketID ID => PacketID.C_RequestStop;

    public int id;
    public C_RequestStop()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);


    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_RequestStop), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}
public class C_RequestDead : ClientPacket
{
    public PacketID ID => PacketID.C_RequestDead;

    public int id;
    public C_RequestDead()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);


    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_RequestStop), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}


class C_RequestSalt : ClientPacket
{
    PacketID IPacket.ID => PacketID.C_RequestSalt;

    //public int playerId;
    public ushort nameLen;
    public string name;

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        //this.playerId = segment.ToInt(ref readCount);
        this.nameLen = segment.ToUshort(ref readCount);
        this.name = segment.ToStr(nameLen, ref readCount);


    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4 + sizeof(ushort) + nameLen;
        int bufferSize = minSize;

        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        //처음 2바이트는 패킷 사이즈인데 아직 안 정해졌으니 나중에 채우기
        writeCount += sizeof(ushort);
        //패킷 넘버 채우기
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_RequestSalt), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(nameLen), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(Encoding.UTF8.GetBytes(name), 0, segment.Array, segment.Offset + writeCount, nameLen);
        writeCount += nameLen;

        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");




        return segment;
    }
}
class C_CheckAvailableID : ClientPacket
{
    PacketID IPacket.ID => PacketID.C_CheckAvailableID;


    public ushort nameLen;
    public string name;

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);


        this.nameLen = segment.ToUshort(ref readCount);
        this.name = segment.ToStr(nameLen, ref readCount);


    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4 + sizeof(ushort) + nameLen;
        int bufferSize = minSize;

        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        //처음 2바이트는 패킷 사이즈인데 아직 안 정해졌으니 나중에 채우기
        writeCount += sizeof(ushort);
        //패킷 넘버 채우기
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_CheckAvailableID), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(nameLen), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(Encoding.UTF8.GetBytes(name), 0, segment.Array, segment.Offset + writeCount, nameLen);
        writeCount += nameLen;

        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");




        return segment;
    }
}
class C_RequestRegister : ClientPacket
{
    PacketID IPacket.ID => PacketID.C_RequestRegister;

    //public int playerId;
    public ushort nameLen;
    public string name;
    public ushort passwordLen;
    public string password;

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        //this.playerId = segment.ToInt(ref readCount);
        this.nameLen = segment.ToUshort(ref readCount);
        this.name = segment.ToStr(nameLen, ref readCount);

        this.passwordLen = segment.ToUshort(ref readCount);
        this.password = segment.ToStr(passwordLen, ref readCount);

    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(ushort) + nameLen + sizeof(ushort) + passwordLen;

        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        //처음 2바이트는 패킷 사이즈인데 아직 안 정해졌으니 나중에 채우기
        writeCount += sizeof(ushort);
        //패킷 넘버 채우기
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_RequestRegister), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);



        Array.Copy(BitConverter.GetBytes(this.nameLen), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);


        Array.Copy(Encoding.UTF8.GetBytes(this.name), 0, segment.Array, segment.Offset + writeCount, nameLen);
        writeCount += nameLen;

        Array.Copy(BitConverter.GetBytes(this.passwordLen), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);


        Array.Copy(Encoding.UTF8.GetBytes(this.password), 0, segment.Array, segment.Offset + writeCount, passwordLen);
        writeCount += passwordLen;




        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");




        return segment;
    }
}

public class C_MakeBubble : ClientPacket
{
    public PacketID ID => PacketID.C_MakeBubble;

    public int id;
    public float posX;
    public float posY;
    public bool isRight;
    public C_MakeBubble()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);
        this.posX = segment.ToFloat(ref readCount);
        this.posY = segment.ToFloat(ref readCount);
        this.isRight = segment.ToBool(ref readCount);


    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + sizeof(float) + sizeof(float) + sizeof(bool);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_MakeBubble), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + writeCount, sizeof(float));
        writeCount += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + writeCount, sizeof(float));
        writeCount += sizeof(float);

        Array.Copy(BitConverter.GetBytes(this.isRight), 0, segment.Array, segment.Offset + writeCount, sizeof(bool));
        writeCount += sizeof(bool);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}

public class C_MakeBubble360 : ClientPacket
{
    public PacketID ID => PacketID.C_MakeBubble360;

    public int id;

    public C_MakeBubble360()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);


    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_MakeBubble360), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}
public class C_RequestJump : ClientPacket
{
    public PacketID ID => PacketID.C_RequestJump;

    public int id;
    public C_RequestJump()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);


    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_RequestJump), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}
public class C_SendPos : ClientPacket
{
    public PacketID ID => PacketID.C_SendPos;



    public int playerListSize;
    public List<PlayerInfo> playerList;
    public C_SendPos()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        playerListSize += segment.ToInt(ref readCount);

        PlayerInfo addPlayer;
        for (int i = 0; i < playerListSize; i++)
        {
            addPlayer = new PlayerInfo();
            addPlayer.Read(segment.Slice(segment.Offset + readCount, Marshal.SizeOf(typeof(PlayerInfo))));

            readCount += (ushort)Marshal.SizeOf(typeof(PlayerInfo));

            playerList.Add(addPlayer);
        }

    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + playerListSize * (ushort)Marshal.SizeOf(typeof(PlayerInfo));


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_SendPos), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.playerListSize), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);


        for (int i = 0; i < playerListSize; i++)
        {
            Array.Copy(playerList[i].Write().Array, 0, segment.Array, segment.Offset + writeCount, (ushort)Marshal.SizeOf(typeof(PlayerInfo)));

            writeCount += (ushort)Marshal.SizeOf(typeof(PlayerInfo));

        }



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}

public class C_RequestHeal : ClientPacket
{
    public PacketID ID => PacketID.C_RequestHeal;



    public int id;
    public C_RequestHeal()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);

    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_RequestHeal), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));

        writeCount += sizeof(int);






        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}

public class C_SelectRole : ClientPacket
{
    public PacketID ID => PacketID.C_SelectRole;



    public int id;
    public C_SelectRole()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);

    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_SelectRole), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));

        writeCount += sizeof(int);






        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}

public class C_SendChat : ClientPacket
{
    public PacketID ID => PacketID.C_SendChat;


    public int id;
    public int strLength;
    public string chatStr;
    public C_SendChat()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);
        strLength = segment.ToInt(ref readCount);
        chatStr = segment.ToStr(strLength, ref readCount);




    }
    public ArraySegment<byte> Write()
    {

        strLength = Encoding.UTF8.GetByteCount(chatStr);

        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + sizeof(int) +
            strLength;


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_SendChat), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(this.strLength), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(Encoding.UTF8.GetBytes(this.chatStr), 0, segment.Array, segment.Offset + writeCount, strLength);
        writeCount += (ushort)this.strLength;











        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}

public class C_RequestLogin : ClientPacket
{
    public PacketID ID => PacketID.C_RequestLogin;


    public int id;
    public int strLength;
    public string nameStr;
    public C_RequestLogin()
    {
        if (!((ClientPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);
        strLength = segment.ToInt(ref readCount);
        nameStr = segment.ToStr(strLength, ref readCount);




    }
    public ArraySegment<byte> Write()
    {

        strLength = Encoding.UTF8.GetByteCount(nameStr);

        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + sizeof(int) +
            strLength;


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_RequestLogin), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(this.strLength), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(Encoding.UTF8.GetBytes(this.nameStr), 0, segment.Array, segment.Offset + writeCount, strLength);
        writeCount += (ushort)this.strLength;











        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}


//------------------------------------------------------------------------

public struct PlayerInfo
{
    public int id;
    public float posX;
    public float posY;
    public float speedPerSec;
    //public int strLength;
    //public string nameStr;

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;

        this.id = segment.ToInt(ref readCount);
        this.posX = segment.ToFloat(ref readCount);
        this.posY = segment.ToFloat(ref readCount);
        this.speedPerSec = segment.ToFloat(ref readCount);
        //this.strLength = segment.ToInt(ref readCount);
        //this.nameStr = Encoding.UTF8.GetString(BitConverter.GetBytes(segment))
    }
    public ArraySegment<byte> Write()
    {
        byte[] buffer = new byte[sizeof(int) + sizeof(float) + sizeof(float) + sizeof(float)];


        ArraySegment<byte> segment = buffer;
        ushort count = 0;

        Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(speedPerSec), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        return segment;
    }
}


class S_SendInitialInfo : ServerPacket
{
    public PacketID ID => PacketID.S_SendInitialInfo;




    public int id;
    public float startPosX;
    public float startPosY;
    public float speedPerSec;
    public int playerListSize;
    public List<PlayerInfo> playerList = new List<PlayerInfo>();

    public int seed;

    public S_SendInitialInfo()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);
        this.startPosX = segment.ToFloat(ref readCount);
        this.startPosY = segment.ToFloat(ref readCount);
        this.speedPerSec = segment.ToFloat(ref readCount);
        this.playerListSize = segment.ToInt(ref readCount);

        PlayerInfo addPlayer;
        for (int i = 0; i < playerListSize; i++)
        {
            addPlayer = new PlayerInfo();
            addPlayer.Read(segment.Slice(segment.Offset + readCount, Marshal.SizeOf(typeof(PlayerInfo))));

            readCount += (ushort)Marshal.SizeOf(typeof(PlayerInfo));

            playerList.Add(addPlayer);
        }

        this.seed = segment.ToInt(ref readCount);

    }

    public ArraySegment<byte> Write()
    {

        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + sizeof(int) + sizeof(float) + sizeof(float) + sizeof(float) + sizeof(int)
           + playerList.Count * Marshal.SizeOf(typeof(PlayerInfo));
        byte[] buffer = new byte[bufferSize];



        ArraySegment<byte> segment = buffer;
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_SendInitialInfo), 0, segment.Array, segment.Offset + count, sizeof(ushort));

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(this.startPosX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.startPosY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.speedPerSec), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.playerList.Count), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);




        for (int i = 0; i < playerList.Count; i++)
        {
            Array.Copy(playerList[i].Write().Array, 0, segment.Array, segment.Offset + count, Marshal.SizeOf(typeof(PlayerInfo)));
            count += (ushort)Marshal.SizeOf(typeof(PlayerInfo));
        }


        Array.Copy(BitConverter.GetBytes(this.seed), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);





        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}


public class S_B_MakeBubble : ServerPacket
{
    public PacketID ID => PacketID.S_B_MakeBubble;

    public int id;
    public float posX;
    public float posY;
    public bool isRight;
    public S_B_MakeBubble()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);
        this.posX = segment.ToFloat(ref readCount);
        this.posY = segment.ToFloat(ref readCount);
        this.isRight = segment.ToBool(ref readCount);


    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + sizeof(float) + sizeof(float) + sizeof(bool);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_MakeBubble), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + writeCount, sizeof(float));
        writeCount += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + writeCount, sizeof(float));
        writeCount += sizeof(float);

        Array.Copy(BitConverter.GetBytes(this.isRight), 0, segment.Array, segment.Offset + writeCount, sizeof(bool));
        writeCount += sizeof(bool);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}
public class S_B_Jump : ServerPacket
{
    public PacketID ID => PacketID.S_B_Jump;

    public int id;
    public int jumpForce;
    public S_B_Jump()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);
        this.jumpForce = segment.ToInt(ref readCount);


    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + sizeof(int);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_Jump), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(this.jumpForce), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}

public class S_B_NotifyDeadPlayer : ServerPacket
{
    public PacketID ID => PacketID.S_B_NotifyDeadPlayer;

    public int id;

    public S_B_NotifyDeadPlayer()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);


    }
    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_Jump), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}

public class S_B_SendChat : ServerPacket
{

    public PacketID ID => PacketID.S_B_SendChat;


    public int id;
    public int strLength;
    public string chatStr;
    public S_B_SendChat()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);
        strLength = segment.ToInt(ref readCount);
        chatStr = segment.ToStr(strLength, ref readCount);




    }
    public ArraySegment<byte> Write()
    {

        strLength = Encoding.UTF8.GetByteCount(chatStr);

        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + sizeof(int) +
            strLength;


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_SendChat), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(this.strLength), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(Encoding.UTF8.GetBytes(this.chatStr), 0, segment.Array, segment.Offset + writeCount, strLength);
        writeCount += (ushort)this.strLength;








        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}

public class S_AllowChoosingNickname : ServerPacket
{

    public PacketID ID => PacketID.S_AllowChoosingNickname;

    public int id;

    public S_AllowChoosingNickname()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }

    public void Read(ArraySegment<byte> segment)
    {
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);



    }
    public ArraySegment<byte> Write()
    {


        int minSize = 4;
        int bufferSize = minSize + sizeof(int);


        byte[] buffer = new byte[bufferSize];
        ArraySegment<byte> segment = buffer;

        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_AllowChoosingNickname), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));

        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));

        writeCount += sizeof(int);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));











        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));

        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");

        return segment;
    }


}
class S_B_NotifyExitedPlayer : ServerPacket
{
    public PacketID ID => PacketID.S_B_NotifyExitedPlayer;

    public int id;

    public S_B_NotifyExitedPlayer()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);



    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_NotifyExitedPlayer), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);


        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}

class S_B_Move : ServerPacket
{
    public PacketID ID => PacketID.S_B_Move;

    public int id;
    public bool isRight;


    public S_B_Move()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);
        this.isRight = segment.ToBool(ref readCount);

        //this.posX = segment.ToFloat(ref readCount);



    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int) + sizeof(bool);
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_Move), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(this.isRight), 0, segment.Array, segment.Offset + writeCount, sizeof(bool));
        writeCount += sizeof(bool);

        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}
class S_RequestPos : ServerPacket
{
    public PacketID ID => PacketID.S_RequestPos;

    public int id;


    public S_RequestPos()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);


        this.id = segment.ToInt(ref readCount);




    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_RequestPos), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}

class S_B_Stop : ServerPacket
{
    public PacketID ID => PacketID.S_B_Stop;

    public int id;


    public S_B_Stop()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        this.id = segment.ToInt(ref readCount);




    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_Stop), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);



        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}




public class S_B_NewPlayerEnterGame : ServerPacket
{


    public PacketID ID => PacketID.S_B_NewPlayerEnterGame;



    public PlayerInfo playerInfo;
    public S_B_NewPlayerEnterGame()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        playerInfo.Read(segment.Slice(segment.Offset + readCount, Marshal.SizeOf(typeof(PlayerInfo))));
        readCount += (ushort)Marshal.SizeOf(typeof(PlayerInfo));


    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + (ushort)Marshal.SizeOf(typeof(PlayerInfo));
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_NewPlayerEnterGame), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(playerInfo.Write().Array, 0, segment.Array, segment.Offset + writeCount, (ushort)Marshal.SizeOf(typeof(PlayerInfo)));
        writeCount += (ushort)Marshal.SizeOf(typeof(PlayerInfo));

        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}

public class S_B_MakeBubble360 : ServerPacket
{


    public PacketID ID => PacketID.S_B_MakeBubble360;



    public int id;
    public S_B_MakeBubble360()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);



    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_MakeBubble360), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + writeCount, (ushort)Marshal.SizeOf(typeof(int)));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}
public class S_B_Heal : ServerPacket
{


    public PacketID ID => PacketID.S_B_Heal;



    public int id;
    public S_B_Heal()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);



    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_Heal), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + writeCount, (ushort)Marshal.SizeOf(typeof(int)));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}

public class S_B_GameOver : ServerPacket
{


    public PacketID ID => PacketID.S_B_GameOver;

    public int id;
    public S_B_GameOver()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);


    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize + sizeof(int);
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_GameOver), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));
        writeCount += sizeof(int);

        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}

public class S_B_GameStart : ServerPacket
{


    public PacketID ID => PacketID.S_B_GameStart;


    public S_B_GameStart()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);




    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4;
        int bufferSize = minSize;
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_GameStart), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);


        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}

public class S_B_Kick : ServerPacket
{


    public PacketID ID => PacketID.S_B_Kick;

    public int id;
    public S_B_Kick()
    {
        if (!((ServerPacket)this).CheckPacket(ID))
        {
            Console.WriteLine("Error ! this Packet ID is fault");
        }
    }


    public void Read(ArraySegment<byte> segment)
    {
        //패킷 사이즈랑 id 크기만큼 건너 뛰고 읽기 
        ushort readCount = 0;
        readCount += sizeof(ushort);
        readCount += sizeof(ushort);

        id = segment.ToInt(ref readCount);



    }

    public ArraySegment<byte> Write()
    {
        int minSize = 4 + sizeof(int);
        int bufferSize = minSize;
        byte[] buffer = new byte[bufferSize];

        ArraySegment<byte> segment = buffer;
        ushort writeCount = 0;

        writeCount += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_B_Kick), 0, segment.Array, segment.Offset + writeCount, sizeof(ushort));
        writeCount += sizeof(ushort);


        Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + writeCount, sizeof(int));

        writeCount += sizeof(int);


        Array.Copy(BitConverter.GetBytes(writeCount), 0, segment.Array, segment.Offset, sizeof(ushort));




        Debug.Assert(buffer.Length == segment.Count, "버퍼 사이즈와 세그먼트 사이즈가 같지않음!!!!");


        return segment;
    }
}

