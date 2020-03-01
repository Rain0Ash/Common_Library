﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com﻿

﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Common_Library.Network
{
    public class Session : IDisposable
    {
        private readonly INetBase _server;
        private readonly Dictionary<String, Object> _attributes = new Dictionary<String, Object>();
        private readonly SocketAsyncEventArgs _writeEventArg = new SocketAsyncEventArgs();
        private ConcurrentQueue<Byte[]> _packetQueue = new ConcurrentQueue<Byte[]>();
        private Boolean _sending;
        private Boolean _closed;

        public UInt32 Id { get; }

        public Socket Socket { get; }
        public SocketAsyncEventArgs ReadEventArg { get; }

        public IPEndPoint LocalEndPoint
        {
            get { return (IPEndPoint) Socket.LocalEndPoint; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return (IPEndPoint) Socket.RemoteEndPoint; }
        }

        public IPAddress Ip { get; }

        public Session(INetBase server, SocketAsyncEventArgs readEventArg, Socket socket)
        {
            Socket = socket;
            Id = (UInt32)RemoteEndPoint.GetHashCode();
            _server = server;
            ReadEventArg = readEventArg;
            _writeEventArg.Completed += WriteComplete;
            Ip = RemoteEndPoint.Address;
            ProccessPackets();
        }

        public void AddAttribute(String name, Object attribute)
        {
            _attributes.Add(name, attribute);
        }

        public Object GetAttribute(String name)
        {
            _attributes.TryGetValue(name, out Object attribute);
            return attribute;
        }

        public void ClearAttribute(String name)
        {
            _attributes.Remove(name);
        }

        public void SendPacket(Byte[] packet)
        {
            if (_packetQueue == null)
            {
                return;
            }

            _packetQueue.Enqueue(packet);
            lock (Socket)
            {
                if (!_sending)
                {
                    ProccessPackets();
                }
            }
        }

        private Byte[] GetNextPacket()
        {
            if (_packetQueue == null)
            {
                return null;
            }

            _packetQueue.TryDequeue(out Byte[] result);
            return result;
        }

        private void ProccessPackets()
        {
            lock (Socket)
            {
                _sending = true;
            }

            Byte[] buffer = GetNextPacket();
            if (buffer == null)
            {
                lock (Socket)
                {
                    _sending = false;
                }

                return;
            }

            _writeEventArg.SetBuffer(buffer, 0, buffer.Length);
            try
            {
                Boolean willRaise = Socket.SendAsync(_writeEventArg);
                if (!willRaise)
                {
                    ProcessSend(_writeEventArg);
                }
            }
            catch (ObjectDisposedException)
            {
                _packetQueue = null;
                _sending = false;
            }
        }

        private void WriteComplete(Object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a send");
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                _server.OnSend(this, e.Buffer, e.Offset, e.BytesTransferred);
                ProccessPackets();
            }
            else
            {
                //_log.Error("Error on ProcessSend: {0}", e.SocketError.ToString());
                Close();
            }
        }

        public void Close()
        {
            if (_closed)
            {
                return;
            }

            _closed = true;
            _packetQueue = null;
            _server.OnDisconnect(this);
            try
            {
                Socket.Shutdown(SocketShutdown.Receive);
            }
            // throws if client process has already closed
            catch (Exception)
            {
                // ignored
            }

            Socket.Close();
            _server.RemoveSession(this);
        }

        public void Dispose()
        {
            _writeEventArg.Dispose();
        }
    }
}
