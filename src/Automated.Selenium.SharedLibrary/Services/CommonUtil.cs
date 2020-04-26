                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                using System.Text.RegularExpressions;
using log4net;
using OpenQA.Selenium;
using static Automated.Selenium.SharedLibrary.Constants.TagConstants;

namespace Automated.Selenium.SharedLibrary.Services
{
    public class CommonUtil
    {
        private static ILog _log = LogManager.GetLogger(typeof(CommonUtil));

        public static string GetNormalizeXpath(string tag, string attribute, string value)
        {
            var xpath = string.Format($"//{tag}" + $"[@{attribute}={value}]");
            _log.Info("NormalizeXpath: " + xpath);
            return xpath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetElementText(IWebElement element)
        {
            var attributeValue = GetAttributeValue(element.TagName);
            var elementText = element.GetAttribute(attributeValue).Trim();
            //remove spaces and newlines in a string 
            var text = Regex.Replace(elementText, @"\t|\n|\r|\s", "");
            _log.Info("Get Element Text: " + text);
            return text;
        }

        /// <summary>
        /// Get Element Selector On Game Console Page Objects
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static string GetAttributeValue(string elementTagName)
        {
            if (elementTagName != null && (elementTagName == TagInput || elementTagName == TagTextarea))
            {
                //get value
                return "value";
            }
            //get textContent
            return "textContent";
        }
    }
}
