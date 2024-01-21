using System;
using System.Collections.Generic;
using System.Text;

public static class Extensions
{

    public static ushort ToUshort(this ArraySegment<byte> segment, ref ushort readCount)
    {
        ushort value = BitConverter.ToUInt16(segment.Array, segment.Offset + readCount);
        readCount += sizeof(ushort);
        return value;
    }

    public static int ToInt(this ArraySegment<byte> segment, ref ushort readCount)
    {
        int value = BitConverter.ToInt32(segment.Array, segment.Offset + readCount);
        readCount += sizeof(int);

        return value;
    }
    public static float ToFloat(this ArraySegment<byte> segment, ref ushort readCount)
    {
        float value = BitConverter.ToSingle(segment.Array, segment.Offset + readCount);
        readCount += sizeof(float);

        return value;
    }

    public static bool ToBool(this ArraySegment<byte> segment, ref ushort readCount)
    {
        bool value = BitConverter.ToBoolean(segment.Array, segment.Offset + readCount);
        readCount += sizeof(bool);

        return value;
    }

    public static string ToStr(this ArraySegment<byte> segment, int shouldReadCount, ref ushort readCount)
    {
        string value = Encoding.UTF8.GetString(segment.Array, segment.Offset + readCount, shouldReadCount);
        return value;
    }


}