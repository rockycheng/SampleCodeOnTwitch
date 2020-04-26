namespace Test.Selenium.Common.Specs
{
    using System;
    using System.Threading;
    using Automated.Selenium.SharedLibrary.Services;
    using Automated.Selenium.SharedLibrary.WebElementsAPI;
    using Controllers;
    using log4net;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class CommonThenSteps : Steps
    {
        private static ILog _log = LogManager.GetLogger(typeof(CommonThenSteps));
        private WebDriverHelper WebDriverHelper = WebDriverHelper.Instance;
        private AssertionController Assertion = AssertionController.Instance;
        private TimerController TimerController = TimerController.Instance;
        private CommonController CommonController = CommonController.Instance;
       

        #region "Private Methods"

        private void ConfirmWebUrl(string relativeUrl)
        {
            var actual = Assertion.SameUrl(relativeUrl);
            if (actual)
            {
                CommonController.CreateAccount();
            }
            Assert.IsTrue(actual);
        }

        #endregion

        #region "Public Methods"

        [Then(@"Confirm (.*) Display (.*) By (.*)")]
        public void ThenConfirm_Display_By_(string elementName, string expectedValue, string tag)
        {
            var elementSelector = ElementSelectorOnPageObjects.GetElementSelectorOnTwitchPageObjects(elementName); 
            var actual = CommonController.ConfirmElementTextIncludeExpectedValue(elementSelector, expectedValue, tag);
            if (!actual)
            {
                var gg = DateTime.Now.ToString("MMddHHmmss.ffff");
                CommonController.SnapShotProcess($"[{gg}]_RESULT_IsTrue(FALSE)");
            }
            Assert.IsTrue(actual);
        }
        
        [Then(@"Close (.*)")]
        public void ThenClose_(string elementName)
        {
            var elementCssSelector = ElementSelectorOnPageObjects.GetElementSelectorOnTwitchPageObjects(elementName);
            var actual = Assertion.IncludeText(WebDriverHelper._webDriver.PageSource, elementCssSelector);
            if (actual)
            {
                var gg = DateTime.Now.ToString("MMddHHmmss.ffff");
               
                CommonController.SnapShotProcess($"[{gg}]_IsClosed_RESULT_IsFalse(FALSE)");
            }
            Assert.IsFalse(actual);
        }


        [Then(@"Wait For (.*) seconds")]
        public void ThenWaitFor_Seconds(string sleepTime)
        {
            _log.Info("Sleep for " + sleepTime + "second");
            Thread.Sleep(TimerController.SetDelayTimeBySecond(sleepTime));
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Combined the relative Url, then enter the web site 
        /// </summary>
        /// <param name="relativeUrl"></param>
        [Then(@"ConfirmWebSite (.*)")]
        public void ThenConfirmWebSite_(string relativeUrl)
        {
            ConfirmWebUrl(relativeUrl);
        }

        [Then(@"Snapshot (.*) page")]
        public void ThenSnapshot_Page(string shapshotFileName)
        {
            var recordTimeStamp = DateTime.Now.ToString("MMddHHmmss.ffff");
            CommonController.SnapShotProcess($"[{recordTimeStamp}]_CustomizeSnapshot_{shapshotFileName}");
        }

        [Then(@"Quit_browserAndDeleteChromeDriverTempFolder")]
        public void ThenQuit_BrowserAndDeleteChromeDriverTempFolder()
        {
            WebDriverHelper.AfterAutomatedTestIsCompleted();
        }
        
        [Then(@"(.*) Clickable is (.*)")]
        public void Then_CanNOTClick(string elementName, string enableOption)
        {
            Thread.Sleep(TimerController.SetDelayTimeBySecond("1"));
            var button = ElementSelectorOnPageObjects.GetElementSelectorOnTwitchPageObjects(elementName);
            var actual = Assertion.ConfirmFuntionEnable(enableOption, button);

            Assert.IsTrue(actual);
        }

        #endregion "Public Methods"

    }
}
