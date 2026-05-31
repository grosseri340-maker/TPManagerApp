using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TPManagerApp
{
    public partial class ExpenseGraph : Window
    {
        Manager manager = new Manager();

        public ExpenseGraph()
        {
            InitializeComponent();

            PeriodBox.SelectedIndex = 0;
        }

        private void LoadChart(Manager.PeriodType period)
        {
            var data = manager.GetExpensePercentages(System.DateTime.Now,period);

            var chartData = data.Select(x => new
            {
                x.Category,
                x.Percent,

                BarHeight = (double)x.Percent * 3
            }).ToList();

            ChartItems.ItemsSource = chartData;
        }

        private void PeriodBox_SelectionChanged(object sender,SelectionChangedEventArgs e)
        {
            if (PeriodBox.SelectedIndex == 0)
                LoadChart(Manager.PeriodType.Day);

            else if (PeriodBox.SelectedIndex == 1)
                LoadChart(Manager.PeriodType.Week);

            else if (PeriodBox.SelectedIndex == 2)
                LoadChart(Manager.PeriodType.Month);

            else if (PeriodBox.SelectedIndex == 3)
                LoadChart(Manager.PeriodType.Year);
        }
    }
}