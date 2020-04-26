namespace Test.Selenium.Common.Specs
{
    using System.Threading;
    using Automated.Selenium.SharedLibrary.Constants;
    using Automated.Selenium.SharedLibrary.WebElementsAPI;
    using Controllers;
    using log4net;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class CommonWhenSteps : Steps
    {
        private static ILog _log = LogManager.GetLogger(typeof(CommonWhenSteps));
        private WebBrowserController WebBrowserController = WebBrowserController.Instance;
        private WebElementsController WebElementsController = WebElementsController.Instance;
        private TimerController TimerController = TimerController.Instance;
        private CommonController CommonController = CommonController.Instance;
        
        #region "Public"

        [When(@"Click (.*) By (.*)")]
        public void WhenClick_By_(string elementName, string tag)
        {
            var elementSelector = ElementSelectorOnPageObjects.GetElementSelectorOnTwitchPageObjects(elementName);
            var type = WebElementsController.CheckElementTagType(tag);
            //TODO: Id, cssClass, Xpath
            if (type == TagConstants.CssSelectorTag)
            {
                CommonController.ClickElementByCssSelector(elementSelector);
            }

        }

        [When(@"Wait For (.*) seconds")]
        public void WhenWaitFor_Seconds(string sleepTime)
        {
            _log.Info("Sleep for " + sleepTime + "second");
            Thread.Sleep(TimerController.SetDelayTimeBySecond(sleepTime));
            Assert.IsTrue(true);
        }
                
        [When(@"ScrollTheWindowContentViewAreaToTop")]
        public void WhenScrollTheWindowContentViewAreaToTop()
        {
            //Y
            //Up < 0
            //Down > 0
            WebElementsController.ScrollTheWindowContentViewAreaByJs(0, -10000);
        }

        [When(@"Input (.*) On (.*) By (.*)")]
        public void WhenInout_On_By_(string value, string elementName, string tag)
        {
            var cssSelector = ElementSelectorOnPageObjects.GetElementSelectorOnTwitchPageObjects(elementName);
            WebElementsController.InputValueToElement(cssSelector, value, tag);
        }

        #endregion
    }
}
