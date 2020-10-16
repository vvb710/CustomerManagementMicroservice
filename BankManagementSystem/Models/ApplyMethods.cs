using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace BankManagementSystem.Models
{
    public class ApplyMethods
    {

        public class RemoveVersionFromParameter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (operation.Parameters.Count > 0)
                {
                    var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                    operation.Parameters.Remove(versionParameter);
                }
            }
        }

        public class ReplaceVersionWithExactValueInPath : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var paths = swaggerDoc.Paths;
                swaggerDoc.Paths = new OpenApiPaths();

                foreach (var path in paths)
                {
                    var key = path.Key.Replace("v{version}", swaggerDoc.Info.Version);
                    var value = path.Value;
                    swaggerDoc.Paths.Add(key, value);
                }
            }
        }
    }
}
