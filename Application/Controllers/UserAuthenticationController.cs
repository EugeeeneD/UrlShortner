using AutoMapper;
using Domain.Dto.Request;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationRepository _userAuthenticationRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IMapper _mapper;

        public UserAuthenticationController(IUserAuthenticationRepository userAuthenticationRepository,
                                    UserManager<User> userManager,
                                    RoleManager<IdentityRole<Guid>> roleManager,
                                    IMapper mapper)
        {
            _userAuthenticationRepository = userAuthenticationRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userAuthenticationRepository = userAuthenticationRepository;
        }

        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (userDTO == null)
                    return BadRequest("user cannot be null.");

                User user = _mapper.Map<User>(userDTO);

                var userResult = await _userAuthenticationRepository.RegisterUserAsync(user, userDTO);
                
                if (!userResult.Succeeded)
                    return BadRequest(new { Message = "Password have to be at least 8 char lenght and email have to contains @." });

                var userRole = _roleManager.FindByNameAsync("user").Result.Name;

                var result = await _userManager.AddToRoleAsync(user, userRole);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            if (!await _userAuthenticationRepository.ValidateUserAsync(user))
                return Unauthorized(new { Message = "Incorrect username or password." });

            var token = await _userAuthenticationRepository.CreateTokenAsync();

            HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddDays(10),
                SameSite = SameSiteMode.Lax
            });

            return Ok(new { Token = token });
        }

        //delete
        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            if (HttpContext.Request.Headers.Cookie.Any(x => x.StartsWith("jwt")))
            {
                // token is included in the request header
                // do something with the token, such as validate it and retrieve user information
                // return data that is only accessible to authenticated users
                return Ok("This is protected data that can only be accessed by authenticated users.");
            }
            else
            {
                // token is not included in the request header
                // return an HTTP 401 Unauthorized response
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");
            return NoContent();
        }

        [HttpGet("claims")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetClaims()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            var jwtHandler = new JwtSecurityTokenHandler();

            var jwt = jwtHandler.ReadJwtToken(token);

            var claims = jwt.Claims;

            return Ok(claims);
        }
    }
}
