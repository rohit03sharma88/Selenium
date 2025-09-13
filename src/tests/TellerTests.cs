using System.IO;
using System.Linq;
using AventStack.ExtentReports.Model;
using NUnit.Framework;
using SeleniumTests.src.core;
using SeleniumTests.src.Pages;

namespace SeleniumTests.src.tests
{

    [TestFixture, Parallelizable(ParallelScope.All)]
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
        [Parallelizable(ParallelScope.Self)]
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
        [Parallelizable(ParallelScope.Self)]
        [TestCaseSource(nameof(GetUsersFromExcel))]
        public void DataDriven_LoginEachUsers(string userId, string password, string accountNumber)
        {
            var loginPage = new LogInPage(Driver);
            loginPage.Navigate();
            ReportManager.LogInfo($"Navigated to: {BaseUrl}");

            loginPage.LogIn(userId, password);
            ReportManager.LogInfo($"Logged in with User: {userId}");

            var home = new HomePage(Driver);
            Assert.That(home.IsAt(), Is.True, "Failed to log in or not at Home Page.");
            ReportManager.LogPass("Successfully logged in and at Home Page.");

            home.GotoAccountsOverview();
            var accountsOverview = new AccountsOverviewPage(Driver);

            Assert.That(accountsOverview.HasAccount(accountNumber), Is.True, "Account number not found");
            ReportManager.LogPass($"Account number {accountNumber} found for the user {userId}.");
        }
        
        private static System.Collections.IEnumerable GetUsersFromExcel()
        {
            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "users.xlsx");
            var users = ExcelReader.ReadUsers(path);
            Assert.That(users, Is.Not.Empty, "No users found in the Excel file.");

            foreach (var user in users)
            {
                yield return new TestCaseData(user.UserId, user.Password, user.AccountNumber)
                    .SetName($"LoginTest_User_{user.UserId}");
            }
        }
    }
}