using practiceAPP.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Xml.Linq;

namespace parcticeAPP.AuthPages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthWindow AuthWindow;
        public AuthPage(AuthWindow window)
        {
            InitializeComponent();
            AuthWindow = window;
        }

        private void AuthBtnClick(object sender, RoutedEventArgs e)
        {
            if (DataContex.ContainsNullOrWhiteSpace([MailTbox.Text, PassTbox.Text]))
            {
                MessageBox.Show("Заполните поля");
                return;
            }

            LoginModel loginData = new LoginModel() {Email = MailTbox.Text , Password = PassTbox.Text};
            var (statusCode, loginResponse) = DataContex.Login(loginData);

            if ((int)statusCode == 200)
            {
                MessageBox.Show($"Добро пожаловать {loginResponse.user.name} вы зашли за {loginResponse.user.role}");
                AuthWindow.OpenUserWindow(loginResponse);
            }
            else if ((int)statusCode == 400)
                MessageBox.Show($"не верные данные");
            else 
                MessageBox.Show($"Ошибка : {(int)statusCode}");
        }
    }
}
