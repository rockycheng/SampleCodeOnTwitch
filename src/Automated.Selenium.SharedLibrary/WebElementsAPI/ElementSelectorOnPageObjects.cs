using System;
using Automated.Selenium.SharedLibrary.Services;
using log4net;

namespace Automated.Selenium.SharedLibrary.WebElementsAPI
{
    public class ElementSelectorOnPageObjects
    {
        private static Lazy<ElementSelectorOnPageObjects> _instance = new Lazy<ElementSelectorOnPageObjects>(() => new ElementSelectorOnPageObjects());
        private static ConfigManager ConfigManager => ConfigManager.Instance;
        private static ILog _log = LogManager.GetLogger(typeof(ElementSelectorOnPageObjects));

        #region private

        private ElementSelectorOnPageObjects()
        {
        }

        #endregion

        public static ElementSelectorOnPageObjects Instance
        {
            get { return _instance.Value; }
        }


        /// <summary>
        /// Get Element Selector On IG Page Objects
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static string GetElementSelectorOnTwitchPageObjects(string elementName)
        {
            _log.Info("Get Element Selector: " + elementName + " on Twitch.");
            var selector = ConfigManager.TwitchPageObjects[elementName];
            _log.Info("The Element Selector: " + selector + " on Twitch.");
            return selector;
        }


    }
}
