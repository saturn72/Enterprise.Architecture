using System.Threading.Tasks;

namespace Programmer.Common.Services.Session
{
    public static class SessionManagerExtensions
    {
        public static async Task<bool> IsSessionExists(this ISessionManager sessionManager, string token)
        {
            return await sessionManager.GetSessionByToken(token) != null;
        }
    }
}