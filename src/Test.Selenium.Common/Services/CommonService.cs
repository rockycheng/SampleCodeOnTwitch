namespace Test.Selenium.Common.Services
{
    using System;
    using Automated.Selenium.SharedLibrary.Constants;
    using Automated.Selenium.SharedLibrary.Services;
    using log4net;

    internal class CommonService
    {
        private ConfigManager ConfigManager => ConfigManager.Instance;
        private static ILog _log = LogManager.GetLogger(typeof(CommonService));
        private static Lazy<CommonService> _instance = new Lazy<CommonService>(() => new CommonService());
        private string useraccount = String.Empty;
        private string password = String.Empty;

        public static CommonService Instance
        {
            get { return _instance.Value; }
        }

        public bool InputProcess(string value)
        {
            var account = InputAccount(value);
            var password = InputPassword(value);

            return (account || password);
        }



        /// <summary>
        /// Does create account step ?
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool InputAccount(string account)
        {

            var option = account.Equals(CommonConstants.Account, StringComparison.CurrentCultureIgnoreCase);
            return option;

        }

        /// <summary>
        /// Does keyin account password step ?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool InputPassword(string value)
        {
            var option = value.Equals(CommonConstants.Password, StringComparison.CurrentCultureIgnoreCase);
            return option;
        }

        #region private

        private CommonService()
        {
        }


        #endregion

        public string GetAccount()
        {
            return useraccount;
        }

        public string GetPassword()
        {
            return password;
        }

        public void CreateAccount()
        {
            _log.Info("Create Account");
            useraccount = ConfigManager.Settings.Account;
            password = ConfigManager.Settings.Password;
        }
    }
}
