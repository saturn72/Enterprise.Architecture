using System;
using System.Collections.Generic;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services.Treatment;

namespace Programmer.Db.LiteDb.Repositories
{
    public class TreatmentRepository:ITreatmentRepository
    {
        public IEnumerable<TreatmentModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
