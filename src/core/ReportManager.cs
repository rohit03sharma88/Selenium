
// ReportManager.cs
// This static class manages the ExtentReports reporting for the Selenium test framework.
// It provides methods to initialize, log, and flush test reports for each test run.

using AventStack.ExtentReports; // ExtentReports library for reporting
using AventStack.ExtentReports.Reporter; // For HTML report generation
using System; // Provides basic system functions
using System.IO; // For file and directory operations

// ReportManager provides static methods to manage test reporting
public static class ReportManager
{
    // The ExtentReports instance used for reporting
    private static ExtentReports _extent;
    // Thread-static variable to hold the current test report for each test thread
    [ThreadStatic] private static ExtentTest _test;

    // Initializes the report and sets up the HTML reporter
    public static void InitReport()
    {
        // Create the Reports directory if it doesn't exist
        var reportDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
        Directory.CreateDirectory(reportDir);

        // Create a new HTML reporter with a unique filename
        var htmlReporter = new ExtentSparkReporter(Path.Combine(reportDir, $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html"));
        _extent = new ExtentReports();
        _extent.AttachReporter(htmlReporter);
    }

    // Flushes the report to disk
    public static void FlushReport() => _extent?.Flush();

    // Creates a new test entry in the report
    public static void CreateTest(string testName) => _test = _extent?.CreateTest(testName);

    // Logs an informational message to the current test
    public static void LogInfo(string message) => _test?.Info(message);
    // Logs a pass message to the current test
    public static void LogPass(string message) => _test?.Pass(message);
    // Logs a fail message to the current test
    public static void LogFail(string message) => _test?.Fail(message);
}