using System;
using OpenQA.Selenium;

namespace SeleniumTests.src.Pages;

public class HomePage
{
    private readonly IWebDriver _driver;
    public HomePage(IWebDriver driver)
    {
        _driver = driver;
    }

    private IWebElement AccountsOverviewLink => _driver.FindElement(By.LinkText("Accounts Overview"));
    private IWebElement LogOutLink => _driver.FindElement(By.LinkText("Log Out"));

    public void GotoAccountsOverview()
    {
        AccountsOverviewLink.Click();
    }

    public void LogOut()
    {
        LogOutLink.Click();
    }
}
