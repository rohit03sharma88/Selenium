using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.src.Pages;

public class LogInPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    
    public LogInPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    private IWebElement UserNameInput => _driver.FindElement(By.Name("username"));
    private IWebElement PasswordInput => _driver.FindElement(By.Name("password"));
    private IWebElement LoginButton => _driver.FindElement(By.XPath("//input[@value='Log In']"));

    public void LogIn(string username, string password)
    {
        UserNameInput.Clear();
        UserNameInput.SendKeys(username);
        PasswordInput.Clear();
        PasswordInput.SendKeys(password);
        LoginButton.Click();
    }

    public void NavigateTo(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }

    public bool IsLoginErrorDisplayed()
    {
        try
        {
            var errorElement = _driver.FindElement(By.CssSelector(".error"));
            return errorElement.Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
}
