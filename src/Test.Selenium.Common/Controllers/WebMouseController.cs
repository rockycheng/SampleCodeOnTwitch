﻿namespace Test.Selenium.Common.Controllers
{
    using System;
    using Automated.Selenium.SharedLibrary.WebElementsAPI;
    using log4net;
    using OpenQA.Selenium;

    public class WebMouseController
    {

        private static ILog _log = LogManager.GetLogger(typeof(WebMouseController));
        private static Lazy<WebMouseController> _instance = new Lazy<WebMouseController>(() => new WebMouseController());
        private WebMouseService WebMouseService => Automated.Selenium.SharedLibrary.WebElementsAPI.WebMouseService.Instance;

        #region "private"

        private WebMouseController()
        {
        }

        #endregion  "private"

        public static WebMouseController Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// Drag means source element.
        /// Drop means where is the target element
        /// </summary>
        /// <param name="drag"></param>
        /// <param name="drop"></param>
        public void MouseDragAndDrop(IWebElement drag, IWebElement drop)
        {
            WebMouseService.DragAndDrop(drag, drop);
        }

        public void MouseOverToElementAndClick(IWebElement element)
        {
            WebMouseService.MouseOverToElementAndClick(element);
        }

        public void MouseOverToElementAndDoubleClick(IWebElement element)
        {
            WebMouseService.MouseOverToElementAndDoubleClick(element);
        }

        public void MouseOverToElement(IWebElement element)
        {
            WebMouseService.MouseOverToElement(element);
        }

        public void ClickElement(IWebElement element)
        {
            WebMouseService.MouseClick(element);
        }


    }
}