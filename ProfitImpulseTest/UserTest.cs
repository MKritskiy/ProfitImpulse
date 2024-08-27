using ProfitImpulseTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Users.API.Exceptions;
using Users.API.Models;

namespace ProfitImpulseTest
{
    internal class UserTest : BaseTest
    {
        [Test]
        public async Task RegisterTest()
        {
            using (TransactionScope scope = Helper.CreateTransactionScope())
            {
                var user1 = new User()
                {
                    Email = "test@test.com",
                    PasswordHash = "Sasha1234",
                    Username = "sasha1",
                };
                // Duplicate email
                var user2 = new User()
                {
                    Email = "test@test.com",
                    PasswordHash = "Sasha1234",
                    Username = "sasha2"
                };
                // Duplcate username
                var user3 = new User()
                {
                    Email = "other.test@test.com",
                    PasswordHash = "Sasha1234",
                    Username = "sasha1"
                };
                
                var afterReg = await userService.SaveNewUserAsync(user1);
                Assert.That(afterReg, Is.GreaterThan(0));

                Assert.ThrowsAsync<DuplicateEmailException>(async () => await userService.ValidateNewUser(user2));

                Assert.ThrowsAsync<DuplicateUsernameException>(async () => await userService.ValidateNewUser(user3));

                Assert.ThrowsAsync<InvalidOperationException>(async () => await userService.SaveNewUserAsync(new User()));


                var afterAuht = await userService.Login(new Users.API.Dto.LoginDto() { NameOrEmail = "sasha1", Password = "Sasha1234" });
                Assert.That(afterAuht.Id, Is.GreaterThan(0));
            }
        }
    }
}
