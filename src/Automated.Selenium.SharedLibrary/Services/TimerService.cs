using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Automated.Selenium.SharedLibrary.WebElementsAPI;
using log4net;
using OpenQA.Selenium;

namespace Automated.Selenium.SharedLibrary.Services
{
    using System.Security.Claims;
    using Constants;

    public class TimerService
    {
        private static ILog _log = LogManager.GetLogger(typeof(TimerService));
        private static Lazy<TimerService> _instance = new Lazy<TimerService>(() => new TimerService());
        private ConfigManager ConfigManager =>ConfigManager.Instance;

        public static TimerService Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// Minimum unit is 1 senonds.
        /// </summary>
        /// <returns></returns>
        public TimeSpan SetTimeBySecond(string assignSecond)
        {
            _log.Info("Set dealy time by second: " + assignSecond);
            //1 sec === 1000 millisecond.
            return TimeSpan.FromSeconds(Convert.ToDouble(assignSecond) * ConfigManager.Settings.DelaySeconds);
        }

        /// <summary>
        /// Option will control which time format
        /// </summary>
        /// <returns></returns>
        public string GetLocalTime(string option)
        {
            _log.Info("Get local time with option");
            var dateTime = DateTime.Now;
            var timeFormat = "";
            if (option.Equals(CommonConstants.Snapshot))
            {
                timeFormat = "yyyy_MM_dd_hh_mm";
            }
            else
            {
                timeFormat = "yyyy_MM_dd";
            }
            
            return dateTime.ToString(timeFormat);
        }


        #region "private"

        private TimerService()
        {

        }
        
        #endregion "private"

    }
}
