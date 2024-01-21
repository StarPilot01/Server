using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class SendBuffer
{
    byte[] _buffer;
    int _usedSize = 0;

    public int FreeSize { get { return _buffer.Length - _usedSize; } }

    public SendBuffer(int size)
    {
        _buffer = new byte[size];
    }

    public ArraySegment<byte> Open(int desiredSize)
    {
        if (desiredSize > FreeSize)
            return null;

        return new ArraySegment<byte>(_buffer, _usedSize, desiredSize);
    }

    public ArraySegment<byte> Close(int usedSize)
    {
        ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
        _usedSize += usedSize;
        return segment;
    }
}

