namespace Programmer.Common.Services
{
    public class ServiceResponse<TServiceDataModel>
    {
        #region ctor

        public ServiceResponse()
        {
            Result = ServiceResponseResult.NotSet;
        }

        #endregion

        #region Properties

        public ServiceResponseResult Result { get; set; }
        public string Message { get; set; }
        public TServiceDataModel Data { get; set; }

        #endregion
    }
}