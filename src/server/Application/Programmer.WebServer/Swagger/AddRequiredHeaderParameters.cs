using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ServiceStack;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Programmer.WebServer.Swagger
{
    public class AddRequiredHeaderParameters : IOperationFilter
    {
        private static readonly IEnumerable<RequiredSessionHeaderData> RequiresSessionIdHeader = new[]
        {
            new RequiredSessionHeaderData( "treatment", HttpMethod.Post)
        };
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            if (RequiresSessionIdHeader.Any(rs =>operation.Tags.Contains(rs.ResourceName, StringComparer.InvariantCultureIgnoreCase) &&  operation.OperationId.EndsWith(rs.HttpMethod, StringComparison.InvariantCultureIgnoreCase)))
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "X-Session-Token",
                    In = "header",
                    Type = "string",
                    Required = true
                });
            }
        }

        private sealed class RequiredSessionHeaderData
        {
            public RequiredSessionHeaderData(string resourceName, HttpMethod httpMethod)
            {
                ResourceName = resourceName.ToLower();
                HttpMethod = httpMethod.ToString().ToLower();
            }

            internal string ResourceName { get; }
            public string HttpMethod { get; }
        }
    }
}