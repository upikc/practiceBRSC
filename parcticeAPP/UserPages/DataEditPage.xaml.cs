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

namespace parcticeAPP.UserPages
{
    /// <summary>
    /// Логика взаимодействия для DataEditPage.xaml
    /// </summary>
    public partial class DataEditPage : Page
    {
        public ExchangeUser thisUser;
        public string token;
        public UserWindow thisWindow;
        public DataEditPage(ExchangeUser user , string token , UserWindow userWindow)
        {
            thisUser = user;
            this.token = token;
            thisWindow = userWindow;
            InitializeComponent();

            if (thisUser.role == "Admin")
            {
                ComboBoxWriter();
            }
            else if (thisUser.role == "Manager")
            {
                ComboBoxWriter();
                PassTbox.Visibility = Visibility.Collapsed;
                PassLabel.Visibility = Visibility.Collapsed;
                roleComboBox.Visibility = Visibility.Collapsed;
                roleLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                usersComboBoxlabel.Visibility = Visibility.Collapsed;
                roleLabel.Visibility = Visibility.Collapsed;
                PassLabel.Visibility = Visibility.Collapsed;

                usersComboBox.Visibility = Visibility.Collapsed;
                roleComboBox.Visibility = Visibility.Collapsed;
                PassTbox.Visibility = Visibility.Collapsed;
                setUserDataInTboxs(user.id);
            }


        }

        private void confirm(object sender, RoutedEventArgs e)
        {
            if (thisUser.role == "Manager")
            {
                managerSaveData();
            }
            else if (thisUser.role == "Admin")
            {
                AdminSaveData();
            }
            else if (thisUser.role == "User")
            {
                UserSaveData();
            }
            else
                MessageBox.Show("Проблема с ролью");

        }
        private void ComboBoxWriter()
        {
            ExchangeUser[] data = DataContex.GetUsersWithDetails(token);
            foreach (var U in data)
            {
                usersComboBox.Items.Add($"{U.id} {U.email} {U.role}");
            }
            usersComboBox.SelectedIndex = 0;

            roleComboBox.Items.Add($"1 Admin");
            roleComboBox.Items.Add($"2 Manager");
            roleComboBox.Items.Add($"3 User");
            roleComboBox.SelectedIndex = 0;
        }

        private void usersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = DataContex.NumbBeforeSpace(usersComboBox.SelectedItem as string);
            setUserDataInTboxs(id);

        }
        
        private void setUserDataInTboxs(int id)
        {
            (System.Net.HttpStatusCode statusCode, ExchangeUser? user) response = DataContex.GetUserById(token, id);
            if (response.statusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("Ошибка при получении пользователя : " + response.statusCode);
                return;
            }
            ExchangeUser? user = response.user;
            NameTbox.Text = user.name;
            MailTbox.Text = user.email;
            //PassTbox.Text = "x" ;
        }

        private void managerSaveData()
        {
            var id = DataContex.NumbBeforeSpace(usersComboBox.SelectedItem as string);
            if (id == null || DataContex.ContainsNullOrWhiteSpace([NameTbox.Text, MailTbox.Text]))
            {
                MessageBox.Show("Ошибка, заполните поля");
                return;
            }

            System.Net.HttpStatusCode returnCode = DataContex.UpdateUser(token, id, new UpdateUserModel { Email = MailTbox.Text, Name = NameTbox.Text }); //менеджер и юзер
            if (returnCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("успешно");
                if (id == thisUser.id)
                {
                    MessageBox.Show("Ваши данные изменены, перезайдите в аккаунт");
                    thisWindow.toAuthWindow();
                }
            }
            else
            {
                MessageBox.Show("почта уже используется");
            }
        }

        private void UserSaveData()
        {
            var id = thisUser.id;
            if (id == null || DataContex.ContainsNullOrWhiteSpace([NameTbox.Text, MailTbox.Text]))
            {
                MessageBox.Show("Ошибка, заполните поля");
                return;
            }

            System.Net.HttpStatusCode returnCode = DataContex.UpdateUser(token, id, new UpdateUserModel { Email = MailTbox.Text, Name = NameTbox.Text }); //менеджер и юзер
            if (returnCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("успешно");
                MessageBox.Show("Ваши данные изменены, перезайдите в аккаунт");
                thisWindow.toAuthWindow();
            }
            else
            {
                MessageBox.Show("почта уже используется");
            }
        }

        private void AdminSaveData()
        {
            var id = DataContex.NumbBeforeSpace(usersComboBox.SelectedItem as string);
            if (id == null || DataContex.ContainsNullOrWhiteSpace([NameTbox.Text, MailTbox.Text]))
            {
                MessageBox.Show("Ошибка, заполните поля");
                return;
            }


            System.Net.HttpStatusCode returnCode = DataContex.UpdateAllUserData(token, id, new UpdateUserModelWithPass 
                { Email = MailTbox.Text,
                    Name = NameTbox.Text , 
                    Password = PassTbox.Text , 
                    Role_id = DataContex.NumbBeforeSpace(roleComboBox.SelectedItem as string) });//админ




            if (returnCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("успешно");
                if (id == thisUser.id)
                {
                    MessageBox.Show("Ваши данные изменены, перезайдите в аккаунт");
                    thisWindow.toAuthWindow();
                }
            }
            else
            {
                MessageBox.Show("почта уже используется");
            }
        }
    }
}
