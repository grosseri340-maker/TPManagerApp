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
    public partial class SelectionWindow : Window
    {
        public SelectionWindow()
        {
            InitializeComponent();
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close(); // Закриваємо вікно вибору
        }

        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            Registration_Window registration = new Registration_Window();
            registration.Show();
            this.Close(); // Закриваємо вікно вибору
        }
    }
}
