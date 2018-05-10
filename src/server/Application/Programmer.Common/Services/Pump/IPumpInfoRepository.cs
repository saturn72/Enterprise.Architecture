using System.Threading.Tasks;
using Programmer.Common.Domain.Pump;

namespace Programmer.Common.Services.Pump
{
    public interface IPumpInfoRepository
    {
        Task<PumpInfoModel> GetById(string id);
    }
}