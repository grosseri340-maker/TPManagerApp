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
    public partial class ShowTopWindow : Window
    {
        Manager manager = new Manager();

        public ShowTopWindow()
        {
            InitializeComponent();

            PeriodBox.SelectedIndex = 0;
        }
        private void LoadTop(Manager.PeriodType period)
        {
            TopList.Items.Clear();

            var top = manager.GetTopCategories(DateTime.Now, period);

            if (!top.Any())
            {
                TopList.Items.Add("Операцій не знайдено.");
                return;
            }

            int place = 1;

            foreach (var c in top)
            {
                TopList.Items.Add($"{place}. {c.CategoryName} | {c.TotalSpent}");

                place++;
            }
        }

        private void PeriodBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PeriodBox.SelectedIndex == 0)
                LoadTop(Manager.PeriodType.Day);

            else if (PeriodBox.SelectedIndex == 1)
                LoadTop(Manager.PeriodType.Week);

            else if (PeriodBox.SelectedIndex == 2)
                LoadTop(Manager.PeriodType.Month);

            else if (PeriodBox.SelectedIndex == 3)
                LoadTop(Manager.PeriodType.Year);
        }
    }
}
