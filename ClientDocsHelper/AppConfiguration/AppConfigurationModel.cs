using System.Text.Json.Serialization;

namespace ClientDocsHelper.AppConfiguration
{
    internal class AppConfigurationModel
    {
        public string? TemplateFolderPath { get; set; }
        public string? ClientsRootPath { get; set; }

        [JsonIgnore]
        public bool HasTemplateFolderPath => !string.IsNullOrWhiteSpace(TemplateFolderPath);
        [JsonIgnore]
        public bool HasClientsRootPath => !string.IsNullOrWhiteSpace(ClientsRootPath);

        [JsonIgnore]
        public bool IsValid => HasTemplateFolderPath && HasClientsRootPath;
    }
}
