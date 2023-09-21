using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Daft.Feature.Test.TestUtilities
{
    public static class TestUtilitiesCode
    {
        #region Private Fields
        private static int Timeout = 40;
        #endregion

        #region Methods
        public static bool WaitForPageLoad(IWebDriver driver)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, Timeout));
                return wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
