using System.Collections.Generic;

namespace Programmer.Common.Services.Command
{
    public class CommandRequest
    {
        private IDictionary<string, object> _parameters;

        public CommandRequest(string name, string sessionToken)
        {
            Name = name;
            SessionToken = sessionToken;
        }
        public string SessionToken { get; }
        public string Name { get; }
        public IDictionary<string, object> Parameters { get => _parameters ?? (_parameters = new Dictionary<string, object>()); set => _parameters = value; }
    }
}