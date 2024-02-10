using ServerConfigurator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerConfigurator.Service
{
    public static class ConfigHelper
    {
        public static ConfigModel ParseConfigStrings(List<string> configLines)
        {
            Dictionary<string, string> DefaultValues = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, string>> ServerConfigs = new Dictionary<string, Dictionary<string, string>>();

            foreach (var line in configLines)
            {
                // if string starts with ; or it's empty we can skip it
                if (string.IsNullOrEmpty(line) || line.StartsWith(";"))
                {
                    continue;
                }
                var parts = line.Split("=");
                if (parts.Length != 2)
                {
                    throw new ArgumentException("Invalid config line");
                }

                var configKey = parts[0].Trim();
                var configValue = parts[1].Trim();

                // if the configKey contains { and } it's a server config
                if (configKey.Contains("{") && configKey.EndsWith("}"))
                {
                    // it's a server config
                    // need to split the configName and serverName
                    var serverName = configKey.Split("{")[1].Split("}")[0];
                    var configName = configKey.Split("{")[0];

                    if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(configKey) || string.IsNullOrEmpty(configValue))
                    {
                        // todo: add more detailed error message
                        throw new ArgumentException("Invalid configs");
                    }

                    if (!ServerConfigs.ContainsKey(serverName))
                    {
                        ServerConfigs[serverName] = new Dictionary<string, string>();
                    }

                    ServerConfigs[serverName][configName] = configValue;
                }
                else
                {
                    // it's a default config
                    if (string.IsNullOrEmpty(configKey) || string.IsNullOrEmpty(configValue))
                    {
                        // todo: add more detailed error message
                        throw new ArgumentException("Invalid config");
                    }

                    DefaultValues[configKey] = configValue;
                }
            }

            return new ConfigModel
            {
                DefaultValues = DefaultValues,
                ServerConfigs = ServerConfigs
            };
        }

        public static string ConvertConfigModelToStrings(ConfigModel configModel)
        {
            var sb = new StringBuilder();
            sb.AppendLine(";START DEFAULTS");
            foreach (var defaultConfig in configModel.DefaultValues)
            {
                sb.AppendLine($"{defaultConfig.Key}={defaultConfig.Value}");
            }
            sb.AppendLine(";END DEFAULTS\n");

            foreach (var serverConfig in configModel.ServerConfigs)
            {
                sb.AppendLine($";START {serverConfig.Value}");
                foreach (var config in serverConfig.Value)
                {
                    sb.AppendLine($"{config.Key}{{{serverConfig.Key}}}={config.Value}");
                }
                sb.AppendLine($";END {serverConfig.Value}\n");
            }

            return sb.ToString();
        }
    }
}
