using RecipeProject.Models.RecipeComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeProject.Repository
{
    public interface IRecipeCommentRepository
    {
        public Task<RecipeComment> UpsertAsync(RecipeCommentCreate recipeCommentCreate, int applicationUserId);
        public Task<List<RecipeComment>> GetAllAsync(int recipeId);
        public Task<RecipeComment> GetAsync(int recipeCommentId);
        public Task<int> DeleteAsync(int recipeCommentId);
    }
}
