using SmartVault.BusinessLogic.BusinessObjects;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartVault.BusinessLogic.Interfaces
{
    public interface IOAuthService
    {
        Task<string> VerifyToken(string token);
        Task<OAuthUser> RetrieveOAuthProfile(string accountId);
        Task<string> CreateRefreshToken(IEnumerable<Claim> claims, OAuthUser user);
        Task<string> CreateAccessToken(IEnumerable<Claim> claims, OAuthUser user);
    }
}
