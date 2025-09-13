
// DriverFactory.cs
// This static class manages the creation and lifecycle of the Selenium WebDriver instance.
// It provides methods to create, access, and quit the WebDriver for browser automation.

using System; // Provides basic system functions
using OpenQA.Selenium; // Selenium WebDriver namespace for browser automation
using OpenQA.Selenium.Chrome; // For Chrome browser automation
using NUnit.Framework; // NUnit test framework

namespace SeleniumTests.src.core
{
    // DriverFactory provides static methods to manage the WebDriver instance
    public static class DriverFactory
    {
        // Thread-static variable to hold the WebDriver instance for each test thread
        [ThreadStatic]
        private static IWebDriver? _driver;

        // Property to get the current WebDriver instance, creating it if necessary
        public static IWebDriver Driver
        {
            get
            {
                // If the driver is not initialized, create a new one
                if (_driver == null)
                {
                    _driver = CreateDriver();
                }
                return _driver;
            }
        }

        // Creates a new Chrome WebDriver instance with custom options
        public static IWebDriver CreateDriver()
        {
            var options = new ChromeOptions();
            // Start the browser maximized
            options.AddArgument("--start-maximized");
            // Disable GPU for compatibility
            options.AddArgument("--disable-gpu");
            // Disable sandbox for compatibility
            options.AddArgument("--no-sandbox");

            // Create the ChromeDriver with the specified options
            var driver = new ChromeDriver(options);
            // Set implicit wait timeout for element searches
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        // Quits the current WebDriver instance and cleans up resources
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
                // Ignore exceptions during driver quit
            }
        }
    }
}
