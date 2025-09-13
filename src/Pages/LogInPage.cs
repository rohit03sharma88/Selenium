
// LogInPage.cs
// This class represents the login page of the Parabank application.
// It provides methods to interact with the login form and verify login errors.

using System; // Provides basic system functions
using OpenQA.Selenium; // Selenium WebDriver namespace for browser automation
using OpenQA.Selenium.Support.UI; // For waiting and UI interactions

// Namespace for all Page classes in the SeleniumTests project
namespace SeleniumTests.src.Pages;

// LogInPage inherits from BasePage, which provides common page functionality
public class LogInPage : BasePage
{
    // The relative URL for the login page
    public override string PageUrl => "/parabank/index.htm";

    // Constructor: initializes the page with a WebDriver instance
    public LogInPage(IWebDriver driver) : base(driver){}

    // Locator for the username input field
    private readonly By UserNameInput = By.Name("userName");
    // Locator for the password input field
    private readonly By PasswordInput = By.Name("password");
    // Locator for the login button
    private readonly By LoginButton = By.XPath("//input[@value='Log In']");
    // Locator for the error message displayed on failed login
    private readonly By ErrorMessage = By.CssSelector(".error");

    // LogIn method: enters the username and password, then clicks the login button
    // Parameters:
    //   userName - the username to enter
    //   password - the password to enter
    public void LogIn(string userName, string password)
    {
        // Enter the username in the username input field
        EnterText(UserNameInput, userName);
        // Enter the password in the password input field
        EnterText(PasswordInput, password);
        // Click the login button to submit the form
        ClickElement(LoginButton);
    }

    // Checks if the login error message is displayed
    // Returns true if the error message is present, false otherwise
    public bool IsLoginErrorDisplayed()
    {
        return IsElementPresent(ErrorMessage);
    }

    // Checks if the current page is the login page by verifying the presence of the login button
    public override bool IsAt()
    {
        return IsElementPresent(LoginButton);
    }
}
