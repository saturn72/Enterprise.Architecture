using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Programmer.Common.Services;
using Shouldly;
using Xunit;

namespace Programmer.WebServer.Tests
{
    public class ServiceResponseExtensionsTests
    {
        [Theory]
        [InlineData( ServiceResponseResult.Success, typeof(OkObjectResult))]
        [InlineData( ServiceResponseResult.Created, typeof(CreatedResult))]
        [InlineData( ServiceResponseResult.Updated, typeof(AcceptedResult))]
        [InlineData( ServiceResponseResult.BadOrMissingData, typeof(BadRequestObjectResult))]
        [InlineData( ServiceResponseResult.NotFound, typeof(NotFoundObjectResult))]
        [InlineData( ServiceResponseResult.Read, typeof(OkObjectResult))]
        public void ServiceResponseExtensions_ToActionResult_ReturnsErrorBy_AssertByMessageResponseType(ServiceResponseResult serviceResponseResult, Type expType)
        {
            var serRes = new ServiceResponse<string>
            {
                Result = serviceResponseResult
            };

            serRes.ToActionResult().ShouldBeOfType(expType);
        }

        [Theory]
        [InlineData("some-message", ServiceResponseResult.NotSet, HttpStatusCode.InternalServerError)]
        [InlineData("some-message", ServiceResponseResult.NotAcceptable, HttpStatusCode.NotAcceptable)]
        [InlineData("some-message", ServiceResponseResult.Accepted, HttpStatusCode.Accepted)]
        public void ServiceResponseExtensions_ToActionResult_ReturnsErrorBy_AssertByHttpStatusCode(string message, ServiceResponseResult serviceResponseResult, HttpStatusCode expStatusCode)
        {
            var serRes = new ServiceResponse<string>
            {
                Message = message,
                Result = serviceResponseResult
            };

            var response = serRes.ToActionResult().ShouldBeOfType<ObjectResult>();
            response.StatusCode.ShouldBe((int)expStatusCode);
        }
    }
}