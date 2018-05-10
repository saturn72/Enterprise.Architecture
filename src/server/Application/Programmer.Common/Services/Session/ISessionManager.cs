using System.Threading.Tasks;

namespace Programmer.Common.Services.Session
{
    public interface ISessionManager
    {
        Task<SessionData> GetSessionByToken(string sessionToken);
        Task<bool> IsResourceConnected(string serialNumber);
    }
}
