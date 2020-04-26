using System;
using System.IO;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace Automated.Selenium.SharedLibrary.Services
{
    using Constants;

    public class SnapshotServices
    {
        private static ILog _log = LogManager.GetLogger(typeof(SnapshotServices));
        private static Lazy<SnapshotServices> _instance = new Lazy<SnapshotServices>(() => new SnapshotServices());
        private WebDriverHelper WebDriverHelper => WebDriverHelper.Instance;
        private TimerService TimerService => TimerService.Instance;
        private ConfigManager ConfigManager => ConfigManager.Instance;

        #region "private"

        private SnapshotServices()
        {
        }

        private void CreateSnapShotFolder(string location)
        {
            _log.Info("Start Create SnapShot Folder.");
            var path = location;
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    _log.Info("That path exists already.");
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                _log.Info($"The directory was created successfully at {Directory.GetCreationTime(path)}.");

                // Delete the directory.
                //di.Delete();
                //_log.Info("The directory was deleted successfully.");

            }
            catch (Exception e)
            {
                _log.Error($"The process failed: {e.ToString()}");
            }
            finally
            {
                _log.Info("End Create SnapShot Folder.");

            }

        }
        private void TakeBrowserSnapshot(string location, string reason)
        {
            _log.Info($"Take Snapshot, Reason: {reason}");
            var webDriver = WebDriverHelper._webDriver;
            //reason_yyyy_MM_DD_hh_mm.png
            //TEST_January_9_2019_17_24.png
            var formatReason = reason.Replace(" ", "_");
            var time = TimerService.GetLocalTime(CommonConstants.Snapshot);
            var fileName = string.Format($"{formatReason}_{time}.png");
            var filePath = Path.Combine(location, fileName);

            //ScreenShot will save all browser window content.
            var browserScreenshots = webDriver.TakeScreenshot();
            browserScreenshots.SaveAsFile(filePath, ScreenshotImageFormat.Png);
        }

        #endregion

        public static SnapshotServices Instance
        {
            get { return _instance.Value; }
        }

        public void SnapShotProcess(string reason)
        {
            var time = TimerService.GetLocalTime(CommonConstants.SnapshotPath);
            var path = ConfigManager.Settings.AutoTestSnapshotPath + $"{time}";
            var location = Path.Combine(path);
            CreateSnapShotFolder(location);
            TakeBrowserSnapshot(location, reason);
        }
    }

}
