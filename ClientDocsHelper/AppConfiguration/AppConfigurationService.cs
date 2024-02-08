using ClientDocsHelper.ConsoleUtilities;

namespace ClientDocsHelper.AppConfiguration
{
    internal class AppConfigurationService
    {
        private readonly AppConfigurationReader appConfigurationReader;
        private readonly AppConfigurationWriter appConfigurationWriter;

        private AppConfigurationModel? appConfig;

        public AppConfigurationService(AppConfigurationReader appConfigurationReader, AppConfigurationWriter appConfigurationWriter)
        {
            this.appConfigurationReader = appConfigurationReader;
            this.appConfigurationWriter = appConfigurationWriter;
        }

        public async Task<AppConfigurationModel?> GetAppConfiguration()
        {
            appConfig = await appConfigurationReader.ReadConfiguration();
            return appConfig;
        }

        public async Task<AppConfigurationModel> UpdateAppConfigurationFromUser()
        {
            appConfig = new AppConfigurationModel
            {
                TemplateFolderPath = GetTemplateFolderPathFromUser(),
                ClientsRootPath = GetClientsRootFolderPathFromUser()
            };
            await appConfigurationWriter.SaveAppConfiguration(appConfig);
            return appConfig;
        }

        private string? GetClientsRootFolderPathFromUser()
        {
            string? clientsRootPath;
            if (appConfig?.HasClientsRootPath ?? false)
            {
                var prompt = $"Zadejte nové umístění kmenového adresáře pro klientské složky. Enterem ponecháte stávající ({appConfig.ClientsRootPath})";
                clientsRootPath = PathEntryHelpers.ReadValidPath(prompt, false);
                clientsRootPath = string.IsNullOrWhiteSpace(clientsRootPath) ? appConfig.ClientsRootPath : clientsRootPath;
            }
            else
            {
                var prompt = $"Zadejte umístění kmenového adresáře pro klientské složky.";
                clientsRootPath = PathEntryHelpers.ReadValidPath(prompt, true);
            }

            return clientsRootPath;
        }

        private string? GetTemplateFolderPathFromUser()
        {
            string? templatePath;
            if (appConfig?.HasTemplateFolderPath ?? false)
            {
                var prompt = $"Zadejte nové umístění složky se vzorovou strukturou. Enterem ponecháte stávající ({appConfig.TemplateFolderPath})";
                templatePath = PathEntryHelpers.ReadValidPath(prompt, false);
                templatePath = string.IsNullOrWhiteSpace(templatePath) ? appConfig.TemplateFolderPath : templatePath;
            }
            else
            {
                var prompt = $"Zadejte nové umístění složky se vzorovou strukturou.";
                templatePath = PathEntryHelpers.ReadValidPath(prompt, true);
            }

            return templatePath;
        }
    }
}
