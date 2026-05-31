using FinanceApp;
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
using System.Windows.Shapes;

namespace TPManagerApp
{
    public partial class LoginWindow : Window
    {
        private Manager manager = new Manager();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var user = manager.GetUsers()
                .FirstOrDefault(u => 
                    u.UserName == txtUser.Text &&
                    u.Password == txtPass.Password);

            if (user != null)
            {
                MessageBox.Show("Вхід успішний!", $"Вітаємо {txtUser.Text}");

                MainWindow mainWindow = new MainWindow(user.Id);
                mainWindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль!", "Помилка");
                txtPass.Clear();
            }
        }
    }
}
