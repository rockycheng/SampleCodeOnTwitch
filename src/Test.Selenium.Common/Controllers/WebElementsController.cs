namespace Test.Selenium.Common.Controllers
{
    using System;
    using System.Collections.ObjectModel;
    using Automated.Selenium.SharedLibrary.Constants;
    using Automated.Selenium.SharedLibrary.WebElementsAPI;
    using log4net;
    using OpenQA.Selenium;
    using Services;

    public class WebElementsController
    {
        private static ILog _log = LogManager.GetLogger(typeof(WebElementsController));
        private static Lazy<WebElementsController> _instance = new Lazy<WebElementsController>(() => new WebElementsController());
        private WebElementsService WebElementsService => WebElementsService.Instance;
        private WebSelectElementsService WebSelectElementsService => WebSelectElementsService.Instance;
        private CommonController CommonController => CommonController.Instance;
        private WebMouseController WebMouseController => WebMouseController.Instance;
        private WebBrowserController WebBrowserController => WebBrowserController.Instance;
        #region private

        private WebElementsController()
        {
        }

        #endregion

        public static WebElementsController Instance
        {
            get { return _instance.Value; }
        }

        public IWebElement CleanOriginTextInsideElement(IWebElement elementObject)
        {
            var CleanUpElement = WebElementsService.CleanOriginText(elementObject);
            return CleanUpElement;
        }

        public string CheckElementTagType(string tag)
        {
            var resultTag = WebElementsService.CheckElementTagType(tag);
            return resultTag;
        }

        public IWebElement FindElementById(string id)
        {
            var resultElementId = WebElementsService.FindElementById(id);
            return resultElementId;
        }

        /// <summary>
        /// 如果有Element的 Display, Visable or enable 會被設定為 false，一律用Class Name來找，因為這個function，只要Element有存在於DOM檔就可以抓的到！
        /// </summary>
        /// <param name="cssName"></param>
        /// <returns></returns>
        public IWebElement FindElementByCss(string cssName)
        {
            var resultElementCss = WebElementsService.FindElementByCss(cssName);
            return resultElementCss;
        }

        public IWebElement FindElementByCssSelector(string CssSelector)
        {
            var resultElementCssSelector = WebElementsService.FindElementByCssSelector(CssSelector);
            return resultElementCssSelector;
        }

        public IWebElement FindElementByTagName(string tagname)
        {
            var resultElementCssSelector = WebElementsService.FindElementByTagName(tagname);
            return resultElementCssSelector;
        }

        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string CssSelectors)
        {
            var resultElementCssSelectors = WebElementsService.FindElementsByCssSelector(CssSelectors);
            return resultElementCssSelectors;
        }

        public IWebElement FindElementByXpath(string xpath)
        {
            var resultElementXpath = WebElementsService.FindElementByXpath(xpath);
            return resultElementXpath;
        }

        public void SetSelectElement(IWebElement elementSelect, string option)
        {
            WebSelectElementsService.SetSelectElement(elementSelect, option);
        }

        public string GetElementAttributes(IWebElement elementText, string attributeName)
        {
            return WebElementsService.GetElementAttributes(elementText,attributeName);
        }

        public IWebElement GetElementObject(string elementSelector, string tag)
        {
            if (tag == TagConstants.XpathTag)
            {
                return FindElementByXpath(elementSelector);
            }
            return FindElementByCssSelector(elementSelector);
        }

        public void WaitUntilJSReady()
        {
            WebElementsService.WaitUntilJSReady();
        }

        public void WaitUntilAngularReady()
        {
            WebElementsService.WaitUntilAngularReady();
        }

        public void InputValueToElement(string cssSelector, string value, string tag)
        {
            var cleanValue = CommonController.TranslateValueByInputValue(value.Trim());

            _log.Info("Input value: " + cleanValue + " to element: " + cssSelector);
            var element = GetElementObject(cssSelector, tag);
            ////This function is mean one step, so on some page will fail.
            //var cleanElement = CleanOriginTextInsideElement(element);
            //WebBrowserController.SendKey(cleanElement, cleanValue);
            ////SelectAll
            //element.SendKeys(Keys.Control + 'a');
            WebBrowserController.SendKeyEveryFiveHundredMilliseconds(element, cleanValue);
        }


        public void ScrollTheWindowContentViewAreaByJs(int x, int y)
        {
            WebElementsService.ScrollTheViewToLeftistByJs(x,y);
        }

        public void SetAttributeByJavaScript(IWebElement element, string attributeName, string value)
        {
            WebElementsService.SetAttributeByJavaScript(element,attributeName,value);
        }

        public void InputValueToElementWithoutCleanElementText(string cssSelector, string value, string tag)
        {
            var cleanValue = CommonController.TranslateValueByInputValue(value.Trim());

            _log.Info("Input value: " + cleanValue + " to element: " + cssSelector);
            var element = GetElementObject(cssSelector, tag);
            WebMouseController.MouseOverToElementAndClick(element);
            WebBrowserController.SendKey(element, cleanValue);
        }
    }
}
