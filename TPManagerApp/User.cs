using System.ComponentModel.DataAnnotations;

namespace TPManagerApp
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ім'я користувача є обов'язковим")]
        [MinLength(1)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Логін є обов'язковим")]
        [MinLength(3)]
        public string Login { get; set; }
        [Required(ErrorMessage = "Пароль є обов'язковим")]
        [MinLength(6)]
        public string Password { get; set; }
        public ICollection<CreditCard> CreditCards { get; set; }
    }
}
