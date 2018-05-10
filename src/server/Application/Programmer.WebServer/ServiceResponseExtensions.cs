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
            ServiceResponseToActionResultDictionary =
                new Dictionary<ServiceResponseResult, Func<object, string, IActionResult>>
                {
                    {
                        ServiceResponseResult.NotSet,
                        (data, errorMessage) =>
                            BuildObjectResult(data, errorMessage, HttpStatusCode.InternalServerError)
                    },
                    {
                        ServiceResponseResult.Success, (data, message) => new OkObjectResult(new
                        {
                            Data = data,
                            Message = message
                        })
                    },
                    {
                        ServiceResponseResult.Read, (data, message) => new OkObjectResult(new
                        {
                            Data = data,
                            Message = message
                        })
                    },
                    {
                        ServiceResponseResult.Accepted,
                        (data, message) => BuildObjectResult(data, message, HttpStatusCode.Accepted)
                    },
                    {
                        ServiceResponseResult.NotAcceptable,
                        (data, errorMessage) => BuildObjectResult(data, errorMessage, HttpStatusCode.NotAcceptable)
                    },
                    {ServiceResponseResult.Created, (data, errorMessage) => new CreatedResult("", data)},
                    {ServiceResponseResult.Updated, (data, errorMessage) => new AcceptedResult("", data)},
                    {
                        ServiceResponseResult.BadOrMissingData, (data, errorMessage) => new BadRequestObjectResult(new
                        {
                            Message = errorMessage.HasValue() ? errorMessage : "Failed due to bad or missing data.",
                            Data = data
                        })
                    },

                    {
                        ServiceResponseResult.NotFound, (data, errorMessage) => new NotFoundObjectResult(new
                        {
                            Message = errorMessage.HasValue() ? errorMessage : "The Object was not found",
                            Data = data
                        })
                    }
                };

        private static IActionResult BuildObjectResult(object data, string message, HttpStatusCode statusCode)
        {
            return
                new ObjectResult(new
                    {
                        Message = message,
                        Data = data
                    })
                    {StatusCode = (int) statusCode};
        }

        /// <summary>
        ///     Converts ServiceResponse to Generic IActionResult instance
        /// </summary>
        /// <param name="serviceResponse"></param>
        /// <returns></returns>
        public static IActionResult ToActionResult<T>(this ServiceResponse<T> serviceResponse)
        {
            return ServiceResponseToActionResultDictionary[serviceResponse.Result](serviceResponse.Data,
                serviceResponse.Message);
        }
    }
}