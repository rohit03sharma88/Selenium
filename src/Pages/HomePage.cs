using System;
using OpenQA.Selenium;

namespace SeleniumTests.src.Pages;

public class HomePage : BasePage
{
    public override string PageUrl => "/parabank/overview.htm";

    private readonly By AccountsOverviewLink = By.LinkText("Accounts Overview");
    private readonly By LogOutLink = By.LinkText("Log Out");

    public HomePage(IWebDriver driver) : base (driver) {}
    

    public void GotoAccountsOverview()
    {
        ClickElement(AccountsOverviewLink);
    }

    public void LogOut()
    {
        ClickElement(LogOutLink);
    }

    public override bool IsAt()
    {
        return IsElementPresent(AccountsOverviewLink);
    }
}
