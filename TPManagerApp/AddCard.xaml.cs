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
    public partial class AddCard : Window
    {
        public string Bank { get; set; }
        public long Number { get; set; }
        public decimal StartCash { get; set; }

        public AddCard()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Bank = BankBox.Text;

            if (!long.TryParse(NumberBox.Text, out long cardNumber) || NumberBox.Text.Length < 16 || NumberBox.Text.Length > 16)
            {
                MessageBox.Show("Номер картки повинен містити лише цифри і містити 16 символів");

                return;
            }

            if (!decimal.TryParse(CashBox.Text, out decimal cash))
            {
                MessageBox.Show("Початкова сума введена неправильно");

                return;
            }

            Number = cardNumber;
            StartCash = cash;

            DialogResult = true;
            Close();
        }
    }
}
