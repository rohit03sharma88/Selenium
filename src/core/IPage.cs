using OpenQA.Selenium;

interface IPage
{
    IWebDriver Driver {get;}
    string PageUrl {get;}
    void Navigate();
    bool IsAt();
}