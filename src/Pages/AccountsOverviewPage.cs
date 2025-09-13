using System;
using OpenQA.Selenium;
using System.Collections;
namespace SeleniumTests.src.Pages;

public class AccountsOverviewPage
{
    private readonly IWebDriver _driver;
    public AccountsOverviewPage(IWebDriver driver)
    {
        _driver = driver;
    }

    public List<string> GetAccountNumbers()
    {
        var accounts = new List<string>();
        var rows = _driver.FindElements(By.CssSelector("#accountTable tbody tr"));
        foreach (var row in rows)
        {
            var accountLink = row.FindElement(By.CssSelector("td a"));
            if(accountLink != null )
                accounts.Add(accountLink.Text);

        }
        return accounts;
    }

    public bool HasAccount(string accountNumber)
    {
        return GetAccountNumbers().Contains(accountNumber);
    }
}
