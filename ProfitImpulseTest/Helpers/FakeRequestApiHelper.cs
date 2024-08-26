using Helpers;
using Purchases.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProfitImpulseTest.Helpers
{
    internal class FakeRequestApiHelper : IRequestApiHelper
    {
        string sales = "[{\r\n        \"purchaseId\": 12496,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T18:46:48\",\r\n        \"purchaseAmount\": 1000.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Бальзамы\",\r\n        \"purchaseSku\": \"2039466938938\",\r\n        \"purchaseBrand\": \"LA ROCHE-POSAY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Северо-Кавказский федеральный округ\",\r\n        \"purchaseRegion\": \"Кабардино-Балкарская Республика\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12497,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T19:57:07\",\r\n        \"purchaseAmount\": 1550.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Дезодоранты\",\r\n        \"purchaseSku\": \"2039321643625\",\r\n        \"purchaseBrand\": \"VICHY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Центральный федеральный округ\",\r\n        \"purchaseRegion\": \"Московская область\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12498,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T20:02:31\",\r\n        \"purchaseAmount\": 900.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Кремы\",\r\n        \"purchaseSku\": \"2039518178428\",\r\n        \"purchaseBrand\": \"LA ROCHE-POSAY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Северо-Западный федеральный округ\",\r\n        \"purchaseRegion\": \"Архангельская область\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12499,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T15:39:46\",\r\n        \"purchaseAmount\": 1200.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Сыворотки\",\r\n        \"purchaseSku\": \"2039437504155\",\r\n        \"purchaseBrand\": \"VICHY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Южный федеральный округ\",\r\n        \"purchaseRegion\": \"Ростовская область\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12500,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T11:13:49\",\r\n        \"purchaseAmount\": 770.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Кремы\",\r\n        \"purchaseSku\": \"2039437643649\",\r\n        \"purchaseBrand\": \"LA ROCHE-POSAY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Сибирский федеральный округ\",\r\n        \"purchaseRegion\": \"Алтайский край\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12501,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T11:43:36\",\r\n        \"purchaseAmount\": 1400.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Сыворотки\",\r\n        \"purchaseSku\": \"2039517191756\",\r\n        \"purchaseBrand\": \"VICHY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Приволжский федеральный округ\",\r\n        \"purchaseRegion\": \"Саратовская область\"\r\n    }]";
        string orders = "[{\r\n        \"purchaseId\": 12496,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T18:46:48\",\r\n        \"purchaseAmount\": 1000.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Бальзамы\",\r\n        \"purchaseSku\": \"2039466938938\",\r\n        \"purchaseBrand\": \"LA ROCHE-POSAY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Северо-Кавказский федеральный округ\",\r\n        \"purchaseRegion\": \"Кабардино-Балкарская Республика\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12497,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T19:57:07\",\r\n        \"purchaseAmount\": 1550.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Дезодоранты\",\r\n        \"purchaseSku\": \"2039321643625\",\r\n        \"purchaseBrand\": \"VICHY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Центральный федеральный округ\",\r\n        \"purchaseRegion\": \"Московская область\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12498,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T20:02:31\",\r\n        \"purchaseAmount\": 900.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Кремы\",\r\n        \"purchaseSku\": \"2039518178428\",\r\n        \"purchaseBrand\": \"LA ROCHE-POSAY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Северо-Западный федеральный округ\",\r\n        \"purchaseRegion\": \"Архангельская область\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12499,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T15:39:46\",\r\n        \"purchaseAmount\": 1200.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Сыворотки\",\r\n        \"purchaseSku\": \"2039437504155\",\r\n        \"purchaseBrand\": \"VICHY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Южный федеральный округ\",\r\n        \"purchaseRegion\": \"Ростовская область\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12500,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T11:13:49\",\r\n        \"purchaseAmount\": 770.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Кремы\",\r\n        \"purchaseSku\": \"2039437643649\",\r\n        \"purchaseBrand\": \"LA ROCHE-POSAY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Сибирский федеральный округ\",\r\n        \"purchaseRegion\": \"Алтайский край\"\r\n    },\r\n    {\r\n        \"purchaseId\": 12501,\r\n        \"profileId\": 1,\r\n        \"purchaseDate\": \"2024-08-06T11:43:36\",\r\n        \"purchaseAmount\": 1400.00,\r\n        \"purchaseQuantity\": 1,\r\n        \"purchaseName\": \"Сыворотки\",\r\n        \"purchaseSku\": \"2039517191756\",\r\n        \"purchaseBrand\": \"VICHY\",\r\n        \"purchaseCategory\": \"Красота\",\r\n        \"purchaseCountry\": \"Россия\",\r\n        \"purchaseState\": \"Приволжский федеральный округ\",\r\n        \"purchaseRegion\": \"Саратовская область\"\r\n    }]";
        string stocks = "[{\r\n        \"stockId\": 155,\r\n        \"profileId\": 1,\r\n        \"warehouseName\": \"Тула\",\r\n        \"productQuantity\": 1,\r\n        \"productName\": \"Красота\",\r\n        \"productSku\": \"2039462060008\"\r\n    },\r\n    {\r\n        \"stockId\": 156,\r\n        \"profileId\": 1,\r\n        \"warehouseName\": \"Коледино\",\r\n        \"productQuantity\": 4,\r\n        \"productName\": \"Красота\",\r\n        \"productSku\": \"2039541738934\"\r\n    },\r\n    {\r\n        \"stockId\": 157,\r\n        \"profileId\": 1,\r\n        \"warehouseName\": \"Электросталь\",\r\n        \"productQuantity\": 2,\r\n        \"productName\": \"Красота\",\r\n        \"productSku\": \"2039596274890\"\r\n    },\r\n    {\r\n        \"stockId\": 158,\r\n        \"profileId\": 1,\r\n        \"warehouseName\": \"Электросталь\",\r\n        \"productQuantity\": 11,\r\n        \"productName\": \"Красота\",\r\n        \"productSku\": \"2039541738934\"\r\n    },\r\n    {\r\n        \"stockId\": 159,\r\n        \"profileId\": 1,\r\n        \"warehouseName\": \"Белые Столбы\",\r\n        \"productQuantity\": 1,\r\n        \"productName\": \"Красота\",\r\n        \"productSku\": \"2039418333293\"\r\n    },\r\n    {\r\n        \"stockId\": 160,\r\n        \"profileId\": 1,\r\n        \"warehouseName\": \"Белые Столбы\",\r\n        \"productQuantity\": 11,\r\n        \"productName\": \"Красота\",\r\n        \"productSku\": \"2039541738934\"\r\n    },\r\n    {\r\n        \"stockId\": 161,\r\n        \"profileId\": 1,\r\n        \"warehouseName\": \"Электросталь\",\r\n        \"productQuantity\": 24,\r\n        \"productName\": \"Красота\",\r\n        \"productSku\": \"2040530708685\"\r\n    },\r\n    {\r\n        \"stockId\": 162,\r\n        \"profileId\": 1,\r\n        \"warehouseName\": \"Электросталь\",\r\n        \"productQuantity\": 13,\r\n        \"productName\": \"Красота\",\r\n        \"productSku\": \"2040530496889\"\r\n    }]";

        public async Task<IEnumerable<T>> FetchListFromApi<T>(string query, int profileid, string jwtToken)
        {
            IEnumerable<T> apiObject;
            await Task.Delay(100);
            switch (query)
            {
                case "https://statistics-api.wildberries.ru/api/v1/supplier/sales?dateFrom=2019-06-20":
                    apiObject = JsonSerializer.Deserialize<IEnumerable<T>>(sales, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiObject == null)
                        throw new Exception("Failed to fetch Objects from api.");

                    return apiObject;
                case "https://statistics-api.wildberries.ru/api/v1/supplier/orders?dateFrom=2019-06-20":
                    apiObject = JsonSerializer.Deserialize<IEnumerable<T>>(orders, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiObject == null)
                        throw new Exception("Failed to fetch Objects from api.");

                    return apiObject;
                case "https://statistics-api.wildberries.ru/api/v1/supplier/stocks?dateFrom=2019-06-20":
                    apiObject = JsonSerializer.Deserialize<IEnumerable<T>>(stocks, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiObject == null)
                        throw new Exception("Failed to fetch Objects from api.");

                    return apiObject;
                default:
                    throw new Exception("Failed to fetch Objects from api.");
            }
        }

        public async Task<string?> GetAuthorizationKey(int profileid, string jwtToken)
        {
            await Task.Delay(10);
            return "auth_key";
        }
    }
}
