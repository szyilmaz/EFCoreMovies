using AutoMapper;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.Precision;

namespace EFCoreMovies.Controllers
{
    public class GenresController : BaseController
    { 
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GenresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Genre>> Get()
        {
            _context.Logs.Add(new Log {Message = "Executing Get from GenresController" });
            await _context.SaveChangesAsync();
            return await _context.Genres
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        [HttpPost("add2")]
        public async Task<ActionResult> Add2(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(p => p.Id == id);

            if (genre == null) return NotFound();

            genre.Name += " 2";

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Post(GenreCreationDTO genre)
        {
            var genreExists = await _context.Genres.AnyAsync(c => c.Name == genre.Name);

            if(genreExists)
            {
                return BadRequest($"The genre with the name {genre.Name} already exists");
            }

            //var status1 = _context.Entry(genre).State;
            var entry = _mapper.Map<Genre>(genre);
            _context.Add(entry);//marking genre as to be added (in memory)
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("several")]
        public async Task<ActionResult> Post(GenreCreationDTO[] genres)
        {
            var entries = _mapper.Map<Genre[]>(genres);

            _context.AddRange(entries);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(p => p.Id == id);

            if (genre == null) return NotFound();

            _context.Remove(genre);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("softDelete/{id:int}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(p => p.Id == id);

            if (genre == null) return NotFound();

            genre.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("restore/{id:int}")]
        public async Task<ActionResult> Restore(int id)
        {
            var genre = await _context.Genres.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);

            if (genre == null) return NotFound();

            genre.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}