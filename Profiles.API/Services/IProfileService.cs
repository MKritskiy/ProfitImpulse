﻿using Profiles.API.Models;

namespace Profiles.API.Services
{
    public interface IProfileService
    {
        Task<int> AddProfile(Profile profile);
        Task DeleteProfile(int  profileId);
        Task<Profile> GetProfileById(int profileId);
        Task<IEnumerable<Profile>> GetProfilesByUserId(int userid);

    }
}
