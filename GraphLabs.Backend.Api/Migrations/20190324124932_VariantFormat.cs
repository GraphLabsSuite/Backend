using System.Globalization;
using GraphLabs.Backend.Domain.VariantData;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GraphLabs.Backend.Api.Migrations
{
    public partial class VariantFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sampleData = new VariantData<Graph>
            {
                Type = VariantDataType.Graph,
                Value = new Graph
                {
                    Vertices = new[] {"1", "2", "3", "4", "5"},
                    Edges = new[]
                    {
                        new Edge {Source = "1", Target = "2"},
                        new Edge {Source = "2", Target = "3"},
                        new Edge {Source = "3", Target = "4"},
                        new Edge {Source = "4", Target = "5"},
                        new Edge {Source = "5", Target = "1"}
                    }
                }
            };

            var sampleDataStr = JsonConvert.SerializeObject(new [] { sampleData }, new JsonSerializerSettings()
            {
                Converters =
                {
                    new StringEnumConverter
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new LowerCamelCaseContractResolver()
            });

            migrationBuilder.Sql($@"update TaskVariants set VariantData = '{sampleDataStr}'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update TaskVariants set VariantData = '[{ 
                ""vertices"": [ ""1"", ""2"", ""3"", ""4"", ""5"" ], 
                ""edges"": [ { ""source"": ""1"", ""target"": ""2"" }, { ""source"": ""2"", ""target"": ""3"" }, { ""source"": ""3"", ""target"": ""4"" }, { ""source"": ""4"", ""target"": ""5"" }, { ""source"": ""5"", ""target"": ""1"" } ] 
            }]'");
        }
    }
}
