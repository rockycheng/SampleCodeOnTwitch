namespace Test.Selenium.Common.Controllers
{
    using System;
    using System.Threading;
    using Automated.Selenium.SharedLibrary.Models;
    using Automated.Selenium.SharedLibrary.Services;
    using log4net;
    using Services;

    public class CommonController
    {
        private static ILog _log = LogManager.GetLogger(typeof(CommonController));
        private static Lazy<CommonController> _instance = new Lazy<CommonController>(() => new CommonController());
        private CommonService CommonService => CommonService.Instance;
        private AssertionController AssertionController => AssertionController.Instance;
        private TimerController TimerController => TimerController.Instance;
        private SnapshotServices SnapshotServices => SnapshotServices.Instance;
        private WebElementsController WebElementsController => WebElementsController.Instance;
        private WebBrowserController WebBrowserController => WebBrowserController.Instance;
        private WebMouseController WebMouseController => WebMouseController.Instance;

        //reTry function
        public delegate void DelgFunction();

        public static CommonController Instance
        {
            get { return _instance.Value; }
        }
        
        public void OpenWebSite(string webSite)
        {
            _log.Info($"Open {webSite} web site");
            //Maximize Browser Window
            //WebBrowserController.MaximizeBrowserWindow();
            WebBrowserController.SetBrowserSizeFromConfig();
            WebBrowserController.SetRunTestURL(webSite);
        }


        public bool InputProcess(string value)
        {
            _log.Info("Input Process");
            var result = CommonService.InputProcess(value);
            _log.Info("Input Process: " + result);
            return result;
        }

        public bool InputAccountProcess(string account)
        {
            _log.Info("Input Account Process");
            var result = CommonService.InputAccount(account);
            _log.Info("Input Account Process: " + result);
            return result;
        }

        /// <summary>
        /// Create password step ?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool InputPasswordProcess(string value)
        {
            _log.Info("Input Password Process");
            var result = CommonService.InputPassword(value);
            _log.Info("Input Password Process: " + result);
            return result;
        }

 
        public void SnapShotProcess(string reason)
        {
            SnapshotServices.SnapShotProcess(reason);
        }
      
        public bool ConfirmElementTextIncludeExpectedValue(string cssSelector, string expectedValue, string tag)
        {
            Thread.Sleep(TimerController.SetDelayTimeBySecond("1"));
            _log.Info("Confirm element text include expected value.");

            var element = WebElementsController.GetElementObject(cssSelector, tag);

            var elementText = CommonUtil.GetElementText(element);
            return AssertionController.IncludeText(elementText, expectedValue);
        }
      
        public string TranslateValueByInputValue(string value)
        {
            _log.Info("Translate origin: " + value);

            var result = string.Empty;
            var inputProcess = InputProcess(value);
            var accountProcess = InputAccountProcess(value);
            var passwordProcess = InputPasswordProcess(value);

            _log.Info("InputProcess: " + inputProcess);
            if (inputProcess)
            {
                if (accountProcess)
                {
                    result = CommonService.GetAccount();
                }

                if (passwordProcess)
                {
                    result = CommonService.GetPassword();
                }
            }
            else
            {
                result = value;
            }
            _log.Info("Translate process done: " + result);
            return result;
        }

        #region private

        private CommonController()
        {
        }

        #endregion


        public void ClickElementByCssSelector(string elementSelector)
        {
            Thread.Sleep(TimerController.SetDelayTimeBySecond("1"));
            _log.Info("Click element by CSS.");
            var element = WebElementsController.FindElementByCssSelector(elementSelector);
            WebMouseController.MouseOverToElementAndClick(element);
        }

        public void CreateAccount()
        {
            CommonService.CreateAccount();
        }
    }
}
