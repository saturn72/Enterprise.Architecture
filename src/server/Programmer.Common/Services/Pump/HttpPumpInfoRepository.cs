using System;
using System.Net.Http;
using System.Threading.Tasks;
using Programmer.Common.Domain.Pump;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Programmer.Common.Services.Pump
{
    public class HttpPumpInfoRepository : IPumpInfoRepository
    {
        private readonly HttpClient _httpClient;

        public HttpPumpInfoRepository(Uri baseAddress)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "pumpinfo"),
            };
        }
        public async Task<PumpInfoModel> GetById(string id)
        {
            var response = await _httpClient.GetAsync(id);
            var dbModel = await response.Content.ReadAsStringAsync();
            return RemoveAllWhiteSpaceCharacters(dbModel) == "{}" ?
            null :
             JsonConvert.DeserializeObject<PumpInfoModel>(dbModel);
        }

        private string RemoveAllWhiteSpaceCharacters(string source)
        {
            return source.Replace('\n', ' ').Replace('\r', ' ').Replace(" ", string.Empty);
        }
    }
}