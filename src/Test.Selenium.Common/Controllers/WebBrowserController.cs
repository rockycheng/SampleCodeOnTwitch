namespace Test.Selenium.Common.Controllers
{
    using System;
    using Automated.Selenium.SharedLibrary.WebElementsAPI;
    using log4net;
    using OpenQA.Selenium;

    public class WebBrowserController
    {
        private static ILog _log = LogManager.GetLogger(typeof(WebBrowserController));
        private static Lazy<WebBrowserController> _instance = new Lazy<WebBrowserController>(() => new WebBrowserController());

        private WebBrowserService WebBrowserService => WebBrowserService.Instance;
        private WebElementsController WebElementsController => WebElementsController.Instance;

        #region private

        private WebBrowserController()
        {
        }

        #endregion


        public static WebBrowserController Instance => _instance.Value;

        public void SetRunTestURL(string whichSite)
        {
            _log.Info("Set Run Test URL");
            WebBrowserService.GoToUrl(whichSite);
        }

        /// <summary>
        /// Making browser window size fit full screen
        /// </summary>
        public void MaximizeBrowserWindow()
        {
            _log.Info("Maximize Browser Window");
            WebBrowserService.MaximizeWindow();
        }

        public void ClosesTheCurrentWindow()
        {
            _log.Info("Closes The Current Window");
            WebBrowserService.ClosesTheCurrentWindow();
        }

        public void SwitchToLatestWindow()
        {
            _log.Info("Switch To Latest Window");
            WebBrowserService.SwitchToWindow();
        }

        public void RefreshBrowserPage()
        {
            _log.Info("Refresh Browser Page");
            WebBrowserService.RefreshBrowser();
        }

        public void SwitchToDefaultPageContainer()
        {
            _log.Info("Switch To Default Page Container");
            WebBrowserService.SwitchToDefault();
        }

        public void SwitchToIframe(string iframeId)
        {
            _log.Info("Switch To Iframe: " + iframeId);
            WebBrowserService.SwitchToIFrame(iframeId);
        }

        public void KeyIn(IWebElement element, string keyValue)
        {
            WebBrowserService.KenIn(element, keyValue);
        }

        public void RemoveElementContentText(string cssSelectorName, string tag)
        {
            var element = WebElementsController.FindElementByCssSelector(cssSelectorName);
            WebBrowserService.RemoveElementContentText(element);
        }

        public void SendKey(IWebElement element, string inputValue)
        {
            WebBrowserService.Press(100, element, inputValue);
        }

        public void SendKeyEveryOneSecond(IWebElement element, string inputValue)
        {
            WebBrowserService.Press(1000, element, inputValue);
        }

        public void SetBrowserSizeFromConfig()
        {
            WebBrowserService.SetBrowserSize();
        }

    }
}
