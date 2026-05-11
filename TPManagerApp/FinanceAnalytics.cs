using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPManagerApp
{
    public class FinanceAnalytics
    {
        // Метод для розрахунку відсотка
        public double CalculatePercentage(double total, double categoryAmount)
        {
            if (total <= 0) return 0; // Захист від ділення на нуль
            double result = (categoryAmount / total) * 100;
            return Math.Round(result, 1); // Повертає напр. 15.5
        }
    }
}