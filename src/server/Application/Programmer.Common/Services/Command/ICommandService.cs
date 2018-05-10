using System.Threading.Tasks;

namespace Programmer.Common.Services.Command
{
    public interface ICommandService
    {
        Task<ServiceResponse<CommandResponse>> SendCommand(CommandRequest request);
    }
}