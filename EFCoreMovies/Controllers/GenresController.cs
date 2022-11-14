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
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
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
            return await _context.Genres
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(GenreCreationDTO genre)
        {
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


    }
}