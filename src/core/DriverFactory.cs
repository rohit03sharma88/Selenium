using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

namespace SeleniumTests.src.core
{

    public static class DriverFactory
    {
        [ThreadStatic]
        private static IWebDriver? _driver;


        public static IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    _driver = CreateDriver();
                }
                return _driver;
            }
        }

        public static IWebDriver CreateDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            var driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(120));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        public static void QuitDriver()
        {
            try
            {
                if (_driver != null)
                {
                    _driver.Quit();
                    _driver = null;
                }
            }
            catch
            {
            }
        }
    }
}
