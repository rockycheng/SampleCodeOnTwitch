using System;

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
    public class CommonGivenSteps : Steps
    {
        private static ILog _log = LogManager.GetLogger(typeof(CommonGivenSteps));

        private TimerController TimerController = TimerController.Instance;
        private WebBrowserController WebBrowserController = WebBrowserController.Instance;
        private WebElementsController WebElementsController = WebElementsController.Instance;
        private CommonController CommonController = CommonController.Instance;
        private WebMouseController WebMouseController = WebMouseController.Instance;

        [Given(@"Enter the (.*) web site url")]
        public void GivenEnterThe_WebSiteUrl(string webSite)
        {
            CommonController.OpenWebSite(webSite);
        }

        [Given(@"Clean (.*) By (.*)")]
        public void GivenClean_By_(string elementName, string tag)
        {
            var elementSelector = ElementSelectorOnPageObjects.GetElementSelectorOnTwitchPageObjects(elementName);
            WebBrowserController.RemoveElementContentText(elementSelector,tag);
            Assert.IsTrue(true);
        }


        #region Public Method
        [Given(@"RefreshBrowserPage")]
        public void GivenRefreshBrowserPage()
        {
            WebBrowserController.RefreshBrowserPage();
        }

        [Given(@"Click (.*) By (.*)")]
        public void GivenClick_By_(string elementName, string tag)
        {
            var elementSelector = ElementSelectorOnPageObjects.GetElementSelectorOnTwitchPageObjects(elementName);
            var type = WebElementsController.CheckElementTagType(tag);
            //TODO: Id, cssClass, Xpath
            //TODO: Id, cssClass, Xpath
            if (type == TagConstants.CssSelectorTag)
            {
                CommonController.ClickElementByCssSelector(elementSelector);
            }

        }

        [Given(@"Input (.*) On (.*) By (.*)")]
        public void GivenInout_On_By_(string value, string elementName, string tag)
        {
            var cssSelector = ElementSelectorOnPageObjects.GetElementSelectorOnTwitchPageObjects(elementName);
            WebElementsController.InputValueToElement(cssSelector, value, tag);
        }

        [Given(@"Wait For (.*) seconds")]
        public void GivenWaitFor_Seconds(string sleepTime)
        {
            _log.Info("Sleep for " + sleepTime + "second");
            Thread.Sleep(TimerController.SetDelayTimeBySecond(sleepTime));
        }

        [Given(@"Snapshot (.*) page")]
        public void GivenSnapshot_Page(string shapshotFileName)
        {
            var recordTimeStamp = DateTime.Now.ToString("MMddHHmmss.ffff");
            CommonController.SnapShotProcess($"[{recordTimeStamp}]_CustomizeSnapshot_{shapshotFileName}");
        }

        [Given(@"Mouse move to (.*) content")]
        public void GivenMouseMoveToVideoContent(string elementName)
        {
            var cssSelector = ElementSelectorOnPageObjects.GetElementSelectorOnTwitchPageObjects(elementName);
            WebMouseController.MouseOverToElement(cssSelector);
        }

        [Given(@"ScrollDownToBottom")]
        public void GivenScrollDownToBottom()
        {
            CommonController.ScrollDownToWindowContentViewAreaBottom();
        }

        [Given(@"ScrollUpToTop")]
        public void GivenScrollUpToTop()
        {
            CommonController.ScrollUpToWindowContentViewAreaTop();
        }
        

        [Given(@"ClickLatestVideo On (.*)")]
        public void GivenClickLatestVideoOn_(string videoOwner)
        {
            CommonController.ClickLatestVideo(videoOwner);
        }


        #endregion

    }
}
