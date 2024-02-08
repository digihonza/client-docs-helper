using ClientDocsHelper.AppConfiguration;

namespace ClientDocsHelper
{
    internal class ProgramRunner
    {
        private readonly AppConfigurationService appConfigurationService;
        private readonly ProjectFolderCreationService projectFolderCreationService;
        private AppConfigurationModel? appConfig;

        public ProgramRunner(AppConfigurationService appConfigurationService, ProjectFolderCreationService projectFolderCreationService)
        {
            this.appConfigurationService = appConfigurationService;
            this.projectFolderCreationService = projectFolderCreationService;
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
                var selection = GetUserActionSelection();
                // The selection could be converted into enum for better readability or at least replaced with constants to eliminate magic strings.

                if (selection == "3")   // Exit
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
                Console.WriteLine("1) Vytvořit složky pro nový projekt");
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
            projectFolderCreationService.CreateProjectFolderStructure(appConfig.TemplateFolderPath, appConfig.ClientsRootPath);
        }
    }
}
