using System.Collections.Generic;

namespace Programmer.Common.Services.Command
{
    public interface ICommandVerifier
    {
        bool IsValidCommand(string commandName, IDictionary<string, object> parameters, ref string message);
    }
}