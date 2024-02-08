namespace ClientDocsHelper.AppConfiguration
{
    internal class AppConfiguration
    {
        public string? TemplateFolderPath { get; set; }
        public string? ClientsRootPath { get; set; }

        public bool HasTemplateFolderPath => !string.IsNullOrWhiteSpace(TemplateFolderPath);
        public bool HasClientsRootPath => !string.IsNullOrWhiteSpace(ClientsRootPath);

        public bool IsValid => HasTemplateFolderPath && HasClientsRootPath;
    }
}
