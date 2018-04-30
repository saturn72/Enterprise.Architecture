using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Programmer.Common.Services;

namespace Programmer.WebServer
{
    /// <summary>
    ///     ServiceResponse object's extensions
    /// </summary>
    public static class ServiceResponseExtensions
    {
        private static readonly IDictionary<ServiceResponseResult, Func<object, string, IActionResult>>
            _serviceResponseToActionResultDictionary =
                new Dictionary<ServiceResponseResult, Func<object, string, IActionResult>>
                {
                    {
                        ServiceResponseResult.NotSet,
                        (obj, errMsg) => BuildObjectResult(obj, "Service Response value was not set.",
                            HttpStatusCode.InternalServerError)
                    },
                    {
                        ServiceResponseResult.Success,
                        (obj, errMsg) => BuildObjectResult(obj, "", HttpStatusCode.PartialContent)
                    },
                    {
                        ServiceResponseResult.Created,
                        (obj, errMsg) => new CreatedResult("", obj)
                    },
                    {
                        ServiceResponseResult.Updated,
                        (obj, errMsg) => new AcceptedResult("", obj)
                    },
                    {
                        ServiceResponseResult.BadOrMissingData,
                        (obj, errMsg) => new BadRequestObjectResult(new
                        {
                            Message = "Failed due to bad or missing data.",
                            Data = obj
                        })
                    },
                    {
                        ServiceResponseResult.NotFound,
                        (obj, errMsg) => new NotFoundObjectResult(new
                        {
                            Message = "The Object was not found",
                            Data = obj
                        })
                    },
                    {
                        ServiceResponseResult.NotAcceptable,
                        (obj, errMsg) => BuildObjectResult(obj, errMsg, HttpStatusCode.NotAcceptable)
                    }
                };

        private static IActionResult BuildObjectResult(object data, string errorMessage, HttpStatusCode httpStatusCode)
        {
            return new ObjectResult(new
            {
                message = errorMessage,
                data
            })
            {
                StatusCode = (int) httpStatusCode
            };
        }

        /// <summary>
        ///     Converts ServiceResponse to Generic IActionResult instance
        /// </summary>
        /// <param name="serviceResponse"></param>
        /// <returns></returns>
        public static IActionResult ToActionResult<T>(this ServiceResponse<T> serviceResponse)
        {
            return serviceResponse.IsSuccessful()
                ? new OkObjectResult(serviceResponse.Data)
                : _serviceResponseToActionResultDictionary[serviceResponse.Result](serviceResponse.Data, serviceResponse.ErrorMessage);
        }
    }
}