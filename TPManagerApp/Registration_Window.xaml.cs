using FinanceApp;
using Microsoft.Win32;
using System.Windows;

namespace TPManagerApp
{
    public partial class Registration_Window : Window
    {
        Manager manager = new Manager();

        public Registration_Window()
        {

            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password != PasswordBox2.Password)
            {
                MessageBox.Show("Паролі не співпадають!");
                return;
            }

            if (NameBox.Text == "" || PasswordBox1.Password == "" || EmailBox.Text == "")
            {
                MessageBox.Show("Заповніть всі поля!");
                return;
            }

            if (manager.userExists(EmailBox.Text))
            {
                MessageBox.Show("Користувач з таким логіном вже існує!");
                return;
            }

            try
            {
                User user = manager.Register(NameBox.Text, EmailBox.Text, PasswordBox1.Password);
                MessageBox.Show("Користувача успішно додано!");

                MainWindow window1 = new MainWindow(user.Id);
                window1.ShowDialog();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}