using Integration_Hub.Config;
using Integration_Hub.Data;
using Integration_Hub.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Integration_Hub.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public IntegrationService(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task ExecuteIntegration(Guid integrationId)
        {
            var integration = await _context.Integrations
                .Include(i => i.Source)
                .Include(i => i.Destination)
                .Include(i => i.Mapping)
                .FirstOrDefaultAsync(i => i.Id == integrationId);

            if (integration == null) throw new Exception("Integration not found");

            // Fetch data from source
            var sourceData = await FetchSourceData(integration.Source!);

            // Apply mappings and transformations
            var transformedData = ApplyMappings(sourceData, integration.Mapping);

            // Send data to destination
            await SendToDestination(transformedData, integration.Destination!);

            // Log the execution
            _context.Logs.Add(new Log
            {
                Id = Guid.NewGuid(),
                IntegrationId = integration.Id,
                Message = "Integration executed successfully",
                LogLevel = LogLevels.Info,
                Timestamp = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        private async Task<JObject> FetchSourceData(Source source)
        {
            if (source.Type == SourceType.API)
            {
                var config = JsonConvert.DeserializeObject<ApiSourceConfig>(source.Configuration);

                // Assume basic authentication for API
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes($"{config.Username}:{config.Password}")));

                var response = await _httpClient.GetAsync(config.ApiUrl);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to fetch data from API source");

                var content = await response.Content.ReadAsStringAsync();
                return JObject.Parse(content);
            }

            throw new NotImplementedException("Source type not supported");
        }

        private JObject ApplyMappings(JObject sourceData, Mapping mapping)
        {
            var fieldMappings = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapping.FieldMappings);
            var transformedData = new JObject();

            foreach (var mappingEntry in fieldMappings)
            {
                var sourceField = mappingEntry.Key;
                var destinationField = mappingEntry.Value;

                if (sourceData[sourceField] != null)
                {
                    transformedData[destinationField] = sourceData[sourceField];
                }
            }

            return transformedData;
        }

        private async Task SendToDestination(JObject transformedData, Destination destination)
        {
            if (destination.Type == DestinationType.Database)
            {
                var config = JsonConvert.DeserializeObject<DatabaseDestinationConfig>(destination.Configuration);

                using (var connection = new SqlConnection(config.ConnectionString))
                {
                    await connection.OpenAsync();

                    var insertCommand = BuildInsertCommand(transformedData, config.TableName);
                    using (var command = new SqlCommand(insertCommand, connection))
                    {
                        foreach (var field in transformedData)
                        {
                            command.Parameters.AddWithValue($"@{field.Key}", field.Value.ToString());
                        }

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            else
            {
                throw new NotImplementedException("Destination type not supported");
            }
        }

        private string BuildInsertCommand(JObject transformedData, string tableName)
        {
            var columns = string.Join(", ", transformedData.Properties().Select(p => p.Name));
            var values = string.Join(", ", transformedData.Properties().Select(p => $"@{p.Name}"));

            return $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
        }
    }
}
