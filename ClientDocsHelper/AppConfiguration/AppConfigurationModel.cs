namespace ClientDocsHelper.AppConfiguration
{
    internal class AppConfigurationModel
    {
        public string? TemplateFolderPath { get; set; }
        public string? ClientsRootPath { get; set; }

        public bool HasTemplateFolderPath => !string.IsNullOrWhiteSpace(TemplateFolderPath);
        public bool HasClientsRootPath => !string.IsNullOrWhiteSpace(ClientsRootPath);

        public bool IsValid => HasTemplateFolderPath && HasClientsRootPath;
    }
}
