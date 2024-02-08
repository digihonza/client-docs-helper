using System.Text.Json;

namespace ClientDocsHelper.AppConfiguration
{
    internal class AppConfigurationWriter
    {
        private readonly string filePath;

        public AppConfigurationWriter(string filePath)
        {
            this.filePath = filePath ?? throw new ArgumentException("File path not provided");
        }

        public async Task SaveAppConfiguration(AppConfiguration appConfiguration)
        {
            var configJson = JsonSerializer.Serialize(appConfiguration, new JsonSerializerOptions { WriteIndented = true });
            try
            {
                await File.WriteAllTextAsync(filePath, configJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stala se chyba při ukládání konfiguračního souboru. ({ex.Message})");
            }
        }
    }
}
