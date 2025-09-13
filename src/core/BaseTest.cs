
// BaseTest.cs
// This class provides the base setup and teardown logic for all test classes in the Selenium test framework.
// It manages the WebDriver instance, reporting, and test lifecycle events.

using System; // Provides basic system functions
using System.IO; // For file and directory operations
using AventStack.ExtentReports.Model; // For reporting
using NUnit.Framework; // NUnit test framework
using OpenQA.Selenium; // Selenium WebDriver namespace for browser automation
namespace SeleniumTests.src.core;

// BaseTest provides common setup and teardown logic for all tests
public class BaseTest
{
    // The WebDriver instance used to interact with the browser
    protected IWebDriver Driver => DriverFactory.Driver;
    // The base URL for the application under test
    protected string? BaseUrl { get; private set; }

    // One-time setup before any tests are run
    [OneTimeSetUp]
    public void GLobalSetup()
    {
        // Initialize the test report
        ReportManager.InitReport();
        // Set the base URL for the application
        BaseUrl = "https://parabank.parasoft.com";
    }

    // Setup before each test
    [SetUp]
    public void BeforeTest()
    {
        // Create a new test entry in the report
        ReportManager.CreateTest(TestContext.CurrentContext.Test.Name);
    }

    // Teardown after each test
    [TearDown]
    public void AfterTest()
    {
        // If the test failed, capture a screenshot and log the failure
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            try
            {
                // Take a screenshot of the browser
                ITakesScreenshot ts = (ITakesScreenshot)Driver;
                Screenshot screenshot = ts.GetScreenshot();

                // Create the screenshots directory if it doesn't exist
                var screenshotsDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots");
                if (!Directory.Exists(screenshotsDir))
                {
                    Directory.CreateDirectory(screenshotsDir);
                }
                // Save the screenshot with a unique name
                var screenshotPath = Path.Combine(screenshotsDir, $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(screenshotPath);

                // Log the failure in the report
                ReportManager.LogFail($"Test Failed. Screenshot: {screenshotPath}");
            }
            catch { }

        }
        else
        {
            // Log the test as passed in the report
            ReportManager.LogPass("Test Passed");
        }

        // Quit the WebDriver instance
        DriverFactory.QuitDriver();
    }

    // One-time teardown after all tests are run
    [OneTimeTearDown]
    public void GlobalTearDown()
    {
        // Flush the test report
        ReportManager.FlushReport();
    }
}
