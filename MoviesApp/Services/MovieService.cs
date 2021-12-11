using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services.Dto;

namespace MoviesApp.Services
{
    public class MovieService:IMovieService
    {
        private readonly MoviesContext _context;
        private readonly IMapper _mapper;
        
        public MovieService(MoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public MovieDto GetMovie(int id)
        {
            return _mapper.Map<MovieDto>(_context.Movies.FirstOrDefault(m => m.Id == id));
        }

        public IEnumerable<MovieDto> GetAllMovies()
        {
            return _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieDto>>(_context.Movies.ToList());
        }

        public MovieDto UpdateMovie(MovieDto movieDto)
        {
            if (movieDto.Id == null)
            {
                return null;
            }
            
            try
            {
                var movie = _mapper.Map<Movie>(movieDto);
                
                _context.Update(movie);
                _context.SaveChanges();
                
                return _mapper.Map<MovieDto>(movie);
            }
            catch (DbUpdateException)
            {
                if (!MovieExists((int) movieDto.Id))
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public MovieDto AddMovie(MovieDto movieDto)
        {
            var movie = _context.Add(_mapper.Map<Movie>(movieDto)).Entity;
            _context.SaveChanges();
            return _mapper.Map<MovieDto>(movie);
        }

        public MovieDto DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return null;
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            
            return _mapper.Map<MovieDto>(movie);
        }

        public IEnumerable<ActerDto> GetAllActorByMovieId(int id)
        {
            return _mapper.Map<IEnumerable<Acter>, IEnumerable<ActerDto>>
            (_context.ActerMovies.Where(m => m.MovieId == id)
                .Select(m => m.Acter).ToList());
        }

        public IEnumerable<ActerDto> GetNotFilmedActersByMovieId(int id)
        {
            var movieActersList = _context.ActerMovies.Where(a => a.MovieId == id).Select(m => new MovieActerViewModel()
            {
                Id = m.Acter.Id,
                Name = m.Acter.Name
            }).ToList();

            var actersList = _context.Acters
                .Select(m => new MovieActerViewModel
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList();
            
            var buffer = new MovieActerViewModel();
            foreach (var a in movieActersList)
            {
                foreach (var m in actersList)
                {
                    if (a.CompareTo(m) == 0)
                    {
                        buffer = m;
                    }
                }

                if (buffer != null)
                {
                    actersList.Remove(buffer);//Console.WriteLine($"elemet deleted m {buffer.Id} - {buffer.Name}");
                    buffer = null;
                }
            }
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}