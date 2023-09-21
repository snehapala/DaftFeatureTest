using Daft.Feature.Test.Drivers;
using Daft.Feature.Test.TestUtilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Daft.Feature.Test.Steps
{
    [Binding]
    public sealed class KeywordFilterTest
    {

        #region Private Fields
        IWebDriver driver;
        string _filter = "";
        private readonly ScenarioContext _scenarioContext;
        #endregion

        #region Constructor Method
        public KeywordFilterTest(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        #endregion

        #region Feature Definition Methods

        [Given(@"I have navigated to the Daft website")]
        public void GivenIHaveNavigatedToTheDaftWebsite()

        {
            driver = _scenarioContext.Get<SeleniumDriver>("SeleniumDriver").Setup();
            driver.Url = "https://www.daft.ie/";
            driver.FindElement(By.XPath("//button[@onclick='CookieConsent.acceptAll();']")).Click();
        }

        [Given(@"search for a saleAd in dublin county")]
        public void GivenSearchForASaleAdInDublinCounty()
        {
            #region Search for Sale Ad in Dublin County
            if (TestUtilitiesCode.WaitForPageLoad(driver))
            {
                try
                {
                    driver.FindElement(By.XPath("//li[contains(text(),'Buy')]")).Click();
                    driver.FindElement(By.XPath("//input[@id='search-box-input']")).Click();
                    driver.FindElement(By.XPath("//span[contains(text(),'Dublin')]")).Click();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured while searching for county: " + ex);
                }
            }
            else
            {
                Console.WriteLine("Page did not load within the specified timeout.");
            }
            #endregion
        }

        [Given(@"there are search results for the county")]
        public void GivenThereAreSearchResultsForTheCounty()
        {
            #region Wait till page is loaded
            if (TestUtilitiesCode.WaitForPageLoad(driver))
            {
                #region Check If there are any results for that county

                string xpathExpression = "//li";
                // Find all elements matching the XPath expression
                IReadOnlyCollection<IWebElement> liElements = driver.FindElements(By.XPath(xpathExpression));
                // Check if any <li> elements were found
                if (liElements.Count > 0)
                {
                    Console.WriteLine("There are " + liElements.Count + " <li> elements on the page.");
                }
                else
                {
                    Console.WriteLine("No <li> elements found on the page.");
                }

                #endregion
            }
            else
            {
                Console.WriteLine("Page did not load within the specified timeout.");
            }
            #endregion
        }

        [When(@"I apply keyword filter '(.*)'")]
        public void WhenIApplyKeywordFilter(string filter)
        {
            #region Click on Filters Option

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            By elementLocator = By.XPath("//button[@aria-label='Filters']");
            try
            {
                // Wait for the element to be clickable
                IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));

                // Once the element is clickable, you can perform actions on it
                element.Click();
            }
            catch (WebDriverTimeoutException ex)
            {
                // Handle timeout exception if the element is not clickable within the specified time
                Console.WriteLine("Element was not clickable within the specified time. " + ex);
            }

            #endregion

            #region Apply garage as keywordfilter

            By keywordEleLocator = By.XPath("//input[@id='keywordtermsModal']");
            try
            {
                _filter = filter;
                //wait till the element is visible
                IWebElement keywordElement = wait.Until(ExpectedConditions.ElementIsVisible(keywordEleLocator));

                // Now you can interact with the element
                keywordElement.Click();
                keywordElement.SendKeys(_filter);
            }
            catch (NoSuchElementException ex)
            {
                // Handle the case where the element is not found
                Console.WriteLine("Element not found on the page. " + ex);
            }

            #endregion
        }

        [When(@"there are results for that filter")]
        public void WhenThereAreResultsForThatFilter()
        {
            #region Click on the Show Results button after applying the keyword filter

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            By resultsEleLocator = By.XPath("//button[@class='NewButton__StyledButton-yem86a-2 klRAtd']");

            try
            {
                // Wait for the element to be clickable
                IWebElement resultsElement = wait.Until(ExpectedConditions.ElementIsVisible(resultsEleLocator));

                // Once the element is clickable, we click the 'Show Results' Button
                resultsElement.Click();
            }
            catch (WebDriverTimeoutException ex)
            {
                // Handle timeout exception if the element is not clickable within the specified time
                Console.WriteLine("Element was not clickable within the specified time.");
            }

            #endregion
        }

        [Then(@"open one search result and verify garage keyword is there on that advert")]
        public void ThenOpenOneSearchResultAndVerifyGarageKeywordIsThereOnThatAdvert()
        {

            #region Results after applying keyword filter

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Use the ExpectedConditions.UrlContains to check if the URL contains the garage
            wait.Until(ExpectedConditions.UrlContains(_filter));

            if (driver.Url.Contains(_filter))
            {
                #region Check If there are any results for that county

                string xpathExpression = "//li";

                // Find all elements matching the XPath expression
                IReadOnlyCollection<IWebElement> liElements = driver.FindElements(By.XPath(xpathExpression));

                // Check if any <li> elements were found
                if (liElements.Count > 0)
                {
                    Console.WriteLine("There are " + liElements.Count + " <li> elements on the page.");

                    #region Open any of the Search result page

                    By searchResultLoc = By.XPath("//li[@class='SearchPage__Result-gg133s-2 djuMQD']");
                    try
                    {
                        //Thread.Sleep(10000);
                        IWebElement aLinkElement = wait.Until(ExpectedConditions.ElementIsVisible(searchResultLoc));
                        aLinkElement.Click();
                    }

                    catch (WebDriverTimeoutException ex)
                    {
                        // Handle timeout exception if the element is not visible within the specified time
                        Console.WriteLine("Element was not visible within the specified time.");
                    }

                    #endregion
                }
                else
                {
                    Console.WriteLine("No <li> elements found on the page.");
                }

                #endregion
            }

            #endregion

            #region Check If the search page has garage listed as property feature

            #region Checking If the page is loaded by verifying one of the elements on the advert

            bool isTrue = wait.Until(driver =>
            {
                IWebElement element = driver.FindElement(By.XPath("//h2[contains(text(),'Description')]"));
                return element.Displayed;
            });

            #endregion

            if (isTrue)
            {
                string pageSource = driver.PageSource;
                Assert.IsTrue(pageSource.Contains(_filter), $"The string: {_filter} does not exist on the page.");
            }

            #endregion

        }

        #endregion

    }
}
