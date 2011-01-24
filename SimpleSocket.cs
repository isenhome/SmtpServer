﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SuperSmtpServer
{
    class SimpleSocket
    {
        Encoding Encoding = ASCIIEncoding.ASCII;
        Socket Socket;

        public string CommandSeperator = "\r\n";
        public bool Connected
        {
            get
            {
                return Socket.Connected;
            }
        }


        public SimpleSocket(Socket s)
        {
            this.Socket = s;
        }

        public void Close()
        {
            this.Socket.Disconnect(false);
            this.Socket.Close();
            this.Socket.Dispose();
        }

        public void SendString(string s)
        {
            this.Socket.Send(this.Encoding.GetBytes(s));
        }

        public int Receive(byte[] b)
        {
            try
            {
                return this.Socket.Receive(b);
            }
            catch (ObjectDisposedException e)
            {
                return 0;
            }
        }

        public string GetNextCommand()
        {
            try
            {
                string currentCommand = "";
                byte[] b = new byte[1];

                while (this.Receive(b) == 1)
                {
                    string c = ASCIIEncoding.ASCII.GetString(b);

                    currentCommand += c;

                    if (currentCommand.Contains(CommandSeperator))
                    {
                        return currentCommand;
                    }
                }
            }
            catch (SocketException e)
            {
                Close();
            }

            return null;
        }
    }
}
