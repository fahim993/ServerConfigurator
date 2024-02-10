using ServerConfigurator.Model;

namespace ServerConfigurator.Service
{
    public interface IConfigService
    {
        ConfigModel GetConfigModel();
        void SetConfigModel(ConfigModel configModel);
    }

    public class ConfigService : IConfigService
    {
        private readonly IConfigFileReaderWriter _fileReaderWriter;

        public ConfigService(IConfigFileReaderWriter fileReaderWriter)
        {
            _fileReaderWriter = fileReaderWriter;
        }

        public ConfigModel GetConfigModel()
        {
            var stringConfigs = _fileReaderWriter.ReadConfig();

            return ConfigHelper.ParseConfigStrings(stringConfigs);
        }

        public void SetConfigModel(ConfigModel configModel)
        {
            var strings = ConfigHelper.ConvertConfigModelToStrings(configModel);

            _fileReaderWriter.SaveConfig(strings);
        }
    }
}
