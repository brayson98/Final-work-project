using Microsoft.AspNetCore.Identity;
using RecipeProject.Models.Account;

namespace RecipeProject.Repository
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken);
        public Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedUsername, CancellationToken cancellationToken);
    }
}