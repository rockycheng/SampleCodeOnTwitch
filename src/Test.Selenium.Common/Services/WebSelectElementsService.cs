namespace Test.Selenium.Common.Services
{
    using System;
    using Controllers;
    using log4net;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class WebSelectElementsService
    {
        private static ILog _log = LogManager.GetLogger(typeof(WebSelectElementsService));
        private static Lazy<WebSelectElementsService> _instance = new Lazy<WebSelectElementsService>(() => new WebSelectElementsService());

        private CommonController CommonController => CommonController.Instance;
       
        public static WebSelectElementsService Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementSelect"></param>
        /// <param name="option"></param>
        public void SetSelectElement(IWebElement elementSelect, string option)
        {
            var inputProcess = CommonController.InputProcess(option);

            var finalOption = option.Trim();

           var select = new SelectElement(elementSelect);

          
            //select.SelectByText(finalOption);
            select.WrappedElement.Click();
            select.WrappedElement.SendKeys(finalOption);
            //select.WrappedElement.Click(); or select.WrappedElement.SendKeys(Keys.Enter); is the same.
            select.WrappedElement.SendKeys(Keys.Enter);

            
        }

        private WebSelectElementsService()
        {
        }

        public void SetSelectElementOnReportBO(IWebElement elementSelect, string option)
        {

            var finalOption = option.Trim();
            var select = new SelectElement(elementSelect);
            select.SelectByText(finalOption);
        }
    }
}
