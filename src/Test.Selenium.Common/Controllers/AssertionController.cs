namespace Test.Selenium.Common.Controllers
{
    using System;
    using Automated.Selenium.SharedLibrary.Services;
    using log4net;

    public class AssertionController
    {
        private static ILog _log = LogManager.GetLogger(typeof(AssertionController));
        private static Lazy<AssertionController> _instance = new Lazy<AssertionController>(() => new AssertionController());
        private Assertions AssertionService => Assertions.Instance;

        #region private

        private AssertionController()
        {
        }

        #endregion

        public static AssertionController Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// Same Url ?
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public bool SameUrl(string expected)
        {
            _log.Info("Confirm with URL: " + expected);
            return AssertionService.SameUrl(expected);
        }
        
        /// <summary>
        /// Same Text ?
        /// </summary>
        /// <param name="expectedResultText"></param>
        /// <param name="actualResultText"></param>
        /// <returns></returns>
        public bool SameText(string expectedResultText, string actualResultText)
        {
            var expectedResult = expectedResultText.Trim();
            var actualResult = actualResultText.Trim();
            _log.Info("Confirm with expected resul text: " + expectedResultText + ", actual result text: " + actualResultText);
            return AssertionService.SameText(expectedResult, actualResult);
        }

        /// <summary>
        /// Same Text Ignore Case ?
        /// </summary>
        /// <param name="expectedResultText"></param>
        /// <param name="actualResultText"></param>
        /// <returns></returns>
        public bool SameTextIgnoreCase(string expectedResultText, string actualResultText)
        {
            var expectedResult = expectedResultText.Trim();
            var actualResult = actualResultText.Trim();
            _log.Info("Confirm string without case, expected result text: " + expectedResult + ", actual result text: " + actualResult);
            return AssertionService.SameTextIgnoreCase(expectedResult, actualResult);
        }

        /// <summary>
        /// Include Text ?
        /// </summary>
        /// <param name="fullText"></param>
        /// <param name="containText"></param>
        /// <returns></returns>
        public bool IncludeText(string fullText, string containText)
        {
            _log.Info("Confirm with the contaon text: " + containText + " in: " + fullText);
            return AssertionService.IncludeText(fullText, containText);
        }

        /// <summary>
        /// Enable Funtion By Id ?
        /// </summary>
        /// <param name="enableOption"></param>
        /// <param name="id"></param>
        /// <returns result="true/false"></returns>
        public bool ConfirmFuntionEnable(string enableOption, string cssSelector)
        {

            return AssertionService.EnableFuntion(enableOption, cssSelector);
        }

    }
}

