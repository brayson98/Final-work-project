using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace RecipeProject.Models.Recipe
{
    public class RecipeCreate
    {
        public int RecipeId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [MinLength(10, ErrorMessage = "Must be at least 10-50 characters")]
        [MaxLength(50, ErrorMessage = "Must be at least 10-50 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Method is required")]
        [MinLength(30)]
        [MaxLength(3000)]
        public string Method { get; set; }
        [Required(ErrorMessage = "Ingredients are required")]
        [MinLength(30)]
        [MaxLength(3000)]
        public string Ingredients { get; set; }
        public int? TimeToCook { get; set; }
        
        public int? PhotoId { get; set; }
    }
}
