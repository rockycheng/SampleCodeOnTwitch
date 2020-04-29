using System;
using System.Drawing;
using System.Threading;
using Automated.Selenium.SharedLibrary.Constants;
using Automated.Selenium.SharedLibrary.Services;
using log4net;
using OpenQA.Selenium;

namespace Automated.Selenium.SharedLibrary.WebElementsAPI
{
    public class WebBrowserService
    {
        private static ILog _log = LogManager.GetLogger(typeof(WebBrowserService));
        private static Lazy<WebBrowserService> _instance = new Lazy<WebBrowserService>(() => new WebBrowserService());
        private ConfigManager ConfigManager => ConfigManager.Instance;
        private WebDriverHelper WebDriverHelper => WebDriverHelper.Instance;
        private TimerService TimerService => TimerService.Instance;

        #region "Private"

        private WebBrowserService()
        {
        }

        /// <summary>
        /// Get Uri
        /// </summary>
        /// <param name="whichSite"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        private Uri GetUri(string whichSite)
        {
            _log.Info($"Get Web site: {whichSite}");
            try
            {
                var url = string.Empty;
                if (whichSite.Equals(CommonConstants.Twitch))
                {
                    url = ConfigManager.Settings.TwitchUrl;
                }
                
                return new Uri(url);
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }

        private void SendKeys(IWebElement element, string value)
        {
            element.SendKeys(value);
        }

        #endregion "Private"

        public static WebBrowserService Instance
        {
            get { return _instance.Value; }
        }

        public void GoToUrl(string whichSite)
        {
            var uri = GetUri(whichSite);
            try
            {
                _log.Info($"Open {whichSite} web site");
                WebDriverHelper._webDriver.Navigate().GoToUrl(uri);
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }

        public void MaximizeWindow()
        {
            try
            {
                var webDriver = WebDriverHelper._webDriver;
                _log.Info("Origin Browser Position: " + webDriver.Manage().Window.Position);
                _log.Info("Origin Browser Size " + webDriver.Manage().Window.Size);
                WebDriverHelper._webDriver.Manage().Window.Maximize();
                _log.Info("After Maximize Browser Size to " + webDriver.Manage().Window.Size);
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }

        /// <summary>
        /// It will always switch to latest Browser window.
        /// </summary>
        public void SwitchToWindow()
        {
            //WindowHandles.Count-1,  avoid array overflow
            var webDriver = WebDriverHelper._webDriver;
            webDriver.SwitchTo().Window(webDriver.WindowHandles[webDriver.WindowHandles.Count - 1]);

        }

        public void SwitchToIFrame(string iframeName)
        {
            try
            {
                _log.Info("Switch To Iframe name: " + iframeName);
                var webDriver = WebDriverHelper._webDriver;
                SwitchToDefault();
                webDriver.SwitchTo().Frame(iframeName);
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void SwitchToDefault()
        {
            try
            {
                WebDriverHelper._webDriver.SwitchTo().DefaultContent();
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }

        public void ClosesTheCurrentWindow()
        {
            var webDriver = WebDriverHelper._webDriver;
            webDriver.Close();
        }

        public void RefreshBrowser()
        {
            var webDriver = WebDriverHelper._webDriver;
            _log.Info("Refresh Browser");
            webDriver.Navigate().Refresh();
        }

        public void KenIn(IWebElement element, string keyValue)
        {
            _log.Info("Key in: " + keyValue + " , on Browser.");
            var key = string.Empty;
            if (keyValue == CommonConstants.KeyTab)
            {
                key = Keys.Tab;
            }

            if (keyValue == CommonConstants.KeyEnter)
            {
                key = Keys.Enter;
            }
           
            if (keyValue == CommonConstants.KeyEnd)
            {
                key = Keys.End;
            }

            if (keyValue == CommonConstants.KeyHome)
            {
                key = Keys.Home;
            }

            Press(0, element, key);
        }

        public void Press(int interval, IWebElement element, string keys)
        {
            var charArray = keys.ToCharArray();
            foreach (var inputChar in charArray)
            {
                SendKeys(element, inputChar.ToString());
                Thread.Sleep(TimeSpan.FromMilliseconds(interval));
            }
        }


        public void SetBrowserSize()
        {
            // driver = new RemoteWebDriver(new URL(grid), capability);
            var webDriver = WebDriverHelper._webDriver;
            _log.Info("Origin Browser Position: " + webDriver.Manage().Window.Position);
            _log.Info("Browser Position set to 0, 0");
            webDriver.Manage().Window.Position = new Point(0, 0);
            _log.Info("After Set Browser Position to " + webDriver.Manage().Window.Position);
            _log.Info("Origin Browser Size " + webDriver.Manage().Window.Size);
            _log.Info("Browser Size set to 1920, 1080");
            webDriver.Manage().Window.Size = new Size(1920, 1080);
            _log.Info("After Set Browser Size to " + webDriver.Manage().Window.Size);
        }

        public void RemoveElementContentText(IWebElement element)
        {
            element.Clear();
        }
    }
}
