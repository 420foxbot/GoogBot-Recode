using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ChatSharp;
using Mono.Data.Sqlite;

namespace Preference_Testing
{
    public partial class ConfigManager
    {
        private Config loadedConfig;
        public Config LoadedConfig
        {
            get { return loadedConfig; }
        }

        public ConfigManager()
        {
            CreateConfigManager("Settings.json");
        }

        public ConfigManager(string configLocation)
        {
            CreateConfigManager(configLocation);
        }

        private void CreateConfigManager(string configLocation)
        {
            Console.WriteLine("No config file specified, looking for 'Settings.json'");
            loadedConfig = new Config();
            LoadConfig(configLocation);
        }
    }
}

