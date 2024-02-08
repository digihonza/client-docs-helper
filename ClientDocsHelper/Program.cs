using ClientDocsHelper;
using ClientDocsHelper.AppConfiguration;
using System.Text;

Console.InputEncoding = Encoding.Unicode;
Console.OutputEncoding = Encoding.Unicode;
var configFilePath = Path.Combine(Environment.CurrentDirectory, "config.json");
var appConfigurationService = new AppConfigurationService(new AppConfigurationReader(configFilePath), new AppConfigurationWriter(configFilePath));
var programRunner = new ProgramRunner(appConfigurationService, new ProjectFolderCreationService());
await programRunner.Run();