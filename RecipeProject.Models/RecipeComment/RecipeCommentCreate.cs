using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeProject.Models.RecipeComment
{
    public class RecipeCommentCreate
    {
        public int RecipeCommentId { get; set; }
        public int? ParentRecipeCommentId { get; set; }
        public int RecipeId { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [MinLength(10, ErrorMessage = "Must be between 10 and 300 characters")]
        [MaxLength(300, ErrorMessage = "Must be between 10 and 300 characters")]
        public string Content { get; set; }
    }
}
