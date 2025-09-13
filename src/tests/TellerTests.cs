using System.IO;
using System.Linq;
using AventStack.ExtentReports.Model;
using NUnit.Framework;
using SeleniumTests.src.core;
using SeleniumTests.src.Pages;

namespace SeleniumTests.src.tests
{

    [TestFixture]
    public class TellerTests : BaseTest
    {
        private string? excelPath;

        [SetUp]
        public void SetUpTest()
        {
            excelPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "users.xlsx");
            excelPath = Path.GetFullPath(excelPath);            
        }

        [Test]
        public void LoginAndVerifyAccountFromExcel()
        {
            var users = ExcelReader.ReadUsers(excelPath);            
            var user = users.First();

            var loginPage = new LogInPage(Driver);
            loginPage.Navigate();
            ReportManager.LogInfo($"Navigated to: {BaseUrl}");

            loginPage.LogIn(user.UserId, user.Password);
            ReportManager.LogInfo($"Logged in with User: {user.UserId}");

            var home = new HomePage(Driver);
            Assert.That(home.IsAt(), Is.True, "Failed to log in or not at Home Page.");
            ReportManager.LogPass("Successfully logged in and at Home Page.");
            home.GotoAccountsOverview();

            var accountsOverview = new AccountsOverviewPage(Driver);
            Assert.That(accountsOverview.HasAccount(user.AccountNumber), Is.True, "Account numbernot found");
            
            ReportManager.LogPass($"Account number {user.AccountNumber} found for the user {user.UserId}.");
        }

        [Test]
        public void DataDriven_LoginAllUsers()
        {
            var users = ExcelReader.ReadUsers(excelPath);
            Assert.That(users, Is.Not.Empty, "No users found in the Excel file.");

            foreach (var user in users)
            {
                var loginPage = new LogInPage(Driver);
                loginPage.Navigate();
                loginPage.LogIn(user.UserId, user.Password);

                var home = new HomePage(Driver);
                home.GotoAccountsOverview();

                var accountsOverview = new AccountsOverviewPage(Driver);
                Assert.That(accountsOverview.GetAccountNumbers().Count() > 0, Is.Not.Empty, $"No accounts {user.AccountNumber} found for the user {user.UserId}.");

                // Logout after each user test
                home.LogOut();

                DriverFactory.QuitDriver();
            }
        }
    }
}