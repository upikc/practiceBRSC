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

namespace parcticeAPP.AuthPages
{
    public partial class RegPage : Page
    {
        public AuthWindow AuthWindow;
        public RegPage(AuthWindow window)
        {
            InitializeComponent();
            AuthWindow = window;
        }

        private void RegBtnClick(object sender, RoutedEventArgs e)
        {
            if (DataContex.ContainsNullOrWhiteSpace([MailTbox.Text, PassTbox.Text, NameTbox.Text]))
            {
                MessageBox.Show("Заполните поля");
                return;
            }

            RegisterModel regData = new RegisterModel() { email = MailTbox.Text, password = PassTbox.Text, name = NameTbox.Text };
            var (statusCode, loginResponse) = DataContex.Register(regData);

            if ((int)statusCode == 200)
            {
                MessageBox.Show($"Успешная регистрация, добро пожаловать {loginResponse.user.name}");
                AuthWindow.OpenUserWindow(loginResponse);
            }
            else if ((int)statusCode == 409)
                MessageBox.Show($"Почта занята");
            else
                MessageBox.Show($"Ошибка : {(int)statusCode}");
        }
    }
}
