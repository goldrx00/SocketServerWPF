using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace SocketServerWPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        SocketConnection mSocketConnection;
        TelegramAPI mTelegramAPI;
        SerialConnection mSerialConnection;

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("test");
            this.DataContext = new LoginViewModel(); //데이터 바인딩
        }

        //string serverPort = "";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //소켓 커넥션
            mSocketConnection = new SocketConnection(this);
            Thread t1 = new Thread(mSocketConnection.socketConnecting);
            t1.Start();

            //telegramAPI 시작
            mTelegramAPI = new TelegramAPI(this);
            mTelegramAPI.telegramAPIAsync();
            mTelegramAPI.setTelegramEvent();

            //시리얼 통신 시작
            mSerialConnection = new SerialConnection(this);
            mSerialConnection.init();
            mSerialConnection.Open();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(textBox1.Text);
            chatBox.Text += "zz\n";
            mSocketConnection.consoleWW();
            doLogin();
            //consoleWW
            mSerialConnection.writeLine();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Application.Current.Shutdown(); //일반적인 방법           
            Process.GetCurrentProcess().Kill(); //프로세스 강제 종료
        }

        private bool doLogin()
        {
            //-- 로그인 처리  
            var viewModel = this.DataContext as LoginViewModel;
            MessageBox.Show(string.Format("아이디={0}, 비밀번호={1}", viewModel.LoginID, viewModel.LoginPasswd));
            return true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as LoginViewModel;
            viewModel.LoginID = "myid2";
        }
    }

    /*
    public class LoginViewModel
    {
        private string _loginID;
        public string LoginID
        {
            get { return _loginID; }
            set
            {
                _loginID = value;               
            }
        }

        private string _loginPasswd;
        public string LoginPasswd
        {
            get { return _loginPasswd; }
            set
            {
                _loginPasswd = value;               
            }
        }
    }
    */


    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _loginID;
        public string LoginID
        {
            get { return _loginID; }
            set
            {
                _loginID = value;
                OnPropertyUpdate("LoginID");
            }
        }

        private string _loginPasswd;
        public string LoginPasswd
        {
            get { return _loginPasswd; }
            set
            {
                _loginPasswd = value;
                OnPropertyUpdate("LoginPasswd");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyUpdate(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
