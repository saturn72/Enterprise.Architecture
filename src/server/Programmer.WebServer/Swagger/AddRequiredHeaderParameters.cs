using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Programmer.WebServer.Swagger
{
    public class AddRequiredHeaderParameters : IOperationFilter
    {
        private static readonly string[] RequiresSessionIdHeader = new[]{"command"};
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            if (operation.Tags.Any(t => RequiresSessionIdHeader.Contains(t.ToLower())))
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
    }
}