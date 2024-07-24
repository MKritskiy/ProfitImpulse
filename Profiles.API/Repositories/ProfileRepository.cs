using Profiles.API.Models;
using Users.API.Database;

namespace Profiles.API.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        public async Task<int> AddProfileAsync(Profile profile)
        {
            const string query = @"INSERT INTO Profiles (user_id, profile_name, api_key) 
                                        VALUES (@UserId, @ProfileName, @ApiKey) 
                                        RETURNING profile_id";
            return await DbHelper.QueryFirstOrDefaultAsync<int>(query, profile);
        }

        public async Task<bool> DeleteProfileByIdAsync(int profileid)
        {
            const string query = "DELETE FROM Profiles WHERE profile_id = @ProfileId";
            int affectedRows = await DbHelper.ExecuteAsync(query, new { ProfileId = profileid });
            return affectedRows > 0;
        }

        public async Task<Profile> GetProfileByIdAsync(int profileid)
        {
            const string query = "SELECT * FROM Profiles";
            return await DbHelper.QueryFirstOrDefaultAsync<Profile>(query, new { ProfileId = profileid }) ?? new Profile();
        }

        public async Task<IEnumerable<Profile>> GetProfilesByUserIdAsync(int userid)
        {
            const string query = "SELECT * FROM Profiles WHERE user_id = @UserId";
            return await DbHelper.QueryAsync<Profile>(query, new { UserId = userid });
        }
    }
}
