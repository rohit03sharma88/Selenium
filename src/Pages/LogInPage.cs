using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.src.Pages;

public class LogInPage : BasePage
{
   
    public override string PageUrl => "/parabank/index.htm";
    
    public LogInPage(IWebDriver driver) : base(driver){}

    private readonly By UserNameInput = By.Name("userName");
    private readonly By PasswordInput = By.Name("password");
    private readonly By LoginButton = By.XPath("//input[@value='Log In']");
    private readonly By ErrorMessage = By.CssSelector(".error");

    public void LogIn(string userName, string password)
    {
       EnterText(UserNameInput, userName);
       EnterText(PasswordInput, password);
       ClickElement(LoginButton);
    }

    public bool IsLoginErrorDisplayed()
    {
        return IsElementPresent(ErrorMessage);
    }

    public override bool IsAt()
    {
        return IsElementPresent(LoginButton);
    }
}
