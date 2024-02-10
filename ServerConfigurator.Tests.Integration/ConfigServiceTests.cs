using Moq;
using ServerConfigurator.Model;
using ServerConfigurator.Service;
using Xunit;

namespace ServerConfigurator.Tests.Integration
{
    public class ConfigServiceTests
    {
        public class TheGetConfigModelMethod
        {
            [Fact]
            public void ReturnsConfigModelWithDefaultValues()
            {
                // Arrange
                var configLines = new List<string>
                {
                    ";START DEFAULTS",
                    "key1=value1",
                    "key2=value2",
                    ";END DEFAULTS"
                };
                // create stub file reader writer
                var fileReaderWriter = new Mock<IConfigFileReaderWriter>();
                fileReaderWriter.Setup(x => x.ReadConfig()).Returns(configLines);

                var configService = new ConfigService(fileReaderWriter.Object);

                // Act
                var result = configService.GetConfigModel();

                // Assert
                Assert.NotNull(result);
                Assert.Equal("value1", result.DefaultValues["key1"]);
                Assert.Equal("value2", result.DefaultValues["key2"]);
            }

            [Fact]
            public void ReturnsConfigModelWithServerConfigs()
            {
                // Arrange
                var configLines = new List<string>
                {
                    "key1{server1}=value1",
                    "key2{server1}=value2"
                };
                var fileReaderWriter = new Mock<IConfigFileReaderWriter>();
                fileReaderWriter.Setup(x => x.ReadConfig()).Returns(configLines);

                var configService = new ConfigService(fileReaderWriter.Object);

                // Act
                var result = configService.GetConfigModel();

                // Assert
                Assert.NotNull(result);
                Assert.Equal("value1", result.ServerConfigs["server1"]["key1"]);
                Assert.Equal("value2", result.ServerConfigs["server1"]["key2"]);
            }
        }

        public class TheSetConfigModelMethod
        {
            [Fact]
            public void SavesConfigModel()
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
                        {
                            "server1", new Dictionary<string, string>
                            {
                                { "key1", "value1" },
                                { "key2", "value2" }
                            }
                        }
                    }
                };
                var fileReaderWriter = new Mock<IConfigFileReaderWriter>();
                var configService = new ConfigService(fileReaderWriter.Object);

                // Act
                configService.SetConfigModel(configModel);

                // Assert
                fileReaderWriter.Verify(x => x.SaveConfig(It.IsAny<string>()), Times.Once);
            }
        }
    }
}