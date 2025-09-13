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
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");

            var driver = new ChromeDriver(options);
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
