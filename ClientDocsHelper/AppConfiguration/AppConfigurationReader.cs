using System.Text.Json;

namespace ClientDocsHelper.AppConfiguration
{
    internal class AppConfigurationReader
    {
        private readonly string filePath;

        public AppConfigurationReader(string filePath)
        {
            this.filePath = filePath ?? throw new ArgumentException("File path not provided");
        }

        public async Task<AppConfigurationModel?> ReadConfiguration()
        {
            var configJson = await GerConfigJsonFromFile();
            return string.IsNullOrWhiteSpace(configJson) ? null : GetAppConfigurationFromJson(configJson);
        }

        private async Task<string?> GerConfigJsonFromFile()
        {
            string? configJson = null;
            try
            {
                configJson = await File.ReadAllTextAsync(filePath);
            }
            catch (FileNotFoundException)
            {
                configJson = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stala se chyba při čtení konfiguračního souboru. ({ex.Message})");
            }

            return configJson;
        }

        private AppConfigurationModel? GetAppConfigurationFromJson(string json)
        {
            AppConfigurationModel? appConfiguration = null;
            try
            {
                appConfiguration = JsonSerializer.Deserialize<AppConfigurationModel>(json);
            } catch (JsonException)
            {
                Console.WriteLine($"Stala se chyba při čtení konfiguračního souboru. Obsah konfigurační soubor má špatný formát, musí být validní JSON.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stala se chyba při čtení konfiguračního souboru. ({ex.Message})");
            }
            return appConfiguration;
        }
    }
}
