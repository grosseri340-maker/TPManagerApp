using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPManagerApp;

namespace FinanceApp
{
    public partial class MainWindow : Window
    {
        Manager manager = new Manager();
        int activeID = 1;

        private void ShowCategories()
        {
            CategoryList.Items.Clear();

            var categories = manager.GetCategories();

            foreach(var c in categories)
            {
                decimal sum = manager.GetSum(c.Id);

                CategoryList.Items.Add($"{c.Name} - {sum}грн");
            }
        }

        private void ShowCards()
        {
            CardsPanel.Children.Clear();

            var cards = manager.GetCards(activeID);

            foreach (var card in cards)
            {
                Border border = new Border
                {
                    Width = 180,
                    Height = 110,
                    CornerRadius = new CornerRadius(15),
                    Margin = new Thickness(5),
                    Background = Brushes.DarkBlue
                };

                StackPanel stack = new StackPanel
                {
                    Margin = new Thickness(10)
                };

                TextBlock typeText = new TextBlock
                {
                    Text = card.CardType,
                    Foreground = Brushes.White,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold
                };

                string num = card.CardNumber.ToString();

                TextBlock numberText = new TextBlock
                {
                    Text = num,
                    Foreground = Brushes.White,
                    FontSize = 14,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                TextBlock cashText = new TextBlock
                {
                    Text = $"{card.Cash} грн",
                    Foreground = Brushes.White,
                    FontSize = 18,
                    TextAlignment = TextAlignment.Right,
                    Margin = new Thickness(0, 15, 0, 0)
                };

                stack.Children.Add(typeText);
                stack.Children.Add(numberText);
                stack.Children.Add(cashText);

                border.Child = stack;

                CardsPanel.Children.Add(border);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ShowCategories();
            ShowCards();
        }

        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            AddCard ACwindow = new AddCard();
            ACwindow.ShowDialog();

            if (ACwindow.DialogResult == true)
            {
                try
                {
                    manager.AddCard(ACwindow.Bank, ACwindow.Number, activeID, ACwindow.StartCash);
                    ShowCards();
                    MessageBox.Show($"Карту {ACwindow.Number} успішно додано");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategory ACwindow = new AddCategory();
            ACwindow.ShowDialog();

            if (ACwindow.DialogResult == true)
            {
                try
                {
                    manager.AddCategory(ACwindow.CategoryName);
                    ShowCategories();
                    MessageBox.Show($"Категорію {ACwindow.CategoryName} успішно додано");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            OperationWindow OPwindow = new OperationWindow(activeID);
            OPwindow.ShowDialog();

            if (OPwindow.DialogResult == true)
            {
                ShowCategories();
                ShowCards();
            }
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow Hwindow = new HistoryWindow();
            Hwindow.ShowDialog();
        }

        private void TopCategory_Click(object sender, RoutedEventArgs e)
        {
            ShowTopWindow STwindow = new ShowTopWindow();
            STwindow.ShowDialog();
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