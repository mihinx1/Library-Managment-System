using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IPSTemplate.AppServer.Filters
{
    public class MultipleOperationsWithSameVerbFilter : IOperationFilter
    {
        /// <summary>
        /// Set operationId as [template][controller]By[parameter] -> GetDocumentByid
        /// This prevent same operation id trught all schema to be unique
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                operation.OperationId += context.ApiDescription.ActionDescriptor.RouteValues["controller"].Replace("_", "") + "By";
                foreach (var parm in operation.Parameters)
                {
                    operation.OperationId += string.Format("{0}", parm.Name);
                }
            }
        }
    }
}
