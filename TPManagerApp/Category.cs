using System.ComponentModel.DataAnnotations;

namespace TPManagerApp
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Назва категорії обов'язкова")]
        [MinLength(1)]
        public string Name { get; set; }
        public ICollection<Operation> Operations { get; set; }
    }
}
