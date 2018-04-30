using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Programmer.Common.Services;

namespace Programmer.WebServer.Controllers
{
    /// <summary>
    /// ServiceResponse object's extensions
    /// </summary>
    public static class ServiceResponseExtensions
    {
        private static readonly IDictionary<ServiceResponseResult, Func<object, string, IActionResult>> _serviceResponseToActionResultDictionary =
        new Dictionary<ServiceResponseResult, Func<object, string, IActionResult>>
        {
            { ServiceResponseResult.NotSet, (data, errorMessage) => BuildObjectResult(data, errorMessage,HttpStatusCode.InternalServerError)},
            { ServiceResponseResult.Success, (data, errorMessage) => BuildObjectResult(data, errorMessage,HttpStatusCode.PartialContent)},
            { ServiceResponseResult.NotAcceptable, (data, errorMessage) => BuildObjectResult(data, errorMessage,HttpStatusCode.NotAcceptable)},
            { ServiceResponseResult.Created, (data, errorMessage) => new CreatedResult("", data)},
            { ServiceResponseResult.Updated, (data, errorMessage) => new AcceptedResult("", data)},
            { ServiceResponseResult.BadOrMissingData, (data, errorMessage) => new BadRequestObjectResult(new
                {
                   Message = errorMessage.HasValue()? errorMessage: "Failed due to bad or missing data.",
                   Data = data
                })
            },

           { ServiceResponseResult.NotFound, (data, errorMessage) => new NotFoundObjectResult(new
                {
                   Message = errorMessage.HasValue()? errorMessage: "The Object was not found",
                   Data = data
                })
            },
        };

        private static IActionResult BuildObjectResult(object data, string errorMessage, HttpStatusCode statusCode)
        {
            return
             new ObjectResult(new
             {
                 Message = errorMessage,
                 Data = data
             })
             { StatusCode = (int)statusCode };
        }

        /// <summary>
        /// Converts ServiceResponse to Generic IActionResult instance
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