using Programmer.Common.Services;
using Shouldly;
using Xunit;

namespace Programmer.Common.Tests.Services
{
    public class ServiceResponseExtensionsTests
    {
        [Theory]
        [InlineData(null, ServiceResponseResult.NotFound)]
        [InlineData("some-error-message", ServiceResponseResult.NotFound)]
        [InlineData("some-error-message", ServiceResponseResult.Success)]
        public void ServiceResponseResult_IsSuccessful_False(string errorMessage, ServiceResponseResult result)
        {
            var srvRes = new ServiceResponse<string>()
            {
                ErrorMessage = errorMessage,
                Result = result
            };

            srvRes.IsSuccessful().ShouldBeFalse();
        }

        [Theory]
        [InlineData(ServiceResponseResult.BadOrMissingData)]
        [InlineData(ServiceResponseResult.NotFound)]
        [InlineData(ServiceResponseResult.NotSet)]
        [InlineData(ServiceResponseResult.Success)]
        public void ServiceResponseExtensions_IsSuccessful_ReturnsError(ServiceResponseResult serviceResponseResult)
        {
            var serRes = new ServiceResponse<string>
            {
                ErrorMessage = "some-error-message",
                Result = serviceResponseResult
            };

            serRes.IsSuccessful().ShouldBeFalse();
        }
        [Fact]
        public void ServiceResponseResult_IsSuccessful_Passes()
        {
            var srvRes = new ServiceResponse<string>
            {
                Result = ServiceResponseResult.Success
            };

            srvRes.IsSuccessful().ShouldBeTrue();
        }
    }
}
