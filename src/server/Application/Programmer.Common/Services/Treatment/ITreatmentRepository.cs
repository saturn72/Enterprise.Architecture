using System.Collections.Generic;
using Programmer.Common.Domain.Treatment;

namespace Programmer.Common.Services.Treatment
{
    public interface ITreatmentRepository
    {
        IEnumerable<TreatmentModel> GetAll();
        void CreateTreatment(TreatmentModel treatmentModel);
        TreatmentModel GetById(long id);
    }
}
