using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SocketServerWPF
{
    class SerialConnection
    {
        MainWindow mainWindow;

        public SerialConnection(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        SerialPort SP = new SerialPort();

        public void init()
        {
            SP.PortName = "COM7";
            SP.BaudRate = (int)9600;
            SP.DataBits = (int)8;
            SP.Parity = Parity.None;
            SP.StopBits = StopBits.One;
            SP.Encoding = Encoding.UTF8;
            //SP.ReadTimeout = (int)500;
            //SP.WriteTimeout = (int)500;            
        }

        public void Open()
        {
            try
            {
                SP.DataReceived += new SerialDataReceivedEventHandler(SP_DataReceived);
                SP.Open();
                SP.WriteLine("ON");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Close()
        {
            SP.Close();
        }

        private void SP_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            byte[] bytes = new byte[SP.BytesToRead];
            //int bytesRec = SP.Read(bytes, 0, SP.BytesToRead);
            //Console.WriteLine(bytesRec);
            //string s = Encoding.UTF8.GetString(bytes);

            string s = SP.ReadLine();
            Console.WriteLine("\t" + s);
            mainWindow.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                mainWindow.chatBox.Text += s + "\n";
            }));
        }

        public void writeLine()
        {
            SP.WriteLine("It's a test.");
        }


    }
}
