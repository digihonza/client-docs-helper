using ClientDocsHelper.AppConfiguration;
using Microsoft.VisualBasic.FileIO;

namespace ClientDocsHelper
{
    internal class ProgramRunner
    {
        private readonly AppConfigurationReader appConfigurationReader;
        private readonly AppConfigurationWriter appConfigurationWriter;
        private AppConfigurationModel? appConfig;

        public ProgramRunner(AppConfigurationReader appConfigurationReader, AppConfigurationWriter appConfigurationWriter)
        {
            this.appConfigurationReader = appConfigurationReader;
            this.appConfigurationWriter = appConfigurationWriter;
        }

        public async Task Run()
        {
            appConfig = await appConfigurationReader.ReadConfiguration();
            if (appConfig == null || !appConfig.IsValid)
            {
                await UpdateAndStoreAppConfiguration();
                CreateClientFolderStructure();
            }

            while (true)
            {
                string selection = GetUserActionSelection();

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
                Console.WriteLine("Vyberte možnost.");
                Console.WriteLine("1) Vytvořit složky pro nového klienta.");
                Console.WriteLine("2) Upravit nastavení.");
                Console.WriteLine("3) Ukončit.");
                selection = Console.ReadLine();
            } while (!validSelections.Contains(selection));
            return selection!;
        }

        private async Task UpdateAndStoreAppConfiguration()
        {
            appConfig = GetAppConfigurationFromUser();
            await appConfigurationWriter.SaveAppConfiguration(appConfig);
        }

        private void CreateClientFolderStructure()
        {
            Console.WriteLine("Zadejte jméno klienta:");
            var clientName = Console.ReadLine();

            // TODO: validate name for invalid path characters

            // TODO: Exception handling
            FileSystem.CopyDirectory(appConfig.TemplateFolderPath, Path.Combine(appConfig.ClientsRootPath, clientName));
            Console.WriteLine("Složky vytvořeny.");
        }

        private AppConfigurationModel GetAppConfigurationFromUser()
        {
            var updatedAppConfiguration = new AppConfigurationModel();
            string? templatePath = null;
            if (appConfig?.HasTemplateFolderPath ?? false)
            {
                Console.WriteLine($"Zadejte nové umístění vzorové struktury složek. Enterem ponecháte stávající ({appConfig.TemplateFolderPath})");
                // TODO: validate name for invalid path characters
                templatePath = Console.ReadLine();
                templatePath = string.IsNullOrWhiteSpace(templatePath) ? appConfig.TemplateFolderPath : templatePath;
            }
            else
            {
                do
                {
                    Console.WriteLine("Zadejte umístění vzorové struktury složek.");
                    templatePath = Console.ReadLine();
                    // TODO: validate name for invalid path characters

                } while (string.IsNullOrWhiteSpace(templatePath));

            }

            string? clientsRootPath = null;
            if (appConfig?.HasClientsRootPath ?? false)
            {
                Console.WriteLine($"Zadejte nové umístění kmenového adresáře pro klientské složky. Enterem ponecháte stávající ({appConfig.TemplateFolderPath})");
                // TODO: validate name for invalid path characters
                clientsRootPath = Console.ReadLine();
                clientsRootPath = string.IsNullOrWhiteSpace(clientsRootPath) ? appConfig.ClientsRootPath : clientsRootPath;
            }
            else
            {
                do
                {
                    Console.WriteLine("Zadejte umístění kmenového adresáře pro klientské složky.");
                    clientsRootPath = Console.ReadLine();
                    // TODO: validate name for invalid path characters

                } while (string.IsNullOrWhiteSpace(clientsRootPath));

            }

            updatedAppConfiguration.TemplateFolderPath = templatePath;
            updatedAppConfiguration.ClientsRootPath = clientsRootPath;

            return updatedAppConfiguration;
        }
    }
}
