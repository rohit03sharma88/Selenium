using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

public static class ReportManager
{
    private static ExtentReports _extent;
    [ThreadStatic] private static ExtentTest _test;

    public static void InitReport()
    {
        var reportDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
        Directory.CreateDirectory(reportDir);

        var htmlReporter = new ExtentSparkReporter(Path.Combine(reportDir, $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html"));
        _extent = new ExtentReports();
        _extent.AttachReporter(htmlReporter);
    }

    public static void FlushReport() => _extent?.Flush();

    public static void CreateTest(string testName) => _test = _extent?.CreateTest(testName);

    public static void LogInfo(string message) => _test?.Info(message);
    public static void LogPass(string message) => _test?.Pass(message);
    public static void LogFail(string message) => _test?.Fail(message);
}