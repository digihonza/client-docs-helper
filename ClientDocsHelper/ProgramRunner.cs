using ClientDocsHelper.AppConfiguration;

namespace ClientDocsHelper
{
    internal class ProgramRunner
    {
        private readonly AppConfigurationService appConfigurationService;
        private readonly ClientFolderCreationService clientFolderCreationService;
        private AppConfigurationModel? appConfig;

        public ProgramRunner(AppConfigurationService appConfigurationService, ClientFolderCreationService clientFolderCreationService)
        {
            this.appConfigurationService = appConfigurationService;
            this.clientFolderCreationService = clientFolderCreationService;
        }

        public async Task Run()
        {
            appConfig = await appConfigurationService.GetAppConfiguration();
            if (appConfig == null || !appConfig.IsValid)
            {
                await UpdateAndStoreAppConfiguration();
                CreateClientFolderStructure();
            }

            while (true)
            {
                string selection = GetUserActionSelection();
                // The selection could be converted into enum for better readability.

                if (selection == "3")
                {
                    break;
                }

                switch (selection)
                {
                    case "1":
                        CreateClientFolderStructure();
                        break;
                    case "2":
                        await UpdateAndStoreAppConfiguration();
                        break;
                    default:
                        throw new NotImplementedException($"Selection {selection} not implemented");
                }
            }
        }

        private static string GetUserActionSelection()
        {
            string[] validSelections = ["1", "2", "3"];
            string? selection;
            do
            {
                Console.WriteLine("Vyberte možnost:");
                Console.WriteLine("1) Vytvořit složky pro nového klienta");
                Console.WriteLine("2) Upravit nastavení");
                Console.WriteLine("3) Ukončit");
                selection = Console.ReadLine();
            } while (!validSelections.Contains(selection));
            return selection!;
        }

        private async Task UpdateAndStoreAppConfiguration()
        {
            appConfig = await appConfigurationService.UpdateAppConfigurationFromUser();
        }

        private void CreateClientFolderStructure()
        {
            clientFolderCreationService.CreateClientFolderStructure(appConfig.TemplateFolderPath, appConfig.ClientsRootPath);
        }
    }
}
