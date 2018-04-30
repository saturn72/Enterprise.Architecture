using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Programmer.Test.Framework
{
    public class RestUtil
    {
        public static HttpRequestMessage BuildRequest(HttpMethod httpMethod, string uri, object content)
        {
            HttpContent requestContent = null;

            if (content != null)
            {
                var model = JObject.FromObject(content);
                requestContent = new StringContent(model.ToString(), Encoding.UTF8,
                    MediaType.ApplicationJson.Name);
            }

            var httpReqMsg = new HttpRequestMessage(httpMethod, uri);
            if (content != null)
                httpReqMsg.Content = requestContent;

            return httpReqMsg;
        }

        public static async Task<JObject> ExtractJObject(HttpResponseMessage httpResponse)
        {
            return ExtractJObject(await ContentToString(httpResponse));
        }

        public static JObject ExtractJObject(object obj)
        {
            return obj is string ? JObject.Parse(obj as string) : JObject.FromObject(obj);
        }

        public static async Task<JArray> ExtractJArray(HttpResponseMessage httpResponse)
        {
            var json = await ContentToString(httpResponse);
            return JArray.Parse(json);
        }

        private static async Task<string> ContentToString(HttpResponseMessage httpResponse)
        {
            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
