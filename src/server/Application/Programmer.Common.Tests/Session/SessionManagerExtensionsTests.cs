using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventBus.Subscriptions;
using Moq;
using Programmer.Common.Services.Session;
using Shouldly;
using Xunit;

namespace Programmer.Common.Tests.Session
{
    public class SessionManagerExtensionsTests
    {
        [Theory]
        [MemberData(nameof(SessionManagerExtensions_IsSessionExists_Data))]
        public async Task SessionManagerExtensions_IsSessionExists(SessionData sessionData, bool expResult)
        {
            var sm = new Mock<ISessionManager>();
            sm.Setup(s => s.GetSessionByToken(It.IsAny<string>())).ReturnsAsync(sessionData);

            var res = await SessionManagerExtensions.IsSessionExists(sm.Object, "dadada");
            res.ShouldBe(expResult);
        }

        public static IEnumerable<object[]> SessionManagerExtensions_IsSessionExists_Data =>
            new[]{
                new object[] { null, false},
                new object[] { new SessionData("dd", "dd", "dd"), true},
            };
    }
}
