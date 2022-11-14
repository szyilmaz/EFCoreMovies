using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace EFCoreMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MoviesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("automapper/{id:int}")]
        public async Task<ActionResult<MovieDTO>> GetWithAutoMapper(int id)
        {
            var movieDTO = await _context.Movies
                .ProjectTo<MovieDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movieDTO is null)
            {
                return NotFound();
            }
            movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(x => x.Id).ToList();

            return movieDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post(MovieCreationDTO movieCreationDTO)
        {
            var movie = _mapper.Map<Movie>(movieCreationDTO);

            movie.Genres.ForEach(g => _context.Entry(g).State = EntityState.Unchanged);
            movie.CinemaHalls.ForEach(g => _context.Entry(g).State = EntityState.Unchanged);

            if(movie.MoviesActors is not null)
            {
                for(int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            _context.Add(movie);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
#region old

//[HttpGet("{id:int}")]
//public async Task<ActionResult<MovieDTO>> Get(int id)
//{
//    var movie = await _context.Movies
//        .Include(c => c.Genres.OrderByDescending(g => g.Name).Where(g => !g.Name.Contains("m")))
//        .Include(c => c.CinemaHalls.OrderByDescending(ch => ch.Cinema.Name))
//            .ThenInclude(e => e.Cinema)
//        .Include(m => m.MoviesActors)
//            .ThenInclude(ma => ma.Actor)
//        .FirstOrDefaultAsync(m => m.Id == id);

//    if (movie is null)
//    {
//        return NotFound();
//    }

//    var movieDTO = _mapper.Map<MovieDTO>(movie);

//    movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(x => x.Id).ToList();

//    return movieDTO;
//}

//[HttpGet("selectoading/{id:int}")]
//public async Task<ActionResult> GetSelectLoading(int id)
//{
//    var movieDTO = await _context.Movies.Select(m => new
//    {
//        Id = m.Id,
//        Title = m.Title,
//        Genres = m.Genres.Select(g => g.Name).OrderByDescending(n => n).ToList()
//    }).FirstOrDefaultAsync(m =>m.Id == id);

//    if (movieDTO is null)
//    {
//        return NotFound();
//    }

//    return Ok(movieDTO);
//}

//[HttpGet("explicitloading/{id:int}")]
//public async Task<ActionResult<MovieDTO>> GetExplicit(int id)
//{
//    var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

//    if (movie is null)
//    {
//        return NotFound();
//    }

//    var genresCount = await _context.Entry(movie).Collection(p => p.Genres).Query().CountAsync();

//    var movieDTO = _mapper.Map<MovieDTO>(movie);

//    return Ok(new
//    {
//        Id = movieDTO.Id,
//        Title = movieDTO.Title,
//        GenresCount = genresCount
//    });    
//}

//[HttpGet("lazyloading/{id:int}")]
//public async Task<ActionResult<MovieDTO>> GetLazyLoading(int id)
//{
//    var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

//    if (movie == null) return NotFound();

//    var movieDTO = _mapper.Map<MovieDTO>(movie);

//    movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(x => x.Id).ToList();

//    return Ok(movieDTO);
//}

//[HttpGet("groupedByCinema")]
//public async Task<ActionResult> GetGroupedByCinema()
//{
//    var groupedMovies = await _context.Movies.GroupBy(m => m.InCinemas).Select(g => new
//    {
//        InCinemas = g.Key,
//        Count = g.Count(),
//        Movies = g.ToList()
//    }).ToListAsync();

//    return Ok(groupedMovies);
//}

//[HttpGet("groupedByGenresCount")]
//public async Task<ActionResult> GetGroupedByGenresCount()
//{
//    var groupedMovies = await _context.Movies.GroupBy(m => m.Genres.Count()).Select(g => new
//    {
//        Count = g.Key,
//        Titles = g.Select(m => m.Title),
//        Genres = g.Select(m => m.Genres).SelectMany(a => a).Select(ge => ge.Name).Distinct()
//    }).ToListAsync();

//    return Ok(groupedMovies);
//}

//[HttpGet("filter")]
//public async Task<ActionResult<IEnumerable<MovieDTO>>> Filter([FromQuery] MovieFilterDTO movieFilterDTO)
//{
//    var moviesQueryable = _context.Movies.AsQueryable();
//    if (!string.IsNullOrEmpty(movieFilterDTO.Title))
//    {
//        moviesQueryable = moviesQueryable.Where(c => c.Title.Contains(movieFilterDTO.Title));
//    }

//    if (movieFilterDTO.InCinemas)
//    {
//        moviesQueryable = moviesQueryable.Where(c => c.InCinemas);
//    }

//    if (movieFilterDTO.UpComingReleases)
//    {
//        var today = DateTime.Today;

//        moviesQueryable = moviesQueryable.Where(c => c.ReleaseDate > today);
//    }

//    if (movieFilterDTO.GenreId != 0)
//    {
//        moviesQueryable = moviesQueryable.Where(c => c.Genres.Select(g => g.Id).Contains(movieFilterDTO.GenreId));
//    }

//    var movies = await moviesQueryable.Include(c => c.Genres).ToListAsync();

//    return _mapper.Map<List<MovieDTO>>(movies);

//}
#endregion old