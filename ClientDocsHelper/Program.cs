using ClientDocsHelper;
using ClientDocsHelper.AppConfiguration;

var configFilePath = Path.Combine(Environment.CurrentDirectory, "config.json");
var programRunner = new ProgramRunner(new AppConfigurationReader(configFilePath), new AppConfigurationWriter(configFilePath));
await programRunner.Run();