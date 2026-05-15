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
    public partial class OperationWindow : Window
    {
        Manager manager = new Manager();

        private int userId;

        public OperationWindow(int activeID)
        {
            InitializeComponent();

            userId = activeID;

            LoadData();
        }

        private void LoadData()
        {
            CardComboBox.ItemsSource = manager.GetCards(userId);

            CategoryComboBox.ItemsSource = manager.GetCategories();
        }

        private void AddOperation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameBox.Text;

                if (!decimal.TryParse(AmountBox.Text, out decimal amount))
                {
                    MessageBox.Show("Неправильна сума!");
                    return;
                }

                var selectedCard = CardComboBox.SelectedItem as CreditCard;

                var selectedCategory = CategoryComboBox.SelectedItem as Category;

                if (selectedCard == null || selectedCategory == null)
                {
                    MessageBox.Show("Виберіть карту і категорію!");
                    return;
                }

                manager.AddOperation(
                    userId,
                    selectedCard.Id,
                    amount,
                    selectedCategory.Id,
                    name
                );

                MessageBox.Show("Операцію додано!");

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}