using System;
using System.ComponentModel.DataAnnotations;

namespace TPManagerApp
{
    public class CreditCard
    {
        public int Id { get; set; }
        public decimal Cash { get; set; }
        public long CardNumber { get; set; }
        public string CardType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Operation> Operations { get; set; } 
    }
}
