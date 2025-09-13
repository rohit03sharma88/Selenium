
// AccountsOverviewPage.cs
// This class represents the accounts overview page in the Parabank application.
// It provides methods to retrieve account numbers and check for the presence of a specific account.

using System; // Provides basic system functions
using OpenQA.Selenium; // Selenium WebDriver namespace for browser automation
using System.Collections; // For collection types
using System.Linq; // For LINQ queries

// Namespace for all Page classes in the SeleniumTests project
namespace SeleniumTests.src.Pages;

// AccountsOverviewPage inherits from BasePage, which provides common page functionality
public class AccountsOverviewPage : BasePage
{
    // The relative URL for the accounts overview page
    public override string PageUrl => "/parabank/overview.htm";

    // Constructor: initializes the page with a WebDriver instance
    public AccountsOverviewPage(IWebDriver driver) : base(driver){}

    // Locator for the rows in the accounts table
    public readonly By AccountsTable = By.CssSelector("#accountTable tbody tr");

    // Retrieves a list of account numbers from the accounts table
    public List<string> GetAccountNumbers()
    {
        // Find all rows in the accounts table
        var rows = Driver.FindElements(AccountsTable);
        // For each row, get the text of the account number link and trim whitespace
        return rows.Select(r => r.FindElement(By.CssSelector("td a")).Text.Trim()).ToList();
    }

    // Checks if a specific account number exists in the accounts table
    public bool HasAccount(string accountNumber)
    {
        return GetAccountNumbers().Contains(accountNumber);
    }

    // Checks if the current page is the accounts overview page by verifying the presence of any account rows
    public override bool IsAt()
    {
        return Driver.FindElements(AccountsTable).Any();
    }
}
