using Helpers;
using Helpers.Database;
using Inventories.API.Repositories;
using Inventories.API.Services;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Internal;
using Orders.API.Repositories;
using Orders.API.Services;
using Profiles.API.Repositories;
using Profiles.API.Services;
using Purchases.API.Repositories;
using Purchases.API.Services;
using Users.API.Repositories;
using Users.API.Services.Encrypt;
using Users.API.Services.General;
using Users.API.Services.Token;
using Users.API.Services.UserService;

namespace ProfitImpulseTest.Helpers
{
    public class BaseTest
    {
        protected IUserRepository userRepository = new UserRepository();
        protected IEncrypt encrypt = new Encrypt();
        protected ITokenGenerator tokenGenerator;
        protected IUserService userService;

        protected IProfileRepository profileRepository = new ProfileRepository();
        protected IProfileService profileService;

        protected IPurchaseRepository purchaseRepository = new PurchaseRepository();
        protected IPurchaseUpdateRepository purchaseUpdateRepository = new PurchaseUpdateRepository();
        protected IPurchaseService purchaseService;

        protected IRequestApiHelper requestApiHelper;

        protected IStockRepository stockRepository = new StockRepository();
        protected IStockUpdateRepository stockUpdateRepository = new StockUpdateRepository();
        protected IStockService stockService;


        protected IOrderRepository orderRepository = new OrderRepository();
        protected IOrderUpdateRepository orderUpdateRepository = new OrderUpdateRepository();
        protected IOrderService orderService;

        protected IConfiguration configuration;
        JwtSettings jwtSettings;
        public BaseTest()
        {
            Dictionary<string, string?> myConf = new Dictionary<string, string?>{
                {"ConnectionStrings:DefaultConnection", "Host=localhost;Port=5432;Database=ProfitImpulseDB;Username=postgres;Password=homyak" }
            };
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConf)
                .Build();
            DbHelper.Initialize(configuration);

            jwtSettings = new JwtSettings(
                issuer: "your_issuer",
                audience: "your_audience",
                key: "87ecc38951d132938fa2ee921e58c0f3691b7ebfb8c5cdf092abb347cb78fc72",
                tokenLifetimeMinutes: 60
            );
            requestApiHelper = new FakeRequestApiHelper();
            tokenGenerator = new TokenGenerator(jwtSettings);
            userService = new UserService(userRepository, encrypt, tokenGenerator);
            profileService = new ProfileService(profileRepository);
            purchaseService = new PurchaseService(purchaseRepository, purchaseUpdateRepository, requestApiHelper);
            stockService = new StockService(stockRepository, stockUpdateRepository, requestApiHelper);
            orderService = new OrderService(orderRepository, orderUpdateRepository, requestApiHelper);
        }
    }
}
