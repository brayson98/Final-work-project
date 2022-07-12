using Dapper;
using Microsoft.Extensions.Configuration;
using RecipeProject.Models.Recipe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeProject.Repository
{
    internal class RecipeRepository : IRecipeRepository
    {
        private readonly IConfiguration _config;
        public RecipeRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<int> DeleteAsync(int recipeId)
        {
            int affectedRows = 0;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                    "Recipe_Delete",
                    new { RecipeId = recipeId },
                    commandType: CommandType.StoredProcedure);


            }
            return affectedRows;
        }

        public async Task<PagedResults<Recipe>> GetAllAsync(RecipePaging recipePaging)
        {
            var results = new PagedResults<Recipe>();
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync("Recipe_GetAll",

                    new
                    {
                        Offset = (recipePaging.Page - 1) * recipePaging.PageSize,
                        PageSize = recipePaging.PageSize
                    }, commandType: CommandType.StoredProcedure))
                {
                    results.Items = multi.Read<Recipe>();

                    results.TotalCount = multi.ReadFirst<int>();
                }
            }
            return results;
        }

        public async Task<List<Recipe>> GetAllByUserIdAsync(int applicationUserId)
        {
            IEnumerable<Recipe> recipes;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                recipes = await connection.QueryAsync<Recipe>(
                    "Recipe_GetByUserId",
                    new { ApplicationUserId = applicationUserId },
                    commandType: CommandType.StoredProcedure);



            }
            return recipes.ToList();
        }

        public async Task<List<Recipe>> GetAllFamousAsync()
        {
            IEnumerable<Recipe> famousRecipes;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                famousRecipes = await connection.QueryAsync<Recipe>(
                    "Recipe_GetAllFamous",
                    new { },
                    commandType: CommandType.StoredProcedure);



            }
            return famousRecipes.ToList();
        }

        public async Task<Recipe> GetAsync(int recipeId)
        {
            Recipe recipe;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                recipe = await connection.QueryFirstOrDefaultAsync<Recipe>(
                    "Recipe_Get",
                    new { RecipeId = recipeId },
                    commandType: CommandType.StoredProcedure);



            }
            return recipe;
        }

        public async Task<Recipe> UpsertAsync(RecipeCreate recipeCreate, int applicationUserId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("RecipeId", typeof(int));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Method", typeof(string));
            dataTable.Columns.Add("Ingredients", typeof(string));
            dataTable.Columns.Add("TimeToCook", typeof(int));
            dataTable.Columns.Add("PhotoId", typeof(int));


            dataTable.Rows.Add(recipeCreate.RecipeId, recipeCreate.Title, recipeCreate.Method, recipeCreate.Ingredients, recipeCreate.TimeToCook, recipeCreate.PhotoId);

            int? newRecipeId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newRecipeId = await connection.ExecuteScalarAsync<int?>(
                    "Recipe_Upsert",
                    new { Recipe = dataTable.AsTableValuedParameter("dbo.RecipeType"), ApplicationUserId = applicationUserId },
                    commandType: CommandType.StoredProcedure
                    );
            }

            newRecipeId = newRecipeId ?? recipeCreate.RecipeId;

            Recipe recipe = await GetAsync(newRecipeId.Value);

            return recipe;
        }
    }
}
