
// HomePage.cs
// This class represents the home page after a successful login in the Parabank application.
// It provides methods to navigate to the accounts overview and to log out.

using System; // Provides basic system functions
using OpenQA.Selenium; // Selenium WebDriver namespace for browser automation

// Namespace for all Page classes in the SeleniumTests project
namespace SeleniumTests.src.Pages;

// HomePage inherits from BasePage, which provides common page functionality
public class HomePage : BasePage
{
    // The relative URL for the home page
    public override string PageUrl => "/parabank/overview.htm";

    // Locator for the 'Accounts Overview' link
    private readonly By AccountsOverviewLink = By.LinkText("Accounts Overview");
    // Locator for the 'Log Out' link
    private readonly By LogOutLink = By.LinkText("Log Out");

    // Constructor: initializes the page with a WebDriver instance
    public HomePage(IWebDriver driver) : base (driver) {}

    // Navigates to the accounts overview page by clicking the link
    public void GotoAccountsOverview()
    {
        ClickElement(AccountsOverviewLink);
    }

    // Logs out the current user by clicking the log out link
    public void LogOut()
    {
        ClickElement(LogOutLink);
    }

    // Checks if the current page is the home page by verifying the presence of the accounts overview link
    public override bool IsAt()
    {
        return IsElementPresent(AccountsOverviewLink);
    }
}
