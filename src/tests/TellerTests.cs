
// TellerTests.cs
// This class contains the main test cases for the Parabank application using Selenium and NUnit.
// It performs login and account verification for users read from an Excel file.

using System.IO; // For file and directory operations
using System.Linq; // For LINQ queries
using AventStack.ExtentReports.Model; // For reporting
using NUnit.Framework; // NUnit test framework
using SeleniumTests.src.core; // Core framework classes
using SeleniumTests.src.Pages; // Page object classes

namespace SeleniumTests.src.tests
{
    // Test fixture for teller tests, runs tests in parallel
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class TellerTests : BaseTest
    {
        // Path to the Excel file containing user data
        private string? excelPath;

        // Setup before each test: set the path to the Excel file
        [SetUp]
        public void SetUpTest()
        {
            excelPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "users.xlsx");
            excelPath = Path.GetFullPath(excelPath);            
        }

        // Test: login with the first user from Excel and verify their account
        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void LoginAndVerifyAccountFromExcel()
        {
            // Read users from Excel
            var users = ExcelReader.ReadUsers(excelPath);
            var user = users.First(); // Get the first user

            // Create and navigate to the login page
            var loginPage = new LogInPage(Driver);
            loginPage.Navigate();
            ReportManager.LogInfo($"Navigated to: {BaseUrl}");

            // Log in with the user's credentials
            loginPage.LogIn(user.UserId, user.Password);
            ReportManager.LogInfo($"Logged in with User: {user.UserId}");

            // Verify that we are at the home page
            var home = new HomePage(Driver);
            Assert.That(home.IsAt(), Is.True, "Failed to log in or not at Home Page.");
            ReportManager.LogPass("Successfully logged in and at Home Page.");
            home.GotoAccountsOverview();

            // Verify the user's account exists
            var accountsOverview = new AccountsOverviewPage(Driver);
            Assert.That(accountsOverview.HasAccount(user.AccountNumber), Is.True, "Account numbernot found");

            ReportManager.LogPass($"Account number {user.AccountNumber} found for the user {user.UserId}.");
        }

        // Data-driven test: login and verify accounts for each user from Excel
        [Test]
        [Parallelizable(ParallelScope.Self)]
        [TestCaseSource(nameof(GetUsersFromExcel))]
        public void DataDriven_LoginEachUsers(string userId, string password, string accountNumber)
        {
            // Create and navigate to the login page
            var loginPage = new LogInPage(Driver);
            loginPage.Navigate();
            ReportManager.LogInfo($"Navigated to: {BaseUrl}");

            // Log in with the user's credentials
            loginPage.LogIn(userId, password);
            ReportManager.LogInfo($"Logged in with User: {userId}");

            // Verify that we are at the home page
            var home = new HomePage(Driver);
            Assert.That(home.IsAt(), Is.True, "Failed to log in or not at Home Page.");
            ReportManager.LogPass("Successfully logged in and at Home Page.");

            home.GotoAccountsOverview();
            var accountsOverview = new AccountsOverviewPage(Driver);

            // Verify the user's account exists
            Assert.That(accountsOverview.HasAccount(accountNumber), Is.True, "Account number not found");
            ReportManager.LogPass($"Account number {accountNumber} found for the user {userId}.");
        }
        
        // Helper method: provides user data from Excel for data-driven tests
        private static System.Collections.IEnumerable GetUsersFromExcel()
        {
            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "users.xlsx");
            var users = ExcelReader.ReadUsers(path);
            Assert.That(users, Is.Not.Empty, "No users found in the Excel file.");

            // Yield a TestCaseData object for each user
            foreach (var user in users)
            {
                yield return new TestCaseData(user.UserId, user.Password, user.AccountNumber)
                    .SetName($"LoginTest_User_{user.UserId}");
            }
        }
    }
}