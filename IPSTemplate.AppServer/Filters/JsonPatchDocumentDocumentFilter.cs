using Microsoft.AspNetCore.JsonPatch;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IPSTemplate.AppServer.Filters
{
    public class JsonPatchDocumentDocumentFilter : IDocumentFilter
    {
        private const string JsonPatchDocumentSchemaName = "JsonPatchDocument";
        private const string OperationSchemaName = "Operation";

        public void Apply(OpenApiDocument swaggerDocument, DocumentFilterContext context)
        {
            RemoveIrrelevantSchemas(swaggerDocument);
            AddJsonPatchDocumentSchemas(swaggerDocument);
            UpdatePatchOperations(swaggerDocument);
        }

        private static void RemoveIrrelevantSchemas(OpenApiDocument swaggerDocument)
        {
            var schemas = swaggerDocument.Components.Schemas.ToList();

            foreach (var schema in schemas)
            {
                bool isIrrelevantSchema = schema.Key == "OperationType"
                    || schema.Key.Contains("ContractResolver")
                    || schema.Key.Contains("JsonPatchDocument")
                    || schema.Key.EndsWith("Operation");

                if (isIrrelevantSchema)
                    swaggerDocument.Components.Schemas.Remove(schema.Key);
            }
        }

        private static void AddJsonPatchDocumentSchemas(OpenApiDocument swaggerDocument)
        {
            swaggerDocument.Components.Schemas.Add(nameof(Microsoft.AspNetCore.JsonPatch.Operations.Operation), new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    { "op", new OpenApiSchema{ Type = "string" } },
                    { "value", new OpenApiSchema{ Type = "object", Nullable = true } },
                    { "from", new OpenApiSchema{ Type = "string" } },
                    { "path", new OpenApiSchema{ Type = "string" } }
                }
            });

            swaggerDocument.Components.Schemas.Add(nameof(JsonPatchDocument), new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema
                {
                    Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "Operation" }
                },
                Description = "Array of operations to perform",
                Example = SchemaExample,
                ExternalDocs = new OpenApiExternalDocs()
                {
                    Description = "JSON Patch Documentation",
                    Url = new Uri("http://jsonpatch.com/"),
                }
            });
        }

        private static void UpdatePatchOperations(OpenApiDocument swaggerDocument)
        {
            var patchOperations = swaggerDocument.Paths.SelectMany(p => p.Value.Operations)
                .Where(p => p.Key == OperationType.Patch);

            foreach (var operation in patchOperations)
            {
                foreach (var item in operation.Value.RequestBody.Content.Where(c => c.Key != "application/json-patch+json"))
                    operation.Value.RequestBody.Content.Remove(item.Key);

                var response = operation.Value.RequestBody.Content.Single(c => c.Key == "application/json-patch+json");

                response.Value.Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = JsonPatchDocumentSchemaName }
                };
            }
        }

        private static readonly OpenApiArray SchemaExample = new()
        {
            new OpenApiObject()
            {
                ["op"] = new OpenApiString("replace"),
                ["path"] = new OpenApiString("/property"),
                ["value"] = new OpenApiString("New Value"),
            },
            new OpenApiObject()
            {
                ["op"] = new OpenApiString("add"),
                ["path"] = new OpenApiString("/property"),
                ["value"] = new OpenApiString("New Value"),
            },
            new OpenApiObject()
            {
                ["op"] = new OpenApiString("remove"),
                ["path"] = new OpenApiString("/property"),
            },
            new OpenApiObject()
            {
                ["op"] = new OpenApiString("copy"),
                ["from"] = new OpenApiString("/fromProperty"),
                ["path"] = new OpenApiString("/toProperty"),
            },
            new OpenApiObject()
            {
                ["op"] = new OpenApiString("move"),
                ["from"] = new OpenApiString("/fromProperty"),
                ["path"] = new OpenApiString("/toProperty"),
            },
            new OpenApiObject()
            {
                ["op"] = new OpenApiString("test"),
                ["path"] = new OpenApiString("/property"),
                ["value"] = new OpenApiString("Has Value"),
            },
            new OpenApiObject()
            {
                ["op"] = new OpenApiString("replace"),
                ["path"] = new OpenApiString("/arrayProperty/0"),
                ["value"] = new OpenApiString("Replace First Array Item"),
            },
            new OpenApiObject()
            {
                ["op"] = new OpenApiString("replace"),
                ["path"] = new OpenApiString("/arrayProperty/-"),
                ["value"] = new OpenApiString("Replace Last Array Item"),
            },
        };
    }
}
