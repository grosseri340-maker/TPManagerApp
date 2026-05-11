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

        // 1. Метод для кнопки "Зареєструватися"
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

            // Ховаємо реєстрацію, показуємо SuccessPanel
            RegistrationPanel.Visibility = Visibility.Collapsed;
            SuccessPanel.Visibility = Visibility.Visible;

            MessageBox.Show("Реєстрація успішна! Тепер натисніть кнопку підтвердження.");
        }

        // 2. Метод для кнопки "Продовжити" (яка на SuccessPanel)
        // ПЕРЕКОНАЙСЯ, ЩО В XAML У ЦІЄЇ КНОПКИ Є Click="Button_Click"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Створюємо головне вікно
            MainWindow main = new MainWindow();

            // Показуємо його
            main.Show();

            // Закриваємо поточне вікно реєстрації
            this.Close();
        }
    }
}