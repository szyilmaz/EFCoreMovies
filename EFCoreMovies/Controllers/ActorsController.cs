using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using EFCoreMovies.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers
{
    public class ActorsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ActorDTO>> Get()
        {
            return await _context.Actors
                .OrderBy(g => g.Name)
                .ProjectTo<ActorDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ActorCreationDTO actorCreationDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreationDTO);
            _context.Add(actor);
            await _context.SaveChangesAsync(); ;
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ActorCreationDTO actorCreationDTO, int id)
        {
            var actorDB = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);

            if (actorDB == null) return NotFound();

            actorDB = _mapper.Map(actorCreationDTO, actorDB);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("disconnected/{id:int}")]
        public async Task<ActionResult> PutDisconnected(ActorCreationDTO actorCreationDTO, int id)
        {
            var existsActor = await _context.Actors.AnyAsync(a => a.Id == id);

            if (!existsActor) return NotFound();

            var actor = _mapper.Map<Actor>(actorCreationDTO);

            actor.Id = id;

            _context.Update(actor);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}