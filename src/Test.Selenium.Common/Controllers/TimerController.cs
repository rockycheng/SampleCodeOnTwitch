namespace Test.Selenium.Common.Controllers
{
    using System;
    using Automated.Selenium.SharedLibrary.Services;
    using log4net;

    public class TimerController
    {
        private static ILog _log = LogManager.GetLogger(typeof(TimerController));
        private static Lazy<TimerController> _instance = new Lazy<TimerController>(() => new TimerController());
        private TimerService TimerService => TimerService.Instance;

        public static TimerController Instance
        {
            get { return _instance.Value; }
        }

        public TimeSpan SetDelayTimeBySecond(string assignSecond)
        {
            return SetTimeBySecond(assignSecond);
        }


        #region "private"

        private TimerController()
        {

        }

        private TimeSpan SetTimeBySecond(string assignSecond)
        {
            var timeValue = TimerService.SetTimeBySecond(assignSecond);
            return timeValue;
        }


        #endregion "private"


    }
}
