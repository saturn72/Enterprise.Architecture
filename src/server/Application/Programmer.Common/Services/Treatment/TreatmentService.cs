using System.Collections.Generic;
using System.Threading.Tasks;
using Programmer.Common.Domain.Treatment;

namespace Programmer.Common.Services.Treatment
{
    public class TreatmentService : ITreatmentService
    {
        private readonly ITreatmentRepository _treatmentRepository;

        public TreatmentService(ITreatmentRepository treatmentRepository)
        {
            _treatmentRepository = treatmentRepository;
        }

        public async Task<ServiceResponse<IEnumerable<TreatmentModel>>> GetAll()
        {
            return new ServiceResponse<IEnumerable<TreatmentModel>>
            {
                Data = await Task.Run(() => _treatmentRepository.GetAll()),
                Result = ServiceResponseResult.Read
            };
        }
    }
}