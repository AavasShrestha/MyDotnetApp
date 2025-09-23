
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sample.Service.Utilities
{
    public class SwaggerEndpointFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // You can modify the swaggerDoc to filter specific endpoints dynamically if needed
        }
    }
}

