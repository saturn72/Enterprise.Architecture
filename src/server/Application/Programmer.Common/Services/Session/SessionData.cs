namespace Programmer.Common.Services.Session
{
    public class SessionData
    {
        public SessionData(string token, string clientId, string resourceId)
        {
            Token = token;
            ClientId = clientId;
            ResourceId = resourceId;
        }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ResourceId { get; set; }
    }
}