using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SocketServerWpf
{
    public class SocketConnection
    {
        MainWindow mainWindow;

        public SocketConnection(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private Dictionary<string, Socket> clientDic = new Dictionary<string, Socket>();      

        public void socketConnecting()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            int serverPort = 0;            
            mainWindow.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                serverPort = Convert.ToInt32(mainWindow.textBox1.Text);                
            }));       
            
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, serverPort);
            //IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 8088);

            try
            {
                listener.Bind(ipep);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();

                    new Thread(() => clientChatting(handler)).Start(); //람다식 사용해서 매개변수 쓰레드로 넘기는 방법
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }       

        void clientChatting(Socket handler)
        {
            byte[] bytes = new Byte[1024];
            string clientName = "";
            try
            {
                //int bytesRec = handler.Receive(bytes);
                clientName = Encoding.UTF8.GetString(bytes, 0, handler.Receive(bytes));
                clientDic.Add(clientName, handler);
                Console.WriteLine(clientName + "이 접속");

                while (true)
                {
                    int bytesRec = handler.Receive(bytes);
                    string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    if (data == "")
                        break;

                    Console.WriteLine(clientName + ": " + data);

                    byte[] msg = Encoding.UTF8.GetBytes(data);
                    //handler.Send(msg);

                    foreach (KeyValuePair<string, Socket> pair in clientDic)
                    {
                        pair.Value.Send(msg);
                    }
                    //clientDic.Remove(clientName);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine(clientName + " 연결 종료");
                clientDic.Remove(clientName);
            }

        }

    }
}
