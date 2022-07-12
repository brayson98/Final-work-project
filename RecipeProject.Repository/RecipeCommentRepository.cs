using Dapper;
using Microsoft.Extensions.Configuration;
using RecipeProject.Models.RecipeComment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeProject.Repository
{
    public class RecipeCommentRepository : IRecipeCommentRepository
    {
        private readonly IConfiguration _config;
        public RecipeCommentRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<int> DeleteAsync(int recipeCommentId)
        {
            int affectedRows = 0;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                    "RecipeComment_Delete",
                    new { RecipeCommentId = recipeCommentId },
                    commandType: CommandType.StoredProcedure);


            }
            return affectedRows;
        }

        public async Task<List<RecipeComment>> GetAllAsync(int recipeId)
        {
            IEnumerable<RecipeComment> recipeComments;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                recipeComments = await connection.QueryAsync<RecipeComment>(
                    "RecipeComment_GetAll",
                    new { RecipeId = recipeId },
                    commandType: CommandType.StoredProcedure);



            }
            return recipeComments.ToList();
        }

        public async Task<RecipeComment> GetAsync(int recipeCommentId)
        {
            RecipeComment recipeComment;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                recipeComment = await connection.QueryFirstOrDefaultAsync<RecipeComment>(
                    "RecipeComment_Get",
                    new { RecipeCommentId = recipeCommentId },
                    commandType: CommandType.StoredProcedure);



            }
            return recipeComment;
        }

        public async Task<RecipeComment> UpsertAsync(RecipeCommentCreate recipeCommentCreate, int applicationUserId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("RecipeCommentId", typeof(int));
            dataTable.Columns.Add("ParentRecipeCommentId", typeof(int));
            dataTable.Columns.Add("RecipeId", typeof(int));
            dataTable.Columns.Add("Content", typeof(string));


            dataTable.Rows.Add
                (recipeCommentCreate.RecipeCommentId,
                recipeCommentCreate.ParentRecipeCommentId,
                recipeCommentCreate.RecipeId,
               recipeCommentCreate.Content);

            int? newRecipeCommentId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newRecipeCommentId = await connection.ExecuteScalarAsync<int?>(
                    "RecipeComment_Upsert",
                    new
                    {
                        RecipeComment = dataTable.AsTableValuedParameter("dbo.RecipeCommentType"),
                        ApplicationUserId = applicationUserId
                    },
                    commandType: CommandType.StoredProcedure);
            }

            newRecipeCommentId = newRecipeCommentId ?? recipeCommentCreate.RecipeCommentId;

            RecipeComment recipeComment = await GetAsync(newRecipeCommentId.Value);
            return recipeComment;
        }
    }
}
