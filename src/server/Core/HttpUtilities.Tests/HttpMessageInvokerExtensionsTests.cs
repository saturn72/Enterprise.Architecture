using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators;
using HttpUtilites;
using Moq;
using Shouldly;
using Xunit;

namespace HttpUtilities.Tests
{
    public class HttpMessageInvokerExtensionsTests
    {
        [Fact]
        public async Task HttpClientExtensions_Throws_OnEmtyFunc()
        {
            var hmh = new Mock<HttpMessageHandler>();
            var hmi = new Mock<HttpMessageInvoker>(hmh.Object);

            try
            {
                var t = HttpMessageInvokerExtensions.Retry(hmi.Object, null);
            }
            catch (Exception e)
            {
                e.ShouldBeOfType<ArgumentException>();
            }
        }

        [Fact]
        public async Task HttpClientExtensions_Fails()
        {
            var maxRetries = 3;
            var totalRetries = 0;

            var hmh = new Mock<HttpMessageHandler>();
            var hmi = new Mock<HttpMessageInvoker>(hmh.Object);

            hmi.Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    totalRetries++;
                    throw new HttpRequestException();
                });

            var req = new HttpRequestMessage(HttpMethod.Get, "https://www.gmail.com");
            Func<HttpMessageInvoker, Task<HttpResponseMessage>> request = h=>h.SendAsync(req, CancellationToken.None);

            try
            {
                var res = await HttpMessageInvokerExtensions.Retry(hmi.Object, request, maxRetries);
            }
            catch (Exception e)
            {
                e.ShouldBeOfType<HttpRequestException>();
            }

            totalRetries.ShouldBe(maxRetries+1);
        }

        [Fact]
        public async Task HttpClientExtensions_Succes_AfterSingleAttempt()
        {
            var maxRetries = 3;
            var totalRetries = 0;

            var hmh = new Mock<HttpMessageHandler>();
            var hmi = new Mock<HttpMessageInvoker>(hmh.Object);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            hmi.Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    totalRetries++;
                    if (totalRetries == 1)
                        throw new HttpRequestException();
                }).ReturnsAsync(response);

            var req = new HttpRequestMessage(HttpMethod.Get, "https://www.gmail.com");
            Func<HttpMessageInvoker, Task<HttpResponseMessage>> request = h => h.SendAsync(req, CancellationToken.None);

                var res = await HttpMessageInvokerExtensions.Retry(hmi.Object, request, maxRetries);
            res.StatusCode.ShouldBe(HttpStatusCode.OK);
            totalRetries.ShouldBe(2);
        }

        [Fact]
        public async Task HttpClientExtensions_Success_OnFirstAttempt()
        {
            var maxRetries = 3;
            var totalRetries = 0;

            var hmh = new Mock<HttpMessageHandler>();
            var hmi = new Mock<HttpMessageInvoker>(hmh.Object);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            hmi.Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Callback(()=> totalRetries++)
                .ReturnsAsync(response);

            var req = new HttpRequestMessage(HttpMethod.Get, "https://www.gmail.com");
            Func<HttpMessageInvoker, Task<HttpResponseMessage>> request = h => h.SendAsync(req, CancellationToken.None);

            var res = await HttpMessageInvokerExtensions.Retry(hmi.Object, request, maxRetries);
            res.StatusCode.ShouldBe(HttpStatusCode.OK);
            totalRetries.ShouldBe(1);
        }
    }
}
