using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesApiController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMovieService _service;

        public MoviesApiController(IMapper mapper, IMovieService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet] // GET: /api/movies
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieViewModel>))]  
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<MovieViewModel>> GetMovies()
        {
            return Ok(_service.GetAllMovies());
        }
        
        [HttpGet("{id}")] // GET: /api/movies/5
        [ProducesResponseType(200, Type = typeof(MovieViewModel))]  
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            var movie = _service.GetMovie(id);
            if (movie == null) return NotFound();  
            return Ok(movie);
        }
        
        [HttpPost] // POST: api/movies
        public ActionResult<InputMovieViewModel> PostMovie(MovieDto inputDto)
        {
            var movie = _service.AddMovie(inputDto);
            return CreatedAtAction("GetById", new { id = movie.Id }, movie);
        }
        
        [HttpPut("{id}")] // PUT: api/movies/5
        public IActionResult UpdateMovie(int id, MovieDto editDto)
        {
            var movie = _service.UpdateMovie(editDto);

            if (movie==null)
            {
                return BadRequest();
            }

            return Ok(movie);
        }
        
        [HttpDelete("{id}")] // DELETE: api/movie/5
        public ActionResult<DeleteMovieViewModel> DeleteMovie(int id)
        {
            var movie = _service.DeleteMovie(id);
            if (movie == null) return NotFound();  
            return Ok(movie);
        }
    }
}