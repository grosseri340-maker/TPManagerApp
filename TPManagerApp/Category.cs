using System.ComponentModel.DataAnnotations;

namespace TPManagerApp
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        [MinLength(1)]
        public string Name { get; set; }
        public ICollection<Operation> Operations { get; set; }
    }
}
