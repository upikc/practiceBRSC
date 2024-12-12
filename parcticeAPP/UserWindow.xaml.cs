using parcticeAPP.ResponseModels;
using parcticeAPP.UserPages;
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

namespace parcticeAPP
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public ExchangeUser thisUser;
        public string token;
        public UserWindow(LoginResponse loginResponse)
        {
            InitializeComponent();
            thisUser = loginResponse.user;
            token = loginResponse.token;
            Title = $"Пользователь {thisUser.name} Роль {thisUser.role}";

            if (thisUser.role == "User")
            {
                UsersViewer.Visibility = Visibility.Collapsed;
                UsersDeleter.Visibility = Visibility.Collapsed;
            }
            else if (thisUser.role == "Manager")
                UsersDeleter.Visibility = Visibility.Collapsed;


        }

        private void ShowViewDatagridPage_Users(object sender, MouseButtonEventArgs e)
        {
            ExchangeUser[] data = DataContex.GetUsersWithDetails(token);
            frame.Content = new ViewDatagridPage(data);
        }

        private void ShowDataChangePage(object sender, MouseButtonEventArgs e)
        {
            frame.Content = new DataEditPage(thisUser , token , this);
        }

        private void AuthWindowShow(object sender, MouseButtonEventArgs e)
        {
            toAuthWindow();
        }

        public void toMainPage()
        {
            ExchangeUser[] data = DataContex.GetUsersWithDetails(token);
            frame.Content = new ViewDatagridPage(data);
        }
        public void toAuthWindow()
        {
            new AuthWindow().Show();
            this.Close();
        }

        private void ViewProfile(object sender, MouseButtonEventArgs e)
        {
            frame.Content = new ProfilePage(thisUser);
        }

        private void ShowDeleteUserPage(object sender, MouseButtonEventArgs e)
        {
            frame.Content = new UserDeletePage(token , thisUser);
        }
    }
}
