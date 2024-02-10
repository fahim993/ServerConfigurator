using ServerConfigurator.Model;
using ServerConfigurator.Service;
using Xunit;

namespace ServerConfigurator.Tests.Unit
{
    public class ConfigHelperTests
    {
        public class TheParseConfigStringsMethod
        {

            [Fact]
            public void WhenCalled_ReturnsConfigModelWithDefaultValues()
            {
                // Arrange
                var configStrings = new List<string>
            {
                "key1=value1",
                "key2=value2"
            };

                // Act
                var result = ConfigHelper.ParseConfigStrings(configStrings);

                // Assert
                Assert.Equal("value1", result.DefaultValues["key1"]);
                Assert.Equal("value2", result.DefaultValues["key2"]);
            }

            [Fact]
            public void WhenCalled_ReturnsConfigModelWithServerConfigs()
            {
                // Arrange
                var configStrings = new List<string>
            {
                "key1{server1}=value1"
            };

                // Act
                var result = ConfigHelper.ParseConfigStrings(configStrings);

                // Assert
                Assert.Equal("value1", result.ServerConfigs["server1"]["key1"]);
            }
            [Fact]
            public void WhenCalled_ReturnsConfigModelWithDefaultAndServerConfigs()
            {
                // Arrange
                var configStrings = new List<string>
            {
                "key1=value1",
                "key2=value2",
                "key3{server3}=value3"
            };

                // Act
                var result = ConfigHelper.ParseConfigStrings(configStrings);

                // Assert
                Assert.Equal("value1", result.DefaultValues["key1"]);
                Assert.Equal("value2", result.DefaultValues["key2"]);
                Assert.Equal("value3", result.ServerConfigs["server3"]["key3"]);
            }

            [Fact]
            public void WhenCalledWithInvalidConfigLine_ThrowsArgumentException()
            {
                // Arrange
                var configStrings = new List<string>
            {
                "InvalidConfig"
            };

                // Act
                // Assert
                Assert.Throws<ArgumentException>(() => ConfigHelper.ParseConfigStrings(configStrings));
            }

            [Fact]
            public void WhenCalledWithInvalidServerConfig_ThrowsArgumentException()
            {
                // Arrange
                var configStrings = new List<string>
            {
                "key1{server1}"
            };

                // Act
                // Assert
                Assert.Throws<ArgumentException>(() => ConfigHelper.ParseConfigStrings(configStrings));
            }

            [Fact]
            public void WhenCalledWithInvalidDefaultConfig_ThrowsArgumentException()
            {
                // Arrange
                var configStrings = new List<string>
            {
                "key1="
            };

                // Act
                // Assert
                Assert.Throws<ArgumentException>(() => ConfigHelper.ParseConfigStrings(configStrings));
            }
        }

        public class TheConvertConfigModelToStringsMethod
        {

            [Fact]
            public void WhenCalled_ReturnsListOfDefaultStrings()
            {
                // Arrange
                var configModel = new ConfigModel
                {
                    DefaultValues = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                },
                    ServerConfigs = new Dictionary<string, Dictionary<string, string>>
                {
                    { "server1", new Dictionary<string, string> { { "key1", "value1" } } }
                }
                };

                // Act
                var result = ConfigHelper.ConvertConfigModelToStrings(configModel);

                // Assert
                var lines = result.Split(Environment.NewLine);
                Assert.Contains("key1=value1", lines);
                Assert.Contains("key2=value2", lines);
            }

            [Fact]
            public void WhenCalled_ReturnsListOfServerStrings()
            {
                // Arrange
                var configModel = new ConfigModel
                {
                    DefaultValues = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                },
                    ServerConfigs = new Dictionary<string, Dictionary<string, string>>
                {
                    { "server1", new Dictionary<string, string> { { "key1", "value1" } } }
                }
                };

                // Act
                var result = ConfigHelper.ConvertConfigModelToStrings(configModel);

                // Assert
                var lines = result.Split(Environment.NewLine);
                Assert.Contains("key1{server1}=value1", lines);
            }

            [Fact]
            public void WhenCalled_ReturnsListOfDefaultAndServerStrings()
            {
                // Arrange
                var configModel = new ConfigModel
                {
                    DefaultValues = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                },
                    ServerConfigs = new Dictionary<string, Dictionary<string, string>>
                {
                    { "server1", new Dictionary<string, string> { { "key1", "value1" } } }
                }
                };

                // Act
                var result = ConfigHelper.ConvertConfigModelToStrings(configModel);

                // Assert
                var lines = result.Split(Environment.NewLine);
                Assert.Contains("key1=value1", lines);
                Assert.Contains("key2=value2", lines);
                Assert.Contains("key1{server1}=value1", lines);
            }
        }
    }
}
