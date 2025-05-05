using BlazorApp.Server.Data;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenDefinitionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScreenDefinitionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveScreen([FromBody] SaveRequest request)
        {
            var screen = new ScreenDefinition
            {
                Name = request.Name,
                JsonDefinition = JsonSerializer.Serialize(request.Controls),
                CreatedAt = DateTime.UtcNow
            };

            _context.ScreenDefinitions.Add(screen);
            await _context.SaveChangesAsync();

            return Ok(screen.Id);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ScreenSummary>>> GetAllScreens()
        {
            var summaries = await _context.ScreenDefinitions
                .Select(s => new ScreenSummary
                {
                    Id = s.Id,
                    Name = s.Name,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();

            return Ok(summaries);
        }

        [HttpGet("load/{id}")]
        public async Task<IActionResult> LoadScreen(int id)
        {
            var screen = await _context.ScreenDefinitions.FindAsync(id);
            if (screen == null)
            {
                return NotFound();
            }

            return Ok(new LoadResponse
            {
                JsonDefinition = screen.JsonDefinition
            });
        }
    }
}
