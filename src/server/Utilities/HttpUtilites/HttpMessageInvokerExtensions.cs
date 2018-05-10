using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace HttpUtilites
{
    public static class HttpMessageInvokerExtensions
    {
        public static async Task<HttpResponseMessage> Retry(this HttpMessageInvoker httpMessageInvoker,
            Func<HttpMessageInvoker, Task<HttpResponseMessage>> request, int maxRetries = 20)
        {
            if (request == null)
                throw new ArgumentException(nameof(request));

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(maxRetries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            var response = await retryPolicy.ExecuteAsync(() => request(httpMessageInvoker));
            return response;
        }
    }
}