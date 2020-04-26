using System;
using System.Collections.Generic;
using Automated.Selenium.SharedLibrary.Enums;

namespace Automated.Selenium.SharedLibrary.Models
{
    public class Settings
    {
        public string AutoTestSnapshotPath { get; set; }
        public string TwitchUrl { get; set; }
        public string SeleniumRCUrl { get; set; }
        public BrowserType TestBrowser { get; set; }
        public int DelaySeconds { get; set; }
        public int FindElementWaitingTimeoutBySeconds { get; set; }
        public float DefaultRetryTimeBySeconds { get; set; }
        public int RetryTimes { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }

        public TimeSpan SetFindElementWaitingTimeout => TimeSpan.FromSeconds(Convert.ToDouble(FindElementWaitingTimeoutBySeconds));
    }
}