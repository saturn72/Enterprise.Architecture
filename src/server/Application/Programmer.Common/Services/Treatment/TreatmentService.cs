﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services.Command;

namespace Programmer.Common.Services.Treatment
{
    public class TreatmentService : ITreatmentService
    {
        #region CTOR

        public TreatmentService(ITreatmentRepository treatmentRepository, ICommandManager commandManager)
        {
            _treatmentRepository = treatmentRepository;
            _commandManager = commandManager;
        }

        #endregion

        #region Get All

        public async Task<ServiceResponse<IEnumerable<TreatmentModel>>> GetAll()
        {
            return new ServiceResponse<IEnumerable<TreatmentModel>>
            {
                Data = await Task.Run(() => _treatmentRepository.GetAll()),
                Result = ServiceResponseResult.Read
            };
        }

        #endregion

        #region GetById

        public async Task<ServiceResponse<TreatmentModel>> GetById(long id)
        {
            var dbData = await Task.Run(() => _treatmentRepository.GetById(id));
            return new ServiceResponse<TreatmentModel>
            {
                Result = ServiceResponseResult.Read,
                Data = dbData
            };
        }

        #endregion

        #region Fields

        private readonly ITreatmentRepository _treatmentRepository;
        private readonly ICommandManager _commandManager;

        #endregion

        #region Create

        public async Task<ServiceResponse<TreatmentModel>> CreateTreament(TreatmentModel treatmentModel)
        {
            var cmdRequest = ToCommandRequest("treatment", treatmentModel.SessionId, treatmentModel);
            var cmdRes = await _commandManager.SendCommand(cmdRequest);
            var srvRes = new ServiceResponse<TreatmentModel>
            {
                Result = cmdRes.Result,
                Data = treatmentModel,
                Message = cmdRes.Message
            };

            //TODO: add audity on save
            //TODO: make this a transactional
            if (cmdRes.IsSuccessful())
                _treatmentRepository.CreateTreatment(treatmentModel);
            return srvRes;
        }

        #region CreateTreatment Utilities

        private CommandRequest ToCommandRequest<T>(string cmdName, string sessionId, T treatmentModel)
        {
            var cmdReq = new CommandRequest(cmdName, sessionId);
            var pInfos = typeof(T).GetProperties();

            foreach (var pi in pInfos)
                cmdReq.Parameters[pi.Name] = pi.GetValue(treatmentModel);

            return cmdReq;
        }

        #endregion

        #endregion
    }
}