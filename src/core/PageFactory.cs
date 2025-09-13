using System;
using OpenQA.Selenium;

namespace SeleniumTests.src.core;

public static class PageFactory
{   
    public static T Create<T>(IWebDriver driver) where T : class
    {
        var ctor = typeof(T).GetConstructor(new[] { typeof(IWebDriver) });
        if (ctor != null)
            return  ctor.Invoke(new object[] { driver }) as T;

        return Activator.CreateInstance<T>();
    }
    
}
