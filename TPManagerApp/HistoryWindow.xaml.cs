using Azure;
using Microsoft.EntityFrameworkCore;
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
using TPManagerApp;

namespace TPManagerApp
{
    public partial class HistoryWindow : Window
    {
        Manager manager = new Manager();
        public HistoryWindow()
        {
            InitializeComponent();

            PeriodBox.SelectedIndex = 0;
        }

        private void LoadHistory(Manager.PeriodType period)
        {
            HistoryList.Items.Clear();

            var operations = manager.GetOperations(DateTime.Now, period);

            if (!operations.Any())
            {
                HistoryList.Items.Add("Операцій не знайдено.");
                return;
            }

            foreach (var op in operations)
            {
                HistoryList.Items.Add($" {op.Date} | {op.Category.Name} | {op.CashAmount}грн");
            }
        }

        private void PeriodBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PeriodBox.SelectedIndex == 0)
                LoadHistory(Manager.PeriodType.Day);

            else if (PeriodBox.SelectedIndex == 1)
                LoadHistory(Manager.PeriodType.Week);

            else if (PeriodBox.SelectedIndex == 2)
                LoadHistory(Manager.PeriodType.Month);

            else if (PeriodBox.SelectedIndex == 3)
                LoadHistory(Manager.PeriodType.Year);
        }
    }
}

