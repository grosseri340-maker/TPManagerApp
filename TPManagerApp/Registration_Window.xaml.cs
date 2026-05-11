using FinanceApp;
using System.Windows;

namespace TPManagerApp
{
    public partial class Registration_Window : Window
    {
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

            if (NameBox.Text == "" || SurnameBox.Text == "" || EmailBox.Text == "")
            {
                MessageBox.Show("Заповніть всі поля!");
                return;
            }
        
            RegistrationPanel.Visibility = Visibility.Hidden;
            SuccessPanel.Visibility = Visibility.Visible;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectionWindow selection = new SelectionWindow();
            selection.Show();
            this.Close();
        }

    }
}