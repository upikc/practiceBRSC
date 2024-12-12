using System.Text;
using System.Windows;
using parcticeAPP.UserPages;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using practiceAPP.UserModels;
using parcticeAPP.ResponseModels;


namespace parcticeAPP
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent(); SwapToAuth();
        }


        private void SwapToAuth()
        {
            FrameSwapButton.Content = "Регистрация >";
            frame.Content = new AuthPages.AuthPage(this);
        }
        private void SwapToReg()
        {
            FrameSwapButton.Content = "Авторизация >";
            frame.Content = new AuthPages.RegPage(this);
        }

        private void FrameSwapButton_Click(object sender, RoutedEventArgs e)
        {
            if (FrameSwapButton.Content == "Регистрация >")
                SwapToReg();
            else
                SwapToAuth();
        }
        public void OpenUserWindow(LoginResponse loginResponse)
        {
            new UserWindow(loginResponse).Show();
            this.Close();
        }
    }
}