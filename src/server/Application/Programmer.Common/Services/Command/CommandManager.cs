using System.Threading.Tasks;
using EventBus;
using Programmer.Common.Services.Session;

namespace Programmer.Common.Services.Command
{
    public class CommandManager : ICommandManager
    {
        #region fields
        private readonly ISessionManager _sessionManager;
        private readonly ICommandVerifier _commandVerifier;
        private readonly IEventBus _eventBus;

        #endregion

        #region CTOR
        public CommandManager(ISessionManager sessionManager, ICommandVerifier commandVerifier,
            IEventBus eventBus)
        {
            _sessionManager = sessionManager;
            _commandVerifier = commandVerifier;
            _eventBus = eventBus;
        }
        #endregion

        public async Task<ServiceResponse<CommandResponse>> SendCommand(CommandRequest request)
        {
            var commandResponse = new CommandResponse(request);
            var srvRes = new ServiceResponse<CommandResponse>
            {
                Data = commandResponse,
            };

            if (!await ValidateSessionData(request.SessionToken, srvRes)
                || !ValidateCommandData(request, srvRes))
                return srvRes;

            _eventBus.PublishEntityCreatedEvent(request);

            srvRes.Result = ServiceResponseResult.Accepted;
            return srvRes;
        }

        private bool ValidateCommandData(CommandRequest cmdRequest, ServiceResponse<CommandResponse> srvRes)
        {
            var message = "";
            var isValid = _commandVerifier.IsValidCommand(cmdRequest.Name, cmdRequest.Parameters, ref message);
            string.Concat(srvRes.Message, message);

            if (!isValid)
                srvRes.Result = ServiceResponseResult.BadOrMissingData;

            return isValid;
        }

        private async Task<bool> ValidateSessionData(string sessionToken, ServiceResponse<CommandResponse> serviceResponse)
        {
            var sessionData = await _sessionManager.GetSessionByToken(sessionToken);
            if (sessionData == null)
            {
                serviceResponse.Result = ServiceResponseResult.NotFound;
                serviceResponse.Message = "Failed to find session with token =  " + sessionToken;
                return false;
            }

            if (!await _sessionManager.IsResourceConnected(sessionData.ResourceId))
            {
                serviceResponse.Result = ServiceResponseResult.NotAcceptable;
                serviceResponse.Message = string.Format("Lost connection with the pump (serial #{0})", sessionData.ResourceId);
                return false;
            }
            return true;
        }
    }
}