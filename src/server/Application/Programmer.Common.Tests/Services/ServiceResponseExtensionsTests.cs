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
        public void ServiceResponseResult_IsSuccessful_False(string errorMessage, ServiceResponseResult result)
        {
            var srvRes = new ServiceResponse<string>()
            {
                Message = errorMessage,
                Result = result
            };

            srvRes.IsSuccessful().ShouldBeFalse();
        }

        [Theory]
        [InlineData(ServiceResponseResult.BadOrMissingData)]
        [InlineData(ServiceResponseResult.NotFound)]
        [InlineData(ServiceResponseResult.NotSet)]
        public void ServiceResponseExtensions_IsSuccessful_ReturnsError(ServiceResponseResult serviceResponseResult)
        {
            var serRes = new ServiceResponse<string>
            {
                Message = "some-error-message",
                Result = serviceResponseResult
            };

            serRes.IsSuccessful().ShouldBeFalse();
        }

        [Theory]
        [InlineData(ServiceResponseResult.Success)]
        [InlineData(ServiceResponseResult.Accepted)]
        public void ServiceResponseResult_IsSuccessful_Passes(ServiceResponseResult serviceResponseResult)
        {
            var srvRes = new ServiceResponse<string>
            {
                Result = serviceResponseResult
            };

            srvRes.IsSuccessful().ShouldBeTrue();
        }
    }
}
