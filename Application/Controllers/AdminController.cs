using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAdminRepository _adminRepository;

        public AdminController(UserManager<User> userManager, IAdminRepository adminRepository)
        {
            _userManager = userManager;
            _adminRepository = adminRepository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> MakeNewAdmin(Guid id)
        {
            try 
            { 
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                User user = await _adminRepository.GetUserByIdAsync(id);

                if (user == null)
                    return BadRequest("User doesnt exist");

                var result = await _userManager.AddToRoleAsync(user, "admin");

                if (!result.Succeeded)
                    return StatusCode(500, new { Message = "Something happen during role updating." });

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("urls")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAllUrls()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _adminRepository.DeleteAllUrlsAsync())
                    return StatusCode(500, new { Message = "Something happen during all urls remove" });

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("users")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            { 
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(await _adminRepository.GetAllUsers());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
