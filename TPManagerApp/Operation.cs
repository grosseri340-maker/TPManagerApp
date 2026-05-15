using System.ComponentModel.DataAnnotations;

namespace TPManagerApp
{
    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public decimal CashAmount { get; set; }
        public DateTime Date { get; set; }
        public int CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
