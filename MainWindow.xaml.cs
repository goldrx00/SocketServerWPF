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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocketServerWpf
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("test");
        }

        //string serverPort = "";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //serverPort = textBox1.Text;
            SocketConnection socketConnection = new SocketConnection(this);
            Thread t1 = new Thread(socketConnection.socketConnecting);
            t1.Start();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(textBox1.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
