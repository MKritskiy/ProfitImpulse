global using NUnit.Framework;
using System.Transactions;
using Helpers.Database;
using Inventories.API.Models;
using Orders.API.Models;
using Profiles.API.Models;
using ProfitImpulseTest.Helpers;
using Purchases.API.Models;

namespace ProfitImpulseTest
{
    public class UserActionTest : Helpers.BaseTest
    {
        [Test]
        public async Task Test1()
        {
            using (TransactionScope scope = Helper.CreateTransactionScope())
            {
                var regDto = new Users.API.Dto.RegisterDto()
                {
                    Email = Guid.NewGuid().ToString() + "@test.com",
                    Password = "Sasha1234",
                    Username = "sasha"
                };
                // Register the user
                var afterReg = await userService.Register(regDto);
                
                Assert.That(afterReg.Id, Is.GreaterThan(0));

                var profile = new Profile()
                {
                    ApiKey = Guid.NewGuid().ToString(),
                    ProfileName = "SashaProfile",
                    UserId = afterReg.Id
                };
                // Create the user profile
                int profileId = await profileService.AddProfile(profile);

                Assert.That(profileId, Is.GreaterThan(0));

                // get orders
                IEnumerable<Order> orders = await orderService.GetOrdersAsync(profileId, afterReg.Token);

                Assert.That(orders.Any(), Is.True);

                // get purchases
                IEnumerable<Purchase> purchases = await purchaseService.GetPurchasesAsync(profileId, afterReg.Token);

                Assert.That(purchases.Any(), Is.True);

                // get stocks
                IEnumerable<Stock> stocks = await stockService.GetStocksAsync(profileId, afterReg.Token);

                Assert.That(stocks.Any(), Is.True);

                Assert.Pass();
            }
        }
    }
}