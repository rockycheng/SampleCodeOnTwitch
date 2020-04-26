using System;
using Automated.Selenium.SharedLibrary.WebElementsAPI;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Automated.Selenium.SharedLibrary.Services
{
    public class Assertions
    {
        private static ILog _log = LogManager.GetLogger(typeof(Assertions));
        private static Lazy<Assertions> _instance = new Lazy<Assertions>(() => new Assertions());
        private ConfigManager ConfigManager => ConfigManager.Instance;
        private WebDriverHelper WebDriverHelper => WebDriverHelper.Instance;
        private WebElementsService WebElementsService => WebElementsService.Instance;

        #region "private"

        private Assertions()
        {

        }


        #endregion "private"

        public static Assertions Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public bool SameUrl(string expected)
        {
            _log.Info("Confirm with URL: " + expected);

            var wait = new WebDriverWait(WebDriverHelper._webDriver, ConfigManager.Settings.SetFindElementWaitingTimeout);

            var actual = wait.Until((webDriver) =>
            {
                //only get pathname, not full url
                var script = "return document.location.pathname;";
                var js = (IJavaScriptExecutor)webDriver;
                var url = (string)js.ExecuteScript(script);
                return expected.Equals(url, StringComparison.CurrentCultureIgnoreCase);

            });
            return actual;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedResultText"></param>
        /// <param name="actualResultText"></param>
        /// <returns></returns>
        public bool SameText(string expectedResultText, string actualResultText)
        {
            var actual = expectedResultText.Equals(actualResultText);
            return actual;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedResulText"></param>
        /// <param name="actualResultText"></param>
        /// <returns></returns>
        public bool SameTextIgnoreCase(string expectedResultText, string actualResultText)
        {
            _log.Info("Confirm with text without case, Expected Result Text: " + expectedResultText + ", Actual Result Text: " + actualResultText);

            var actual = expectedResultText.Trim().Equals(actualResultText.Trim(), StringComparison.CurrentCultureIgnoreCase);
            return actual;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullText"></param>
        /// <param name="containText"></param>
        /// <returns></returns>
        public bool IncludeText(string fullText, string containText)
        {
            _log.Info("Confirm with fullText: " + fullText + " include the contaon text: " + containText);

            //delete the space. and check does fullText include containText
            return fullText.Trim().IndexOf(containText.Trim(), StringComparison.CurrentCultureIgnoreCase) != -1;

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enableOption"></param>
        /// <param name="cssSelector"></param>
        /// <returns result="true/false"></returns>
        public bool EnableFuntion(string enableOption, string cssSelector)
        {
            _log.Info($"Confirm with the element is enable or not by cssSelector: {cssSelector}");

            var element = WebElementsService.FindElementByCssSelector(cssSelector);
            var option = Convert.ToBoolean(enableOption);
            return element.Enabled.Equals(option) && element.Displayed.Equals(option);
        }

    }
}
