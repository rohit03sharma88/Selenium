using System;
using OpenQA.Selenium;
using System.Collections;
using System.Linq;

namespace SeleniumTests.src.Pages;

public class AccountsOverviewPage : BasePage
{
    public override string PageUrl => "/parabank/overview.htm";

    public AccountsOverviewPage(IWebDriver driver) : base(driver){}
    public readonly By AccountsTable = By.CssSelector("#accountTable tbody tr");
    public List<string> GetAccountNumbers()
    {        
        var rows = Driver.FindElements(AccountsTable);        
        return rows.Select(r => r.FindElement(By.CssSelector("td a")).Text.Trim()).ToList();
    }

    public bool HasAccount(string accountNumber)
    {
        return GetAccountNumbers().Contains(accountNumber);
    }

    public override bool IsAt()
    {
        return Driver.FindElements(AccountsTable).Any();
    }
}
