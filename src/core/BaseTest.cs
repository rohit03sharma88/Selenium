using System;
using System.IO;
using AventStack.ExtentReports.Model;
using NUnit.Framework;
using OpenQA.Selenium;
namespace SeleniumTests.src.core;

public class BaseTest
{
    protected IWebDriver Driver => DriverFactory.Driver;
    protected string? BaseUrl { get; private set; }

    [OneTimeSetUp]
    public void GLobalSetup()
    {
        ReportManager.InitReport();
        BaseUrl = "https://parabank.parasoft.com";
    }

    [SetUp]
    public void BeforeTest()
    {
        ReportManager.CreateTest(TestContext.CurrentContext.Test.Name);
    }

    [TearDown]
    public void AfterTest()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            try
            {
                //var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                ITakesScreenshot ts = (ITakesScreenshot)Driver;
                Screenshot screenshot = ts.GetScreenshot();

                var screenshotsDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots");
                if (!Directory.Exists(screenshotsDir))
                {
                    Directory.CreateDirectory(screenshotsDir);
                }
                var screenshotPath = Path.Combine(screenshotsDir, $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(screenshotPath);

                ReportManager.LogFail($"Test Failed. Screenshot: {screenshotPath}");
            }
            catch { }

        }
        else
        {
            ReportManager.LogPass("Test Passed");
        }

        DriverFactory.QuitDriver();
    }

    [OneTimeTearDown]
    public void GlobalTearDown()
    {
        ReportManager.FlushReport();
    }
}
