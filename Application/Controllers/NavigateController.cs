using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigateController : ControllerBase
    {
        private readonly IShortenedUrlRepository _shortenedUrlRepository;

        public NavigateController(IShortenedUrlRepository shortenedUrlRepository)
        {
            _shortenedUrlRepository = shortenedUrlRepository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(308)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Navigate(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var longUrl = await _shortenedUrlRepository.GetShortenedUrlAsync(id);

                if (longUrl == null)
                    return NotFound(new { Message = "No such shortened url." });

                return RedirectPermanent(longUrl.LongUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
