
// IPage.cs
// This interface defines the contract for all page objects in the Selenium test framework.
// It ensures that each page object provides navigation and verification methods.

using OpenQA.Selenium; // Selenium WebDriver namespace for browser automation

// IPage interface defines required members for page objects
interface IPage
{
    // The WebDriver instance used to interact with the browser
    IWebDriver Driver {get;}
    // The relative URL for the page
    string PageUrl {get;}
    // Navigates to the page's URL
    void Navigate();
    // Checks if the page is loaded and ready
    bool IsAt();
}