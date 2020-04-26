using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Automated.Selenium.SharedLibrary.Models;
using log4net;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Automated.Selenium.SharedLibrary.Services
{
    public class ConfigManager
    {
        private static ILog _log = LogManager.GetLogger(typeof(ConfigManager));
        private static Lazy<ConfigManager> _instance = new Lazy<ConfigManager>(() => new ConfigManager());
        
        public Settings Settings { get; private set; }
        public Dictionary<string, string> TwitchPageObjects { get; private set; }

        private ConfigManager()
        {
            Load();
        }

        public static ConfigManager Instance
        {
            get { return _instance.Value; }
        }

        public void Load()
        {
            var fileName = "Twitch";
            var logForNetFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Configurations\\Log4net", $"{fileName}.config");

            var settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\EnvironmentVariables", $"{fileName}.json");

            var twitchPageObjectsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Pages",$"{fileName}.json");//FilePath:\\Pages

            try
            {
                //log4net
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logForNetFilePath));

                //env setting
                var settingsJson = File.ReadAllText(settingsFilePath, Encoding.UTF8);
                Settings = JsonConvert.DeserializeObject<Settings>(settingsJson);


                //twitch Page Objects
                var twitchJson = File.ReadAllText(twitchPageObjectsFilePath, Encoding.UTF8);
                TwitchPageObjects = JsonConvert.DeserializeObject<Dictionary<string, string>>(twitchJson);

            }
            catch (Exception ex)
            {

                _log.Error(ex.Message, ex);

                throw;
            }
        }

    }
}