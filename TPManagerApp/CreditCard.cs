using System;
using System.ComponentModel.DataAnnotations;

namespace TPManagerApp
{
    public class CreditCard
    {
        public int Id { get; set; }
        public decimal Cash { get; set; }
        public long CardNumber { get; set; }
        [Required(ErrorMessage = "Номер карти є обов'язковим")]
        [Range(16, 16)]
        public string CardType { get; set; }
        [Required(ErrorMessage = "Назва банку є обов'язковою")]
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Operation> Operations { get; set; } 
    }
}
