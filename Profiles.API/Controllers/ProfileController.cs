using Microsoft.AspNetCore.Mvc;
using Profiles.API.Models;
using Profiles.API.Services;

namespace Profiles.API.Controllers
{
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        [HttpGet]
        [Route("/{id}")]
        public async Task<IActionResult> Authkey(int id)
        {
            Profile profile = await _profileService.GetProfileById(id);

            return Ok(new { apikey = profile.ApiKey });
        }
        [HttpGet]
        [Route("/user/{id}")]
        public async Task<IActionResult> Userprofiles(int id)
        {
            var profiles = await _profileService.GetProfilesByUserId(id);
            return Ok(profiles);
        }

        [HttpPost]
        [Route("/create")]
        public async Task<IActionResult> CreateProfile(Profile profile)
        {
            int id = await _profileService.AddProfile(profile);
            return Ok(new { ProfileId = id });
        }
    }
}
