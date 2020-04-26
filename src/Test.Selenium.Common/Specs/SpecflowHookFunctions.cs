namespace Test.Selenium.Common.Specs
{
    //Reference: https://github.com/techtalk/SpecFlow/wiki/Hooks
    using System.Diagnostics;
    using Automated.Selenium.SharedLibrary.Services;
    using log4net;
    using TechTalk.SpecFlow;

    /// <summary>
    /// BeforeTestRun->BeforeFeature->BeforeScenario->AfterScenario->AfterFeature->AfterTestRun (a cycle)
    /// </summary>
    [Binding]
    public class SpecflowHookFunctions
    {
        #region SpecFlow
        private static ILog _log = LogManager.GetLogger(typeof(SpecflowHookFunctions));
        private static WebDriverHelper WebDriverHelper => WebDriverHelper.Instance;

        //private SpecflowHookFunctions()
        //{
        //}
        static Stopwatch swForFeature = new Stopwatch();//引用stopwatch物件
        static Stopwatch swForScenario = new Stopwatch();//引用stopwatch物件

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _log.Info("================================================================ Before the test run does some process ================================================================");
            WebDriverHelper.CreateWebDriverBeforeTestRun();
        }

        /// <summary>
        /// Feature -> Scenario
        /// </summary>
        [BeforeFeature(Order = 0)]
        public static void BeforeFeature()
        {
            //Before the feature does some process, one feature runs once.
            var featureName = FeatureContext.Current.FeatureInfo.Title;
            _log.Info($"================================================================ [Start Feature] {featureName} ================================================================");
            _log.Warn($"================================================================ [Start Feature] {featureName} ================================================================");
            swForFeature.Reset();
            swForFeature.Start();
            _log.Info($"Start watch feature time");
        }

        /// <summary>
        /// 
        /// </summary>
        [BeforeScenario(Order = 0)]
        public static void BeforeScenario()
        {
            var scenarioName = ScenarioContext.Current.ScenarioInfo.Title;
            _log.Info($"================================================================ [Start Scenario] {scenarioName} ================================================================");
            swForScenario.Reset();
            swForScenario.Start();
            _log.Info($"Start watch scenario time");
            WebDriverHelper.WebdriverSwitchToDefaultContent();
        }



        /// <summary>
        /// 
        /// </summary>
        [AfterScenario(Order = 0)]
        public static void AfterScenario()
        {
            //After the scenario does some process
            swForScenario.Stop();
            var scenarioName = ScenarioContext.Current.ScenarioInfo.Title;
            var scenarioRunTime = swForScenario.Elapsed.TotalMilliseconds;
            _log.Info($"{scenarioName} run time (ms): {scenarioRunTime}");
            _log.Warn($"{scenarioName} run time (ms): {scenarioRunTime}");
            _log.Info($"================================ [Scenario Done] {scenarioName} ================================");
        }

        /// <summary>
        /// 
        /// </summary>
        [AfterFeature(Order = 0)]
        public static void AfterFeature()
        {
            //After the feature dose some process.
            swForFeature.Stop();
            var featureName = FeatureContext.Current.FeatureInfo.Title;
            var featureRunTime = swForFeature.Elapsed.TotalMilliseconds;
            _log.Info($"{featureName}, totally feature run time (ms): {featureRunTime}");
            _log.Warn($"{featureName}, totally feature run time (ms): {featureRunTime}");
            _log.Info($"================================ [Feature Done] {featureName} ================================");
            _log.Warn($"================================ [Feature Done] {featureName} ================================");
            _log.Info("=======================================================================================================");
            _log.Warn("=======================================================================================================");
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            //After the test run dose some process.
            _log.Info("=======================================================================================================");
        }

        #endregion SpecFlow
    }
}
