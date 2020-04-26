using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Automated.Selenium.SharedLibrary.Constants;
using Automated.Selenium.SharedLibrary.Services;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Automated.Selenium.SharedLibrary.WebElementsAPI
{
    public class WebElementsService
    {
        private static ILog _log = LogManager.GetLogger(typeof(WebElementsService));
        private static Lazy<WebElementsService> _instance = new Lazy<WebElementsService>(() => new WebElementsService());
        private ConfigManager ConfigManager => ConfigManager.Instance;
        private WebDriverHelper WebDriverHelper => WebDriverHelper.Instance;
        private TimerService TimerService => TimerService.Instance;
        private SnapshotServices SnapshotServices => SnapshotServices.Instance;
        //reTry function
        public delegate void DelgFunction();

        #region "private"

        //NO use
        private IWebElement FindElementByLinkText(string linkText)
        {
            IWebElement element = null;
            var webDriver = WebDriverHelper._webDriver;
            var wait = new WebDriverWait(webDriver, ConfigManager.Settings.SetFindElementWaitingTimeout);
            try
            {
                _log.Info("Find element by LinkText:" + linkText);
                RetryFunction(ConfigManager.Settings.RetryTimes, "", true, delegate
                {
                    element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.LinkText(linkText)));
                }
                );
                return element;
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " , for " + ConfigManager.Settings.RetryTimes + " times.", e);
                throw;
            }
        }

        private string GetAttribute(IWebElement element, string attributeName)
        {
            var elementAttributeValue = element.GetAttribute(attributeName).Trim();
            //remove spaces and newlines in a string 
            var value = Regex.Replace(elementAttributeValue, @"\t|\n|\r|\s", "");
            _log.Info($"Get Element attribute : {value}");
            return value;
        }

        private WebElementsService()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="retryTimes"></param>
        /// <param name="interval"></param>
        /// <param name="throwIfFail"></param>
        /// <param name="function"></param>
        private void RetryFunction(int retryTimes, string interval, bool throwIfFail, DelgFunction function)
        {
            var functionName = function.GetMethodInfo().Name;
            _log.Info("Run retry function: " + functionName);
            var sleepTime = String.Empty;
            for (int retryTimeCount = 0; retryTimeCount <= retryTimes; retryTimeCount++)
            {
                try
                {
                    function();
                    break;
                }
                catch (Exception e)
                {

                    if (retryTimeCount.Equals(retryTimes))
                    {
                        var gg = DateTime.Now.ToString("MMddHHmmss.ffff");
                        if (throwIfFail)
                        {
                            SnapshotServices.SnapShotProcess($"[{gg}]" + e.Message.Replace(".", "_"));
                            throw new Exception($"[{gg}]_IT_HAD_RETRY_{retryTimeCount}_SECONDS " + e.Message, e);
                        }
                        break;
                    }
                    if (interval.Equals(""))
                    {
                        //Default sleep time: 1.0s
                        var defaultSleepTime = ConfigManager.Settings.DefaultRetryTimeBySeconds;

                        sleepTime = defaultSleepTime.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _log.Info("Every " + interval + " seconds retry the function : " + function.GetMethodInfo().Name);
                        sleepTime = interval;
                    }

                    var delay = TimerService.SetTimeBySecond(sleepTime);
                    _log.Info("[System Setting] Round " + retryTimeCount + ", interval time WILL setup adds " + delay.ToString() + " delay each time on " + function.GetMethodInfo().Name);
                    Thread.Sleep(delay);
                }
            }
        }


        //Check jQuery Objects
        private void WaitUntilJQueryReady()
        {
            _log.Info("Start Waiting JQuery and JS Ready");
            var webDriver = WebDriverHelper._webDriver;
            IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;
            var script = "return typeof jQuery != 'undefined'";
            var errorMessage = "jQuery is not defined on this page!!!!";
            var undefinedCount = 0;
            var sleepTime = TimerService.SetTimeBySecond("1");
            while (true)
            {
                if (js == null)
                {
                    var ex = new ArgumentException("Element", "The element must wrap a web driver that supports javascript execution.");
                    _log.Error(ex.Message);
                    throw ex;
                }

                var result = js.ExecuteScript(script);
                var jQueryDefined = (bool)result;
                if (jQueryDefined)
                {
                    break;
                }
                else
                {
                    _log.Error($"THE JS IS {result.ToString()}");
                    _log.Error($"{errorMessage} after 1 second, system will confirm jquery again.");
                    _log.Error($"THIS IS PAGE SOURCE ========================= {webDriver.PageSource}");
                    undefinedCount++;
                    Thread.Sleep(sleepTime);
                    _log.Error($"Refresh Page");
                    webDriver.Navigate().Refresh();
                }
            }
            _log.Info($"JQueryUndefined :{undefinedCount} ....................");
        }

        //WaitForJQueryLoad
        private void WaitForJQueryLoad()
        {
            _log.Info("Start Waiting For JQuery");
            var webDriver = WebDriverHelper._webDriver;
            var timeCount = 0;
            IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;
            var script = "return jQuery.active == 0";
            var sleepTime = TimerService.SetTimeBySecond("1");
            while (true)
            {
                if (js == null)
                {
                    var ex = new ArgumentException("Element", "The element must wrap a web driver that supports javascript execution.");
                    _log.Error(ex.Message);
                    throw ex;
                }

                var jQueryLoadIsComplete = (bool)js.ExecuteScript(script);
                if (jQueryLoadIsComplete)
                    break;
                //1 sec
                _log.Error($"THIS IS PAGE SOURCE ========================= {webDriver.PageSource}");
                Thread.Sleep(sleepTime);
                timeCount++;
            }
            _log.Info($"End waiting for JQuery Load, this process waits for : {timeCount} seconds.");
        }



        //Wait for Angular Load
        private void WaitForAngularLoad()
        {
            _log.Info("Start Waiting For Angular Load");
            var webDriver = WebDriverHelper._webDriver;
            var timeCount = 0;
            IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;
            var sleepTime = TimerService.SetTimeBySecond("1");
            var script = "return angular.element(document).injector().get('$http').pendingRequests.length === 0";
            while (true)
            {
                if (js == null)
                {
                    var ex = new ArgumentException("Element", "The element must wrap a web driver that supports javascript execution.");
                    _log.Error(ex.Message);
                    throw ex;
                }

                var angularLoadIsComplete = (bool)js.ExecuteScript(script);
                if (angularLoadIsComplete)
                    break;
                //1 sec
                Thread.Sleep((TimeSpan)sleepTime);
                timeCount++;
            }
            _log.Info($"End waiting for Angular Load, this process waits for : {timeCount} seconds.");

        }

        #endregion "private"


        public static WebElementsService Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string RemoveTag(string text)
        {
            _log.Info("Remove tag");
            return text.Trim(new Char[] { '#', '.' });
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="elementObject"></param>
        /// <returns></returns>
        public IWebElement CleanOriginText(IWebElement elementObject)
        {
            var removeOriginText = elementObject;
            removeOriginText.Clear();
            return removeOriginText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string CheckElementTagType(string tag)
        {
            //check elementTag;
            // Css Selector
            // # => Id
            // . => Css class
            // other => Xpath
            if (tag.Equals(TagConstants.CssSelectorTag, StringComparison.CurrentCultureIgnoreCase))
            {
                return TagConstants.CssSelectorTag;
            }
            else if (tag.Equals(TagConstants.IdTag, StringComparison.CurrentCultureIgnoreCase))
            {
                return TagConstants.IdTag;
            }
            else if (tag.Equals(TagConstants.CssClassTag, StringComparison.CurrentCultureIgnoreCase))
            {
                return TagConstants.CssClassTag;
            }
            else
            {
                return TagConstants.XpathTag;
            }

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IWebElement FindElementById(string id)
        {
            IWebElement element = null;
            var webDriver = WebDriverHelper._webDriver;
            var wait = new WebDriverWait(webDriver, ConfigManager.Settings.SetFindElementWaitingTimeout);

            try
            {
                _log.Info("Find element by Id:" + id);
                RetryFunction(ConfigManager.Settings.RetryTimes, "", true, delegate
                {
                    element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(RemoveTag(id))));
                }
                );
                return element;
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " , for " + ConfigManager.Settings.RetryTimes + " times.", e);
                throw;
            }
        }

        /// <summary>
        /// 如果有Element的 Display, Visable or enable 會被設定為 false，一律用Class Name來找，因為這個function，只要Element有存在於DOM檔就可以抓的到！
        /// </summary>
        /// <param name="cssName"></param>
        /// <returns></returns>
        public IWebElement FindElementByCss(string cssName)
        {
            IWebElement element = null;
            var webDriver = WebDriverHelper._webDriver;
            var wait = new WebDriverWait(webDriver, ConfigManager.Settings.SetFindElementWaitingTimeout);
            try
            {
                _log.Info("Find element by css:" + cssName);
                RetryFunction(ConfigManager.Settings.RetryTimes, "", true, delegate
                {
                    element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.ClassName(cssName)));
                });
                return element;
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " , for " + ConfigManager.Settings.RetryTimes + " times.", e);
                throw;
            }
        }

        public IWebElement FindElementByTagName(string tagName)
        {
            IWebElement element = null;
            var webDriver = WebDriverHelper._webDriver;
            var wait = new WebDriverWait(webDriver, ConfigManager.Settings.SetFindElementWaitingTimeout);
            try
            {
                _log.Info($"Find element by Selector:{tagName}");
                RetryFunction(ConfigManager.Settings.RetryTimes, "", true, delegate { element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.TagName(tagName))); }
                );
                return element;
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " , for " + ConfigManager.Settings.RetryTimes + " times.", e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CssSelector"></param>
        /// <returns></returns>
        public IWebElement FindElementByCssSelector(string CssSelector)
        {
            IWebElement element = null;
            var webDriver = WebDriverHelper._webDriver;
            var wait = new WebDriverWait(webDriver, ConfigManager.Settings.SetFindElementWaitingTimeout);
            try
            {
                _log.Info($"Find element by Selector:{CssSelector}");
                RetryFunction(ConfigManager.Settings.RetryTimes, "", true, delegate
                {
                    element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(CssSelector)));
                }
                );
                return element;
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " , for " + ConfigManager.Settings.RetryTimes + " times.", e);
                throw;
            }
        }

        public IWebElement FindElementByCssSelectorByFluentWait(string cssSelector, bool isEnableExtension, int extensionTime = 60)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(WebDriverHelper._webDriver)
            {
                Timeout =
                    ConfigManager.Settings.SetFindElementWaitingTimeout.Add(TimeSpan.FromSeconds(extensionTime)),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };

            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            IWebElement searchResult = fluentWait.Until(x => x.FindElement(By.CssSelector(cssSelector)));

            return searchResult;
        }

        /// <summary>
        /// find and wait elements by CSS Selector string
        /// </summary>
        /// <param name="CssSelectors"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string CssSelectors)
        {
            _log.Info($"Find elementS by Selector:{CssSelectors}");
            var webDriver = WebDriverHelper._webDriver;
            var wait = new WebDriverWait(webDriver, ConfigManager.Settings.SetFindElementWaitingTimeout);
            //var elementList = wait.Until(PresenceOfAllElementsLocatedBy(By.CssSelector(CssSelectors)));
            var elementList = webDriver.FindElements(By.CssSelector(CssSelectors));
            return elementList;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IWebElement FindElementByXpath(string xpath)
        {
            IWebElement element = null;
            var webDriver = WebDriverHelper._webDriver;
            var wait = new WebDriverWait(webDriver, ConfigManager.Settings.SetFindElementWaitingTimeout);
            try
            {
                _log.Info($"Find element by Xpath: {xpath}");
                RetryFunction(ConfigManager.Settings.RetryTimes, "", true, delegate
                {
                    element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
                }
                );
                return element;
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " , for " + ConfigManager.Settings.RetryTimes + " times.", e);
                throw;
            }
        }
        
      public string GetElementAttributes(IWebElement element, string attributeName)
        {
            return GetAttribute(element, attributeName);
        }
        public string GetElementCssValue(IWebElement element, string cssParameter)
        {
            var elementCssValue = element.GetCssValue($"{cssParameter}");
            //remove spaces and newlines in a string 
            var cssValue = Regex.Replace(elementCssValue, @"\t|\n|\r|\s", "");
            _log.Info($"Get Element Css Value: {cssValue}");
            return cssValue;
        }

        //Wait Until JavaScript loading Completed.
        public void WaitUntilJSReady()
        {
            _log.Info("Start Waiting Until JS Ready");
            var webDriver = WebDriverHelper._webDriver;
            var timeCount = 0;
            IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;
            var sleepTime = TimerService.SetTimeBySecond("1");
            var script = "return document.readyState";
            var state = "complete";
            while (true)
            {
                if (js == null)
                {
                    var ex = new ArgumentException("Element", "The element must wrap a web driver that supports javascript execution.");
                    _log.Error(ex.Message);
                    throw ex;
                }

                var untilJsReady = js.ExecuteScript(script).ToString().Equals(state);
                if (untilJsReady)
                {
                    //Wait JQuery Ready
                    WaitUntilJQueryReady();
                    //Wait JQuery Load
                    WaitForJQueryLoad();
                    break;
                }
                else
                {
                    //1 sec
                    _log.Error($"THIS IS PAGE SOURCE ========================= {webDriver.PageSource}");
                    Thread.Sleep(sleepTime);
                }
            }

            _log.Info($"End waiting until JS ready, this process document.ready spends: {timeCount} .");

        }

        //Reference: https://www.swtestacademy.com/selenium-wait-javascript-angular-ajax/
        //Wait Until Angular and JS Ready
        public void WaitUntilAngularReady()
        {
            _log.Info("Start Wait Until Angular Ready");
            var webDriver = WebDriverHelper._webDriver;
            IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;
            var confirmAngularScript = "return window.angular === undefined";
            var angularUnDefinedErrorMessage = "Angular is not defined on this site!";
            var angularInjectorScript = "return angular.element(document).injector() === undefined";
            var angularInjectorErrorMessage = "Angular injector is not defined on this site!";
            var angularUnDefinedCount = 0;
            var angularInjectorCount = 0;
            while (true)
            {

                if (js == null)
                {
                    var ex = new ArgumentException("Element", "The element must wrap a web driver that supports javascript execution.");
                    _log.Error(ex.Message);
                    throw ex;
                }

                var angularUnDefined = (bool)js.ExecuteScript(confirmAngularScript);
                if (!angularUnDefined)
                {
                    var angularInjectorUnDefined = (bool)js.ExecuteScript(angularInjectorScript);
                    if (angularInjectorUnDefined)
                    {
                        //Wait JS Load
                        WaitUntilJSReady();
                        //Wait Angular Load
                        WaitForAngularLoad();
                        break;
                    }
                    else
                    {
                        angularInjectorCount++;
                        _log.Error(angularInjectorErrorMessage);

                    }
                }
                else
                {
                    angularUnDefinedCount++;
                    _log.Error(angularUnDefinedErrorMessage);
                }
            }

            _log.Info($"This Process, Angular Injector Time: {angularInjectorCount}. Angular UnDefined Time: {angularUnDefinedCount}.");
        }

        /// <summary>
        ///scrollBy(X,Y);unit:pixel
        /// </summary>
        /// <param name="x" ></param>
        /// <param name="y"></param>
        public void ScrollTheViewToLeftistByJs(int x, int y)
        {
            _log.Info("Start Scroll The View By JavaScript");
            var webDriver = WebDriverHelper._webDriver;
            IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;

            if (js.Equals(null))
            {
                var ex = new ArgumentException("Element", "The element must wrap a web driver that supports javascript execution.");
                _log.Error(ex.Message);
                throw ex;
            }

            //js.ExecuteScript("window.scrollBy(-100000,0)");
            js.ExecuteScript($"window.scrollBy({x},{y})");
        }

        public void SetAttributeByJavaScript(IWebElement element, string attributeName, string value)
        {
            try
            {
                var webDriver = WebDriverHelper._webDriver;
                IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;
                if (js.Equals(null))
                    throw new ArgumentException("Element", "The element must wrap a web driver that supports javascript execution.");

                js.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2])", element, attributeName, value);
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
        }
    }
}
