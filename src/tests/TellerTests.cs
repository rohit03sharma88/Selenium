using System.IO;
using System.Linq;
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
            Console.WriteLine($"Using Excel file at: {excelPath}");
            excelPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "users.xlsx");
            excelPath = Path.GetFullPath(excelPath);
            
        }

        [Test]
        public void LoginAndVerifyAccountFromExcel()
        {
            Console.WriteLine("Test");
            var users = ExcelReader.ReadUsers(excelPath);
            Assert.That(users, Is.Not.Empty, "No users found in the Excel file.");

            var user = users.First();
            var loginPage = PageFactory.Create<LogInPage>(Driver);
            loginPage.NavigateTo(BaseUrl + "/parabank/index.htm");
            loginPage.LogIn(user.UserId, user.Password);

            var home = PageFactory.Create<HomePage>(Driver);
            var accountsOverview = PageFactory.Create<AccountsOverviewPage>(Driver);

            home.GotoAccountsOverview();
            Assert.That(accountsOverview.HasAccount(user.AccountNumber), Is.Not.Empty, $"No accounts {user.AccountNumber} found for the user {user.UserId}.");
        }

        [Test]
        public void DataDriven_LoginAllUsers()
        {
            var users = ExcelReader.ReadUsers(excelPath);
            Assert.That(users, Is.Not.Empty, "No users found in the Excel file.");

            foreach (var user in users)
            {
                var loginPage = PageFactory.Create<LogInPage>(Driver);
                loginPage.NavigateTo(BaseUrl + "/parabank/index.htm");
                loginPage.LogIn(user.UserId, user.Password);

                var home = PageFactory.Create<HomePage>(Driver);
                home.GotoAccountsOverview();

                var accountsOverview = PageFactory.Create<AccountsOverviewPage>(Driver);
                Assert.That(accountsOverview.GetAccountNumbers().Count() > 0, Is.Not.Empty, $"No accounts {user.AccountNumber} found for the user {user.UserId}.");

                // Logout after each user test
                home.LogOut();

                DriverFactory.QuitDriver();
            }
        }
    }
}