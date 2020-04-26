using System;
using System.Diagnostics;
using System.IO;
using Automated.Selenium.SharedLibrary.Enums;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Automated.Selenium.SharedLibrary.Services
{
    public class WebDriverHelper
    {
        private static ILog _log = LogManager.GetLogger(typeof(WebDriverHelper));
        private static Lazy<WebDriverHelper> _instance = new Lazy<WebDriverHelper>(() => new WebDriverHelper());

        private ConfigManager ConfigManager;

        //create _webDriverHelper until use it.
        public IWebDriver _webDriver = null;

        private WebDriverHelper()
        {
            ConfigManager = ConfigManager.Instance;
        }

        #region "Public Methods"

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IWebDriver GetWebDriver()
        {
            try
            {
                _log.Info("Get WebDriver");
                return _webDriver;
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }

        public static WebDriverHelper Instance
        {
            get { return _instance.Value; }
        }

        #endregion "Public Methods"

        #region "Private Methods"

        /// <summary>
        ///
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        private IWebDriver CreateWebDriver(BrowserType browser)
        {
            _log.Info($"Create Web Driver: {browser}");

            var uri = string.IsNullOrEmpty(ConfigManager.Settings.SeleniumRCUrl) ? null : new Uri(ConfigManager.Settings.SeleniumRCUrl);

            switch (browser)
            {
                case BrowserType.Firefox:
                    return uri == null ? new FirefoxDriver() : new RemoteWebDriver(uri, new FirefoxOptions());

                case BrowserType.Chrome:
                    var options = new ChromeOptions();
                    options.AddArgument("-no-sandbox");
                    return uri == null ? new ChromeDriver(options) : new RemoteWebDriver(uri, options);

                case BrowserType.IE:
                    return uri == null ? new InternetExplorerDriver() : new RemoteWebDriver(uri, new InternetExplorerOptions());
            }

            throw new ArgumentException("Argument for WebDriver incorrect. Please check WebDriver setting in app.config.");
        }

         /// <summary>
        /// Kill All Web Drivers,  avoid getting locked error.
         /// <remarks> geckodriver: Firefox; chromedriver: Chrome; iedriverserver: IE </remarks>
        /// </summary>
        private void KillAllWebDrivers()
        {
            try
            {
                _log.Info("Kill all WebDrivers");
                // get geckodriver.exe
                foreach (var process in Process.GetProcessesByName("geckodriver"))
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
                // get chromedriver.exe
                foreach (var process in Process.GetProcessesByName("chromedriver"))
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
                // get IEDriverServer.exe
                foreach (var process in Process.GetProcessesByName("iedriverserver"))
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
            }
            catch (Exception e)
            {
                _log.Info(e.Message, e);
                throw;
            }
        }

        private void QuitAllWebBrowsers()
        {
            _log.Info("Starting the quit and release the resource.");
            try
            {
                if (_webDriver != null)
                {
                    _log.Info("Closing the all tabs on chrome browser");
                    _webDriver.Close();
                    _log.Info("Close and quit the chrome browser");
                    _webDriver.Quit();
                    _log.Info("Release the chrome web driver resource");
                    _webDriver.Dispose();
                }
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }

         private void CleanTempFolder()
         {
             // delete all "scoped_dir" temp folders 
             string tempfolder = System.IO.Path.GetTempPath();
             string[] tempfiles = Directory.GetDirectories(tempfolder, "scoped_dir*", SearchOption.AllDirectories);
             foreach (string tempfile in tempfiles)
             {
                 try
                 {
                     System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(tempfolder);
                     foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
                     {
                         _log.Info($"[Delete] {subDirectory.FullName}");
                         subDirectory.Delete(true);
                     }
                     directory.Delete();
                 }
                 catch (Exception ex)
                 {
                     _log.Error("File '" + tempfile + "' could not be deleted:\r\n" + "Exception: " + ex.Message + ".");
                 }
             }
         }
        #endregion "Private Methods"

        #region "Specflow"

        /// <summary>
        /// BeforeTestRun->BeforeFeature->BeforeScenario->AfterScenario->AfterFeature->AfterTestRun (a cycle)
        /// </summary>
        public void CreateWebDriverBeforeTestRun()
        {
            _log.Info("================================================================ Create webdriver before test run ================================================================");
            if (ReferenceEquals(_webDriver, null))
            {
                _webDriver = CreateWebDriver(ConfigManager.Settings.TestBrowser);
            }
        }

        public void BeforeFeature()
        {
        }

        public void WebdriverSwitchToDefaultContent()
        {
            _log.Info("The webdriver switches to default content before the scenario.");
            _webDriver.SwitchTo().DefaultContent();
        }

        public void AfterScenario()
        {
        }

        public void AfterFeature()
        {
        }

        public void AfterAutomatedTestIsCompleted()
        {
            _log.Info("The Automated test process is completed.");
            QuitAllWebBrowsers();
            CleanTempFolder();
        }



        #endregion "Specflow"


    }
}
