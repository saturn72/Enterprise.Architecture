
namespace PnpServiceFake.Integration
{
    public class CommandResponse
    {
        public CommandRequest Request { get; }

        public CommandResponse(CommandRequest request)
        {
            Request = request;
        }
    }
}