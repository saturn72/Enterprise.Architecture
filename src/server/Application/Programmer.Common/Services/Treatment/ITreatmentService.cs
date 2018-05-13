using System.Collections.Generic;
using System.Threading.Tasks;
using Programmer.Common.Domain.Treatment;

namespace Programmer.Common.Services.Treatment
{
    public interface ITreatmentService
    {
        Task<ServiceResponse<IEnumerable<TreatmentModel>>> GetAll();
        Task<ServiceResponse<TreatmentModel>> CreateTreament(TreatmentModel treatmentModel);
    }
}
