using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services.Dto;

namespace MoviesApp.Services
{
    public class ActorService: IActorService
    {
        private readonly MoviesContext _context;
        private readonly IMapper _mapper;
        
        public ActorService(MoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public ActerDto GetActor(int id)
        {
            return _mapper.Map<ActerDto>(_context.Acters.FirstOrDefault(a => a.Id == id));
        }

        public IEnumerable<ActerDto> GetAllActors()
        {
            return _mapper.Map<IEnumerable<Acter>, IEnumerable<ActerDto>>(_context.Acters.ToList());
        }

        public ActerDto UpdateActor(ActerDto actorDto)
        {
            if (actorDto.Id == null)
            {
                return null;
            }
            
            try
            {
                var actor = _mapper.Map<Acter>(actorDto);
                
                _context.Update(actor);
                _context.SaveChanges();
                
                return _mapper.Map<ActerDto>(actor);
            }
            catch (DbUpdateException)
            {
                if (!ActorExists((int) actorDto.Id))
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }        
        }

        
        public ActerDto AddActor(ActerDto actorDto)
        {
            var actor = _context.Add(_mapper.Map<Acter>(actorDto)).Entity;
            _context.SaveChanges();
            return _mapper.Map<ActerDto>(actor);
        }

        public ActerDto DeleteActor(int id)
        {
            var actor = _context.Acters.Find(id);
            if (actor == null)
            {
                return null;
            }

            _context.Acters.Remove(actor);
            _context.SaveChanges();
            
            return _mapper.Map<ActerDto>(actor);
        }

        public IEnumerable<MovieDto> GetAllMoviesByActorId(int id)
        {
            return _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieDto>>
            (_context.ActerMovies.Where(m => m.ActerId == id)
                .Select(m => m.Movie).ToList());        }
        
        private bool ActorExists(int id)
        {
            return _context.Acters.Any(e => e.Id == id);
        }
    }
}