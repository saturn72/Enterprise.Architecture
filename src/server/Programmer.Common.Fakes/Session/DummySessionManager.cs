using System.Threading.Tasks;
using Programmer.Common.Services.Session;

namespace Programmer.Common.Fakes.Session
{
    public class DummySessionManager:ISessionManager
    {
        public Task<SessionData> GetSessionByToken(string sessionToken)
        {
            return Task.FromResult(new SessionData(sessionToken, "client-id", "resource-id"));
        }

        public Task<bool> IsResourceConnected(string serialNumber)
        {
            return Task.FromResult(true);
        }
    }
}
