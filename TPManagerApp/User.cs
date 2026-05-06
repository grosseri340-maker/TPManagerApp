using System.ComponentModel.DataAnnotations;

namespace TPManagerApp
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        [MinLength(1)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Login is required")]
        [MinLength(3)]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6)]
        public string Password { get; set; }
        public ICollection<CreditCard> CreditCards { get; set; }
    }
}
