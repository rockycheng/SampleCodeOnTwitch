using System;
using Automated.Selenium.SharedLibrary.Services;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Automated.Selenium.SharedLibrary.WebElementsAPI
{
    public class WebMouseService
    {
        private static ILog _log = LogManager.GetLogger(typeof(WebMouseService));
        private static Lazy<WebMouseService> _instance = new Lazy<WebMouseService>(() => new WebMouseService());
        private WebDriverHelper WebDriverHelper => WebDriverHelper.Instance;

        #region "private"

        private WebMouseService()
        {

        }

        #endregion "private"

        public static WebMouseService Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// Drag means source element.
        /// Drop means where is the target element
        /// </summary>
        /// <param name="drag"></param>
        /// <param name="drop"></param>
        public void DragAndDrop(IWebElement drag, IWebElement drop)
        {
            var webDriver = WebDriverHelper._webDriver;
            _log.Info("DragAndDrop");
            Actions action = new Actions(webDriver);
            action.DragAndDrop(drag, drop).Build().Perform();
        }

        public void MouseOverToElementAndClick(IWebElement element)
        {
            _log.Info("MouseOver To Element And Click: " + element);
            var webDriver = WebDriverHelper._webDriver;
            Actions action = new Actions(webDriver);
            action.MoveToElement(element);
            action.Click().Build().Perform();
        }

        public void MouseOverToElement(IWebElement element)
        {
            _log.Info("MouseOver To Element: " + element);
            var webDriver = WebDriverHelper._webDriver;
            Actions action = new Actions(webDriver);
            action.MoveToElement(element);
        }

        public void MouseClick(IWebElement element)
        {
            _log.Info("Click Element: " + element);
            element.Click();
        }

        public void MouseOverToElementAndDoubleClick(IWebElement element)
        {
            _log.Info("MouseOver To Element And Double Click: " + element);
            var webDriver = WebDriverHelper._webDriver;
            Actions action = new Actions(webDriver);
            action.MoveToElement(element);
            action.DoubleClick().Build().Perform();
        }



    }
}
