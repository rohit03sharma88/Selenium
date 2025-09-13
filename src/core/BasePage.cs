
// BasePage.cs
// This abstract class provides common functionality for all page objects in the Selenium test framework.
// It implements navigation, element interaction, and waiting logic for derived page classes.

using OpenQA.Selenium; // Selenium WebDriver namespace for browser automation
using OpenQA.Selenium.Support.UI; // For waiting and UI interactions
using SeleniumExtras.PageObjects; // For PageFactory and page object support
using SeleniumExtras.WaitHelpers; // For expected conditions
using OpenQA.Selenium.Support; // Additional Selenium support
using System; // Provides basic system functions

// BasePage implements the IPage interface and provides shared logic for all pages
public abstract class BasePage: IPage
{
    // The WebDriver instance used to interact with the browser
    public IWebDriver Driver {get;}
    // The relative URL for the page (must be defined in derived classes)
    public abstract string PageUrl {get;}
    // WebDriverWait instance for waiting on elements
    private WebDriverWait _wait;
    // Abstract method to check if the page is loaded (must be implemented by derived classes)
    public abstract bool IsAt();

    // Constructor: initializes the page with a WebDriver instance and sets up the wait
    public BasePage(IWebDriver driver)
    {
        Driver = driver;
        _wait  = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
    }

    // Navigates to the page's URL if it is not empty
    public virtual void Navigate()
    {
        if(!String.IsNullOrWhiteSpace(PageUrl))
            Driver.Navigate().GoToUrl(PageUrl);
    }

    // Waits for an element to be visible on the page
    // Parameters:
    //   locator - the By locator for the element
    //   timeout - the maximum time to wait in seconds
    protected IWebElement WaitForElementVisible(By locator, int timeout=10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
        return wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    // Clicks an element after waiting for it to be visible
    public void ClickElement(By locator)
    {
        var element = WaitForElementVisible(locator);
        element.Click();
    }

    // Enters text into an input field after waiting for it to be visible
    public void EnterText(By locator, string text)
    {
        var element = WaitForElementVisible(locator);
        element.Clear();
        element.SendKeys(text);
    }

    // Gets the text content of an element after waiting for it to be visible
    public string GetElementText(By locator)
    {
        return WaitForElementVisible(locator).Text;
    }

    // Checks if an element is present and visible on the page
    public bool IsElementPresent(By locator)
    {
        try
        {
            return WaitForElementVisible(locator, 5).Displayed;
        }
        catch
        {
            // If the element is not found or not visible, return false
            return false;
        }
    }
}