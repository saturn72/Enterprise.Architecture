using System.Threading.Tasks;

namespace Programmer.Common.Services.Command
{
    public interface ICommandManager
    {
        Task<ServiceResponse<CommandResponse>> SendCommand(CommandRequest request);
    }
}