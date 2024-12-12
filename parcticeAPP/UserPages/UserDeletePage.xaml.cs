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
    /// Логика взаимодействия для UserDeletePage.xaml
    /// </summary>
    public partial class UserDeletePage : Page
    {
        public string token;
        ExchangeUser thisUser;
        public UserDeletePage(string token , ExchangeUser user)
        {
            InitializeComponent();
            this.token = token;
            thisUser = user;
            reloadUserComboBox();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            System.Net.HttpStatusCode code = DataContex.DeleteUser(token , DataContex.NumbBeforeSpace(usersComboBox.SelectedItem as string));
            if (code == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("Успешное удаление"); reloadUserComboBox();
            }
        }

        private void reloadUserComboBox()
        {
            usersComboBox.Items.Clear();
            ExchangeUser[] data = DataContex.GetUsersWithDetails(token);
            foreach (var U in data.Where(x => x.id != thisUser.id))
            {
                usersComboBox.Items.Add($"{U.id} {U.email} {U.role}");
            }
            usersComboBox.SelectedIndex = 0;
        }
    }
}
