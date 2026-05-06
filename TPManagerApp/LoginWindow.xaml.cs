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
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Проста перевірка: логін admin, пароль 1234
            if (txtUser.Text == "admin" && txtPass.Password == "1234")
            {
                MessageBox.Show("Вхід успішний!", "Вітаємо");
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль!", "Помилка");
                txtPass.Clear();
            }
        }
        }
}
