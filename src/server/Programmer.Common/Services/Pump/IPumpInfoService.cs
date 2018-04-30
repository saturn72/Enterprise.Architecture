using System.Threading.Tasks;
using Programmer.Common.Domain.Pump;

namespace Programmer.Common.Services.Pump
{
    public interface IPumpInfoService
    {
        Task<ServiceResponse<PumpInfoModel>> GetPumpInfoById(string pumpId);
    }
}