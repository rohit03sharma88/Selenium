using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
namespace SeleniumTests.src.core;

public class BaseTest
{
    protected IWebDriver Driver => DriverFactory.Driver;
    protected string? BaseUrl { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var configPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "appsettings.json");
        if(File.Exists(configPath))
        {
            var json = File.ReadAllText(configPath);
            dynamic config = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            BaseUrl = config.baseUrl ?? "https://parabank.parasoft.com";
        }
        else
        {
            BaseUrl = "https://parabank.parasoft.com";
        }
    }


    [TearDown]
    public void TearDown(){
        if(TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            try{
                //var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                ITakesScreenshot ts = (ITakesScreenshot)Driver;
                Screenshot screenshot = ts.GetScreenshot();

                var screenshotsDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots");
                if(!Directory.Exists(screenshotsDir))
                {
                    Directory.CreateDirectory(screenshotsDir);
                }
                var screenshotPath = Path.Combine(screenshotsDir, $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(screenshotPath);
                TestContext.AddTestAttachment(screenshotPath);
            }
            catch{}

            DriverFactory.QuitDriver();
            
        }
    }
}
