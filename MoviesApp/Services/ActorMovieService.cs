using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services.Dto;

namespace MoviesApp.Services
{
    public class ActorMovieService:IActorMovieService
    {
        private readonly MoviesContext _context;
        private readonly IMapper _mapper;
        
        public ActorMovieService(MoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        //лучше сделать через ActerMovieDto или просто передавать id?
        public ActerMovieDto AddActerMoviesLink(ActerMovieDto acterMovieDto)
        {
            var link = _context.ActerMovies.Add(_mapper.Map<ActerMovieDto, ActerMovie>(acterMovieDto)).Entity;
            _context.SaveChanges();
            return _mapper.Map<ActerMovie, ActerMovieDto>(link);
        }

        public ActerMovieDto DeleteActerMoviesLink(ActerMovieDto acterMovieDto)
        {
            var link = _context.Remove(_mapper.Map<ActerMovieDto, ActerMovie>(acterMovieDto)).Entity;
            _context.SaveChanges();
            return _mapper.Map<ActerMovie, ActerMovieDto>(link);
            
        }

        public IEnumerable<ActerMovieDto> DeleteAllActorsFilmedInMovieByMovieId(int movieId)
        {
            var deleteActer = _context.ActerMovies.Where(m => m.MovieId == movieId).ToList();
            foreach (var am in deleteActer)
            {
                _context.ActerMovies.Remove(am);
            }
            _context.SaveChanges();

            return _mapper.Map<IEnumerable<ActerMovie>, IEnumerable<ActerMovieDto>>(deleteActer);
        }

        public IEnumerable<ActerMovieDto> AddActerMovieLinks(IEnumerable<ActerMovieDto> acterMovieDto)
        {
            var links = _mapper.Map<IEnumerable<ActerMovieDto>, IEnumerable<ActerMovie>>(acterMovieDto);
            foreach (var l in links)
            {
                _context.ActerMovies.Add(l);
            }
            
            _context.SaveChanges();
            return _mapper.Map<IEnumerable<ActerMovie>, IEnumerable<ActerMovieDto>>(links);
        }
    }
}