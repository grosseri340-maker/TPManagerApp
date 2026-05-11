using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void AddCard(int userId, decimal cash)
        {
            if (cash < 0)
                throw new ArgumentException("Initial cash cannot be negative");

            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new Exception("User not found");

            var card = new CreditCard
            {
                Cash = cash,
                UserId = userId
            };

            var context = new ValidationContext(card);
            Validator.ValidateObject(card, context, true);

            db.CreditCards.Add(card);
            db.SaveChanges();
        }

        public void AddOperation(int userId, int creditCardId, decimal amount, int categoryId)
        {
            var card = db.CreditCards
                .FirstOrDefault(c => c.Id == creditCardId && c.UserId == userId);

            if (card == null)
            {
                Console.WriteLine("Card not found!");
                return;
            }

            while (true)
            {
                if (card.Cash >= amount)
                {
                    card.Cash -= amount;

                    var operation = new Operation
                    {
                        CashAmount = amount,
                        Date = DateTime.Now,
                        CreditCardId = card.Id,
                        CategoryId = categoryId
                    };

                    db.Operations.Add(operation);
                    db.SaveChanges();

                    Console.WriteLine("Operation successful!");
                    break;
                }
                else
                {
                    Console.WriteLine("Not enough money on this card!");
                    Console.WriteLine("1 - Cancel operation");
                    Console.WriteLine("2 - Choose another card");

                    var choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Console.WriteLine("Operation canceled.");
                        break;
                    }
                    else if (choice == "2")
                    {
                        var userCards = db.CreditCards
                            .Where(c => c.UserId == userId)
                            .ToList();

                        Console.WriteLine("Your cards:");

                        foreach (var c in userCards)
                        {
                            Console.WriteLine($"Id: {c.Id}, Cash: {c.Cash}");
                        }

                        Console.Write("Enter new card Id: ");
                        if (int.TryParse(Console.ReadLine(), out int newCardId))
                        {
                            var newCard = userCards.FirstOrDefault(c => c.Id == newCardId);

                            if (newCard != null)
                            {
                                card = newCard;
                            }
                            else
                            {
                                Console.WriteLine("Invalid card id!");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice!");
                    }
                }
            }
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

        public void ShowExpenseHistory(DateTime date, PeriodType period)
        {
            var range = GetPeriodRange(date, period);

            var operations = db.Operations
                .Include(o => o.Category)
                .Include(o => o.CreditCard)
                .Where(o => o.Date >= range.start && o.Date < range.end)
                .OrderByDescending(o => o.Date)
                .ToList();

            if (!operations.Any())
            {
                Console.WriteLine("No operations found.");
                return;
            }

            Console.WriteLine($"Expense history for {period}:");

            foreach (var op in operations)
            {
                Console.WriteLine(
                    $"Date: {op.Date} | " +
                    $"Category: {op.Category.Name} | " +
                    $"Amount: {op.CashAmount}"
                );
            }

            Console.WriteLine($"Total expenses: {operations.Sum(o => o.CashAmount)}");
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

        public void ShowTopCategories(DateTime date, PeriodType period)
        {
            var range = GetPeriodRange(date, period);

            var topCategories = db.Operations
                .Include(o => o.Category)
                .Where(o => o.Date >= range.start && o.Date < range.end)
                .GroupBy(o => o.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSpent = g.Sum(x => x.CashAmount)
                })
                .OrderByDescending(x => x.TotalSpent)
                .ToList();

            if (!topCategories.Any())
            {
                Console.WriteLine("No operations found.");
                return;
            }

            Console.WriteLine($"Top categories for {period}:");

            int place = 1;

            foreach (var category in topCategories)
            {
                Console.WriteLine(
                    $"{place}. {category.Category} - {category.TotalSpent}"
                );

                place++;
            }
        }

        public decimal GetSum(int categoryId)
        {
            DateTime startDate = DateTime.Now.AddMonths(-1);
            DateTime endDate = DateTime.Now;

            var totalExpenses = db.Operations
                .Where(o => o.CategoryId == categoryId
                         && o.Date >= startDate
                         && o.Date <= endDate)
                .Sum(o => (decimal?)o.CashAmount) ?? 0;

            return totalExpenses;
        }
    }
}
