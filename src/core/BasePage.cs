using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support;
using System;

public abstract class BasePage: IPage
{
    public IWebDriver Driver {get;}
    public  abstract string PageUrl {get;}
    private WebDriverWait _wait;
     public abstract bool IsAt();

    public BasePage(IWebDriver driver)
    {
        Driver = driver;
        _wait  = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
    }

    public virtual void Navigate()
    {
        if(!String.IsNullOrWhiteSpace(PageUrl))
            Driver.Navigate().GoToUrl(PageUrl);
    }

    protected IWebElement WaitForElementVisible(By locator, int timeout=10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
        return wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    public void ClickElement(By locator)
    {
        var element = WaitForElementVisible(locator);
        element.Click();
    }

    public void EnterText(By locator, string text)
    {
        var element = WaitForElementVisible(locator);
        element.Clear();
        element.SendKeys(text);
    }

    public string GetElementText(By locator)
    {
        return WaitForElementVisible(locator).Text;
    }

    public bool IsElementPresent(By locator)
    {
        try
        {
            return WaitForElementVisible(locator, 5).Displayed;
        }
        catch
        {
            
            return false;
        }
    }
   
}