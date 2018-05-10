namespace Programmer.Common.Services.Command
{
    public class CommandResponse
    {
        public CommandResponse(CommandRequest request)
        {
            Request = request;
        }

        public CommandRequest Request { get; }
    }
}