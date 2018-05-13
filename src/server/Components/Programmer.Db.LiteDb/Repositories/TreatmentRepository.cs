using System;
using System.Collections.Generic;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services.Treatment;

namespace Programmer.Db.LiteDb.Repositories
{
    public class TreatmentRepository : ITreatmentRepository
    {
        #region Fields

        private readonly LiteDbAdapter _dbAdapter;

        #endregion

        #region ctor

        public TreatmentRepository(LiteDbAdapter dbAdapter)
        {
            _dbAdapter = dbAdapter;
        }

        #endregion

        public IEnumerable<TreatmentModel> GetAll()
        {
            return _dbAdapter.GetAll<TreatmentModel>();
        }

        public void CreateTreatment(TreatmentModel treatmentModel)
        {
            _dbAdapter.Create(treatmentModel);
        }

        public TreatmentModel GetById(long id)
        {
            return _dbAdapter.GetById<TreatmentModel>(id);
        }
    }
}