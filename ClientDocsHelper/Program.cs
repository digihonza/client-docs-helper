using ClientDocsHelper;
using ClientDocsHelper.AppConfiguration;

var configFilePath = Path.Combine(Environment.CurrentDirectory, "config.json");
var appConfigurationService = new AppConfigurationService(new AppConfigurationReader(configFilePath), new AppConfigurationWriter(configFilePath));
var programRunner = new ProgramRunner(appConfigurationService, new ClientFolderCreationService());
await programRunner.Run();