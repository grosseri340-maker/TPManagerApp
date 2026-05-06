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

        public static void ShowTopCategories(DBContext context, PeriodType period, DateTime date, int top = 5)
        {
            var (start, end) = GetPeriodRange(date, period);

            var result = context.Operations
                .Where(o => o.Date >= start && o.Date < end)
                .GroupBy(o => o.Category.Name)
                .Select(g => new 
                {
                    Category = g.Key,
                    Total = g.Sum(o => o.CashAmount)
                })
                .OrderByDescending(x => x.Total)
                .Take(top)
                .ToList();

            Console.WriteLine($"Top {top} categories for {period}:");

            foreach (var item in result)
            {
                Console.WriteLine($"{item.Category} - {item.Total}");
            }
        }
    }
}
