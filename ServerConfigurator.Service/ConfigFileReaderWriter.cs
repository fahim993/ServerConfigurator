using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerConfigurator.Service
{
    public interface IConfigFileReaderWriter
    {
        public List<string> ReadConfig();
        public void SaveConfig(string fileContent);
    }

    public class ConfigFileReaderWriter : IConfigFileReaderWriter
    {
        public string FilePath { get; }

        public ConfigFileReaderWriter(string filePath)
        {
            FilePath = filePath;
        }

        public List<string> ReadConfig()
        {
            // Check if the file exists before reading
            if (File.Exists(FilePath))
            {
                return File.ReadAllLines(FilePath).ToList();
            }
            else
            {
                throw new FileNotFoundException($"The file {FilePath} does not exist.");
            }
        }

        public void SaveConfig(string fileContent)
        {
            try
            {
                File.WriteAllText(FilePath, fileContent);
            }
            catch (Exception e)
            {
                // todo: add better error handling
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
