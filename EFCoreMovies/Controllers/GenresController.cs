using EFCoreMovies.Entities;
using EFCoreMovies.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Genre>> Get(int page = 1, int recordsToTake = 2)
        {
            return await _context.Genres
                .AsNoTracking()
                .Paginate(page, recordsToTake)
                .ToListAsync();
        }

        [HttpGet("first")]
        public async Task<ActionResult<Genre>> GetFirst()
        {
            var genre =  await _context.Genres.FirstOrDefaultAsync(c=> c.Name.Contains("z"));

            if (genre == null) return NotFound();
            return genre;
        }

        [HttpGet("filter")]
        public async Task<IEnumerable<Genre>> Filter(string name)
        {
            return await _context.Genres.AsNoTracking().Where(g => g.Name.Contains(name)).ToListAsync();
        }
    }
}