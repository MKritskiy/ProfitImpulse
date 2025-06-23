using Profiles.API.Models;
using Profiles.API.Repositories;

namespace Profiles.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<int> AddProfile(Profile profile)
        {
            return await _profileRepository.AddProfileAsync(profile);
        }

        public async Task DeleteProfile(int profileId)
        {
            bool res = await _profileRepository.DeleteProfileByIdAsync(profileId);
            if (!res)
                throw new Exception();
        }

        public async Task<Profile> GetProfileById(int profileId)
        {
            return await _profileRepository.GetProfileByIdAsync(profileId);
        }

        public async Task<IEnumerable<Profile>> GetProfilesByUserId(int userid)
        {
            return await _profileRepository.GetProfilesByUserIdAsync(userid);
        }
    }
}
