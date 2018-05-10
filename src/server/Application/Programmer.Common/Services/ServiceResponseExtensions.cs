namespace Programmer.Common.Services
{
    public static class ServiceResponseExtensions
    {
        public static bool IsSuccessful<T>(this ServiceResponse<T> serviceResponse)
        {
            return serviceResponse.Result == ServiceResponseResult.Success
                   || serviceResponse.Result == ServiceResponseResult.Accepted;
        }        
    }
}