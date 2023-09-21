using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace Daft.Feature.Test.Drivers
{
    public class SeleniumDriver
    {
        #region Private Methods
        private IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;
        #endregion

        #region Constructor Method
        public SeleniumDriver(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;
        #endregion

        #region Driver Setup
        public IWebDriver Setup()
        {
            var chromeOptions = new ChromeOptions();
            driver = new ChromeDriver(chromeOptions);

            #region Set the driver
            _scenarioContext.Set(driver, "WebDriver");
            #endregion

            driver.Manage().Window.Maximize();
            return driver;
        }
        #endregion
    }
}
