namespace ServerConfigurator.Model
{
    public class ConfigModel
    {
        public Dictionary<string, string> DefaultValues { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, Dictionary<string, string>> ServerConfigs { get; set; } = new Dictionary<string, Dictionary<string, string>>();
    }
}
