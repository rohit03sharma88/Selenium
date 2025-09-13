
// PageFactory.cs
// This static class provides a factory method to create page objects with a WebDriver instance.
// It uses reflection to find and invoke the appropriate constructor for each page class.

using System; // Provides basic system functions
using OpenQA.Selenium; // Selenium WebDriver namespace for browser automation

namespace SeleniumTests.src.core;

// PageFactory provides a generic method to create page objects
public static class PageFactory
{   
    // Creates an instance of the specified page type with the given WebDriver
    // Parameters:
    //   driver - the WebDriver instance
    // Returns an instance of type T (page object)
    public static T Create<T>(IWebDriver driver) where T : class
    {
        // Find the constructor that takes a WebDriver parameter
        var ctor = typeof(T).GetConstructor(new[] { typeof(IWebDriver) });
        if (ctor != null)
            // Invoke the constructor with the WebDriver
            return  ctor.Invoke(new object[] { driver }) as T;

        // If no matching constructor, create a default instance
        return Activator.CreateInstance<T>();
    }
}
