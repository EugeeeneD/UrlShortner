using AutoMapper;
using Domain.Dto.Request;
using Domain.Dto.Response;
using Domain.Interfaces;
using Domain.Interfaces.TokenHandler;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors("UrlShortner")]
    public class UrlShortenedController : ControllerBase
    {
        private readonly IShortenedUrlRepository _shortenedUrlRepository;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        private readonly IServer _server;

        public UrlShortenedController(IShortenedUrlRepository shortenedUrlRepository,
                                      IMapper mapper,
                                      ITokenHandler tokenHandler, 
                                      IServer server)
        {
            _shortenedUrlRepository = shortenedUrlRepository;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
            _server = server;
        }

        [HttpGet("/urls/{id}")]
        [ProducesResponseType(typeof(UrlShortenedDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUrlInfo(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _shortenedUrlRepository.GetShortenedUrlAsync(id);

            var urlInfo = _mapper.Map<UrlShortenedDto>(res);
            SetDomainNameForShortenedUrl(urlInfo);

            return Ok(urlInfo);
        }

        [HttpGet("/urls")]
        [ProducesResponseType(typeof(ICollection<UrlShortenedDto>), 200)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> GetShortenedUrls()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _shortenedUrlRepository.GetShortenedUrlsAsync();

            var urlInfo = _mapper.Map<List<UrlShortenedDto>>(res);

            urlInfo.ForEach(x => SetDomainNameForShortenedUrl(x));

            return Ok(urlInfo);
        }

        [HttpDelete("/urls/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteShortenedUrl(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (!authHeader.StartsWith("Bearer ") || authHeader == null)
                return StatusCode(401);

            var token = authHeader.Split(" ")[1];

            Guid userId = default;
            string role = string.Empty;
            if (!_tokenHandler.TryGetIdFromJwtToken(token, ref userId))
                return BadRequest(new { Message = "Invalid token." });
            if (!_tokenHandler.TryGetRoleFromJwtToken(token, ref role))
                return Forbid();

            var url = await _shortenedUrlRepository.GetShortenedUrlAsync(id);

            if (url == null)
                return NotFound();

            if (role != "admin")
                if(url.UserId != userId)
                    return Forbid();

            if (!await _shortenedUrlRepository.DeleteShortnedUrlAsync(url))
                return StatusCode(500, new { Message = "Something happen during url remove" });

            return Ok();
        }

        [HttpPost("/urls")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateShortenedUrl([FromBody] CreateShortenedUrl urlDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (urlDto == null)
                return BadRequest(new { Message = "Url object cannot be null" });

            var urlExists = _shortenedUrlRepository.DoesLongUrlExists(urlDto.Url);

            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (!authHeader.StartsWith("Bearer ") || authHeader == null)
                return StatusCode(401);

            var token = authHeader.Split(" ")[1];

            Guid userId = default;
            if (!_tokenHandler.TryGetIdFromJwtToken(token, ref userId))
                return BadRequest("Invalid token.");

            if (await urlExists)
                return StatusCode(405, new { Message = "Shortened Url already exist." });

            if (!await _shortenedUrlRepository.AddShortenedUrlAsync(urlDto.Url, userId))
                return StatusCode(500, new { Message = "Something happen during url adding" });

            return Ok();
        }

        // should i include this to repo?
        private void SetDomainNameForShortenedUrl(UrlShortenedDto urlShortenedDto)
        {
            string domainName = _server.Features.Get<IServerAddressesFeature>().Addresses.First();
            urlShortenedDto.Shortened = domainName + "/api/Navigate/" + urlShortenedDto.Id;
        }
    }
}
