using System.Windows;

namespace FinanceApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Додати карту");
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Додати категорію");
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Банкова операція");
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Історія витрат");
        }

        private void TopCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Топ категорій");
        }

        private void Percent_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Відсоток категорії");
        }

        private void ChangeAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Зміна акаунту");
        }
    }
}