using RecipeProject.Models.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeProject.Repository
{
    public interface IRecipeRepository
    {
        public Task<Recipe> UpsertAsync(RecipeCreate recipeCreate, int applicationUserId);
        public Task<PagedResults<Recipe>> GetAllAsync(RecipePaging recipePaging);
        public Task<Recipe> GetAsync(int recipeId);
        public Task<List<Recipe>> GetAllByUserIdAsync(int applicationUserId);
        public Task<List<Recipe>> GetAllFamousAsync();
        public Task<int> DeleteAsync(int recipeId);
    }
}
