using System.Collections.Generic;
using Programmer.Common.Services.Command;

namespace Programmer.Common.Fakes.Command
{
    public class DummyCommandVerifier:ICommandVerifier
    {
        public bool IsValidCommand(string commandName, IDictionary<string, object> parameters, ref string message)
        {
            return true;
        }
    }
}
