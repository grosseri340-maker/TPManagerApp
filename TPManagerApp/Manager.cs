using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TPManagerApp
{
    internal class Manager
    {
        public enum PeriodType
        {
            Day,
            Week,
            Month,
            Year
        }

        DBContext db = new DBContext();

        public void AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty");
            }
            var category = new Category
            {
                Name = name
            };
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public List<Category> GetCategories()
        {
            return db.Categories.ToList();
        }
        public List<CreditCard> GetCards(int userId)
        {
            return db.CreditCards
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public void AddCard(string bank, long number, int userId, decimal cash = 0)
        {
            if (cash < 0)
                throw new ArgumentException("Initial cash cannot be negative");

            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new Exception("User not found");

            var card = new CreditCard
            {
                Cash = cash,
                CardType = bank,
                CardNumber = number,
                UserId = userId
            };

            var context = new ValidationContext(card);
            Validator.ValidateObject(card, context, true);

            db.CreditCards.Add(card);
            db.SaveChanges();
        }

        public void AddOperation(int userId, int creditCardId, decimal amount, int categoryId, string name = "Операція без назви")
        {
            var card = db.CreditCards
                .FirstOrDefault(c => c.Id == creditCardId && c.UserId == userId);

            if (card == null)
                throw new Exception("Картку не знайдено");

            if (card.Cash < amount)
                throw new Exception("Недостатньо коштів");

            card.Cash -= amount;

            var operation = new Operation
            {
                Name = name,
                CashAmount = amount,
                Date = DateTime.Now,
                CreditCardId = card.Id,
                CategoryId = categoryId
            };

            db.Operations.Add(operation);

            db.SaveChanges();
        }

        public static (DateTime start, DateTime end) GetPeriodRange(DateTime date, PeriodType period)
        {
            switch (period)
            {
                case PeriodType.Day:
                    return (date.Date, date.Date.AddDays(1));

                case PeriodType.Week:
                    var startOfWeek = date.Date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
                    return (startOfWeek, startOfWeek.AddDays(7));

                case PeriodType.Month:
                    var startOfMonth = new DateTime(date.Year, date.Month, 1);
                    return (startOfMonth, startOfMonth.AddMonths(1));

                case PeriodType.Year:
                    var startOfYear = new DateTime(date.Year, 1, 1);
                    return (startOfYear, startOfYear.AddYears(1));

                default:
                    throw new ArgumentException("Invalid period");
            }
        }
        public void AddCashToCard(int cardId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");

            var card = db.CreditCards.FirstOrDefault(c => c.Id == cardId);

            if (card == null)
                throw new Exception("Card not found");

            card.Cash += amount;

            db.SaveChanges();

            Console.WriteLine($"Successfully added {amount} to card.");
            Console.WriteLine($"Current balance: {card.Cash}");
        }

        public List<Operation> GetOperations(DateTime date, PeriodType period)
        {
            var range = GetPeriodRange(date, period);

            return db.Operations
                .Include(o => o.Category)
                .Include(o => o.CreditCard)
                .Where(o => o.Date >= range.start && o.Date < range.end)
                .OrderByDescending(o => o.Date)
                .ToList();
        }

        public void ShowExpensePercentages(DateTime date, PeriodType period)
        {
            var range = GetPeriodRange(date, period);

            var operations = db.Operations
                .Include(o => o.Category)
                .Where(o => o.Date >= range.start && o.Date < range.end)
                .ToList();

            if (!operations.Any())
            {
                Console.WriteLine("No operations found.");
                return;
            }

            decimal totalExpenses = operations.Sum(o => o.CashAmount);

            var grouped = operations
                .GroupBy(o => o.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(x => x.CashAmount),
                    Percent = (g.Sum(x => x.CashAmount) / totalExpenses) * 100
                })
                .OrderByDescending(x => x.Total);

            Console.WriteLine($"Expense percentages for {period}:");

            foreach (var item in grouped)
            {
                Console.WriteLine(
                    $"{item.Category} : " +
                    $"{item.Total} ({item.Percent}%)"
                );
            }

            Console.WriteLine($"Total expenses: {totalExpenses}");
        }

        public class TopCategory()
        {
            public string CategoryName { get; set; }
            public decimal TotalSpent { get; set; }
        }

        public List<TopCategory> GetTopCategories(DateTime date, PeriodType period)
        {
            var range = GetPeriodRange(date, period);

            return db.Operations
                .Include(o => o.Category)
                .Where(o => o.Date >= range.start && o.Date < range.end)
                .GroupBy(o => o.Category.Name)
                .Select(g => new TopCategory
                {
                    CategoryName = g.Key,
                    TotalSpent = g.Sum(x => x.CashAmount)
                })
                .OrderByDescending(x => x.TotalSpent)
                .ToList();
        }

        public decimal GetSum(int categoryId)
        {
            DateTime startDate = DateTime.Now.AddMonths(-1);
            DateTime endDate = DateTime.Now;

            var totalExpenses = db.Operations
                .Where(o => o.CategoryId == categoryId
                         && o.Date >= startDate
                         && o.Date <= endDate)
                .Sum(o => o.CashAmount);

            return totalExpenses;
        }
    }
}
