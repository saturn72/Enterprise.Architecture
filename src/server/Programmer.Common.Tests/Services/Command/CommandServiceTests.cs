using System.Collections;
using System.Collections.Generic;
using Programmer.Common.Services.Command;
using System.Threading.Tasks;
using EventBus;
using Moq;
using Programmer.Common.Services;
using Programmer.Common.Services.Session;
using Shouldly;
using Xunit;

namespace Programmer.Common.Tests.Services.Command
{
    public class CommandServiceTests
    {
        [Fact]
        public async Task CommandService_SendCommand_SessionNotFound()
        {
            var sessionManager = new Mock<ISessionManager>();
            sessionManager.Setup(sm => sm.GetSessionByToken(It.IsAny<string>())).ReturnsAsync(null as SessionData);

            var cmdReq = new CommandRequest("some-command", "some-session-id");

            var cmdSrv = new CommandService(sessionManager.Object, null, null);
            var res = await cmdSrv.SendCommand(cmdReq);
            res.Result.ShouldBe(ServiceResponseResult.NotFound);
        }

        [Fact]
        public async Task CommandService_SendCommand_PumpNotConnected()
        {
            var sd = new SessionData("some-token", "some-client-id", "some-resource-id");

            var sessionManager = new Mock<ISessionManager>();
            sessionManager.Setup(sm => sm.GetSessionByToken(It.IsAny<string>())).ReturnsAsync(sd);
            sessionManager.Setup(sm => sm.IsResourceConnected(It.IsAny<string>())).ReturnsAsync(false);

            var cmdReq = new CommandRequest("some-command", "some-session-id");

            var cmdSrv = new CommandService(sessionManager.Object, null, null);
            var res = await cmdSrv.SendCommand(cmdReq);
            res.Result.ShouldBe(ServiceResponseResult.NotAcceptable);
        }

        [Fact]
        public async Task CommandService_SendCommand_BadCommand()
        {
            var sd = new SessionData("some-token", "some-client-id", "some-resource-id");

            var sessionManager = new Mock<ISessionManager>();
            sessionManager.Setup(sm => sm.GetSessionByToken(It.IsAny<string>())).ReturnsAsync(sd);
            sessionManager.Setup(sm => sm.IsResourceConnected(It.IsAny<string>())).ReturnsAsync(true);
            string msg = null;
            var cmdVer = new Mock<ICommandVerifier>();
            cmdVer.Setup(c => c.IsValidCommand(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), ref msg))
                .Returns(false);
            var cmdReq = new CommandRequest("some-command", "some-session-id");

            var cmdSrv = new CommandService(sessionManager.Object, cmdVer.Object, null);
            var res = await cmdSrv.SendCommand(cmdReq);
            res.Result.ShouldBe(ServiceResponseResult.BadOrMissingData);
        }

        [Fact]
        public async Task CommandService_SendCommand_PublishCommand()
        {
            var sd = new SessionData("some-token", "some-client-id", "some-resource-id");

            var sessionManager = new Mock<ISessionManager>();
            sessionManager.Setup(sm => sm.GetSessionByToken(It.IsAny<string>())).ReturnsAsync(sd);
            sessionManager.Setup(sm => sm.IsResourceConnected(It.IsAny<string>())).ReturnsAsync(true);
            var cmdVer = new Mock<ICommandVerifier>();
            cmdVer.Setup(c => c.IsValidCommand(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), ref It.Ref<string>.IsAny)).Returns(true);
            var cmdReq = new CommandRequest("some-command", "some-session-id");

            var eventBus = new Mock<IEventBus>();
            var cmdSrv = new CommandService(sessionManager.Object, cmdVer.Object, eventBus.Object);
            var res = await cmdSrv.SendCommand(cmdReq);
            res.Result.ShouldBe(ServiceResponseResult.Created);

            eventBus.Verify(eb => eb.Publish(It.Is<CrudIntegrationEvent<CommandRequest>>(e=>e.CrudAction == CrudAction.Created)), Times.Once);
        }
    }
}