using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    [Route("api/acters")]
    [ApiController]
    public class ActersApiController:ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IActorService _actorService;
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        public ActersApiController(IMapper mapper, IMovieService movieService, IActorService actorService, ILogger<HomeController> logger)
        {
            _movieService = movieService;
            _logger = logger;
            _actorService = actorService;
            _mapper = mapper;
        }

        [HttpGet] // GET: /api/acters
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActerViewModel>))]  
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ActerViewModel>> GetActers()
        {
            var acters = _mapper.Map<IEnumerable<ActerDto>, IEnumerable<ActerViewModel>>
                (_actorService.GetAllActors());
            return Ok(acters);
        }
        
        
        [HttpGet("withMovies/{id}")] // GET: /api/acters
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActerViewModel>))]  
        [ProducesResponseType(404)]
        public IActionResult GetActerWithMovies(int id)
        {
            var acter = _mapper.Map<EditActerViewModel>(_actorService.GetActor(id));
            if (acter == null) return NotFound();
            acter.SelectMovies = _mapper.Map<IEnumerable<MovieDto>,IEnumerable<ActerMovieViewModel>>
                (_movieService.GetAllMoviesByActorId(id));
            return Ok(acter);
        }
        
        [HttpGet("{id}")] // GET: /api/acters/5
        [ProducesResponseType(200, Type = typeof(ActerViewModel))]  
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            var acter = _mapper.Map<ActerViewModel>(_actorService.GetActor(id));
            if (acter == null) return NotFound();  
            return Ok(acter);
        }
        
        [HttpPost] // POST: api/acters
        public ActionResult<ActerDto> PostActer(ActerDto inputDto)
        {
            var movie = _actorService.AddActor(inputDto);
            return CreatedAtAction("GetById", new { id = movie.Id }, movie);
        }
        
        [HttpPut("{id}")] // PUT: api/acters/5
        public IActionResult UpdateActer(int id, ActerDto editDto)
        {
            var actor = _actorService.UpdateActor(editDto);

            if (actor==null)
            {
                return BadRequest();
            }

            return Ok(actor);
        }
        
        [HttpDelete("{id}")] // DELETE: api/acters/5
        public ActionResult<ActerDto> DeleteActer(int id)
        {
            var movie = _actorService.DeleteActor(id);
            if (movie == null) return NotFound();  
            return Ok(movie);
        }
    }
}