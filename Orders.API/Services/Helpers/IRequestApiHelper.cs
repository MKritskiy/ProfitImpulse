﻿using Orders.API.Models;

namespace Orders.API.Services.Helpers
{
    public interface IRequestApiHelper
    {
        Task<IEnumerable<T>> FetchListFromApi<T>(string query, int profileid, string jwtToken);
        Task<string?> GetAuthorizationKey(int profileid, string jwtToken);
    }
}
