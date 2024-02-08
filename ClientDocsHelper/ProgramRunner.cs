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
            string? clientName;
            do
            {
                Console.WriteLine("Zadejte jméno klienta:");
                clientName = Console.ReadLine();

            } while (string.IsNullOrWhiteSpace(clientName) || !ValidateFileNameCharacters(clientName));

            CopyClientFolders(appConfig.TemplateFolderPath, Path.Combine(appConfig.ClientsRootPath, clientName));
            Console.WriteLine("Složky vytvořeny.\n");
        }

        private AppConfigurationModel GetAppConfigurationFromUser()
        {
            var updatedAppConfiguration = new AppConfigurationModel();
            string? templatePath;
            if (appConfig?.HasTemplateFolderPath ?? false)
            {
                do
                {
                    Console.WriteLine($"Zadejte nové umístění vzorové struktury složek. Enterem ponecháte stávající ({appConfig.TemplateFolderPath})");
                    templatePath = Console.ReadLine();

                } while (!string.IsNullOrEmpty(templatePath) && !ValidatePath(templatePath));
                templatePath = string.IsNullOrWhiteSpace(templatePath) ? appConfig.TemplateFolderPath : templatePath;
            }
            else
            {
                do
                {
                    Console.WriteLine("Zadejte umístění vzorové struktury složek.");
                    templatePath = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(templatePath) || !ValidatePath(templatePath));

            }

            string? clientsRootPath;
            if (appConfig?.HasClientsRootPath ?? false)
            {
                do
                {
                    Console.WriteLine($"Zadejte nové umístění kmenového adresáře pro klientské složky. Enterem ponecháte stávající ({appConfig.TemplateFolderPath})");
                    clientsRootPath = Console.ReadLine();
                } while (!string.IsNullOrEmpty(clientsRootPath) && !ValidatePath(clientsRootPath));
                clientsRootPath = string.IsNullOrWhiteSpace(clientsRootPath) ? appConfig.ClientsRootPath : clientsRootPath;
            }
            else
            {
                do
                {
                    Console.WriteLine("Zadejte umístění kmenového adresáře pro klientské složky.");
                    clientsRootPath = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(clientsRootPath) || !ValidatePath(clientsRootPath));

            }

            updatedAppConfiguration.TemplateFolderPath = templatePath;
            updatedAppConfiguration.ClientsRootPath = clientsRootPath;

            return updatedAppConfiguration;
        }

        private bool ValidatePath(string path) => ValidatePathCharacters(path) && ValidatePathExists(path);

        private bool ValidatePathExists(string path)
        {
            if (!Path.Exists(path))
            {
                Console.WriteLine("Zadaná složka neexistuje");
                return false;
            }
            return true;
        }

        private bool ValidatePathCharacters(string path)
        {
            var invalidCharIndex = path.IndexOfAny(Path.GetInvalidPathChars());
            if (invalidCharIndex >= 0)
            {
                Console.WriteLine($"Následující znak není povolen: '{path[invalidCharIndex]}'");
                return false;
            }
            return true;
        }

        private bool ValidateFileNameCharacters(string path)
        {
            var invalidCharIndex = path.IndexOfAny(Path.GetInvalidFileNameChars());
            if (invalidCharIndex >= 0)
            {
                Console.WriteLine($"Následující znak není povolen: '{path[invalidCharIndex]}'");
                return false;
            }
            return true;
        }

        private void CopyClientFolders(string templateFolderPath, string destinationFolderPath)
        {
            try
            {
                FileSystem.CopyDirectory(templateFolderPath, destinationFolderPath);
            } catch (Exception ex)
            {
                Console.WriteLine($"Stala se chyba při kopírování složek. ({ex.Message})");
            }
        }
    }
}
