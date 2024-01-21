using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static System.Collections.Specialized.BitVector32;


public class Connector
{
    ServerSession _session;
    CancellationToken _cToken;
    public void Connect(IPEndPoint endPoint, ServerSession session, CancellationToken cToken)
    {
        _session = session;
        _cToken = cToken;
        
        Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.Completed += OnConnectCompleted;
        args.RemoteEndPoint = endPoint;
        args.UserToken = socket;

        RegisterConnect(args);
        
    }

    void RegisterConnect(SocketAsyncEventArgs args)
    {
        Socket socket = args.UserToken as Socket;
        if (socket == null)
            return;

        bool pending = socket.ConnectAsync(args);
        if (pending == false)
            OnConnectCompleted(null, args);
    }

    void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            //ServerSession session = new ServerSession();

            
            _session.Start(args.ConnectSocket , _cToken);
            _session.OnConnected(args.RemoteEndPoint);
        }
        else
        {
            GameManager.PrintLog($"OnConnectCompleted Fail: {args.SocketError}");
        }
    }
}

