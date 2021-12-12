using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    [Route("api/acters")]
    //[Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    [ApiController]
    public class ActersApiController:ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IActorService _service;
        private readonly IMapper _mapper;

        public ActersApiController(IMapper mapper, IActorService service, ILogger<HomeController> logger)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet] // GET: /api/acters
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActerViewModel>))]  
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ActerViewModel>> GetActers()
        {
            var acters = _mapper.Map<IEnumerable<ActerDto>, IEnumerable<ActerViewModel>>
                (_service.GetAllActors());
            return Ok(acters);
        }
        
        
        [HttpGet("withMovies/{id}")] // GET: /api/acters
        //[Route("api/acters/withMovies/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActerViewModel>))]  
        [ProducesResponseType(404)]
        public IActionResult GetActerWithMovies(int id)
        {
            var acter = _mapper.Map<EditActerViewModel>(_service.GetActor(id));
            if (acter == null) return NotFound();
            acter.SelectMovies = _mapper.Map<IEnumerable<MovieDto>,IEnumerable<ActerMovieViewModel>>
                (_service.GetAllMoviesByActorId(id));
            return Ok(acter);
        }
        
        [HttpGet("{id}")] // GET: /api/acters/5
        [ProducesResponseType(200, Type = typeof(ActerViewModel))]  
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            var acter = _mapper.Map<ActerViewModel>(_service.GetActor(id));
            if (acter == null) return NotFound();  
            return Ok(acter);
        }
        
        [HttpPost] // POST: api/acters
        public ActionResult<InputActerViewModel> PostActer(InputActerViewModel inputModel)
        {

            var addedActer = _service.AddActor(_mapper.Map<InputActerViewModel, ActerDto>(inputModel));

            return CreatedAtAction("GetById", new { id = acter.Id }, _mapper.Map<InputActerViewModel>(inputModel));
        }
        
        [HttpPut("{id}")] // PUT: api/acters/5
        public IActionResult UpdateActer(int id, EditActerViewModel editModel)
        {
            try
            {
                var acter = _mapper.Map<Acter>(editModel);
                acter.Id = id;
                
                _context.Update(acter);
                _context.SaveChanges();
                
                return Ok(_mapper.Map<EditActerViewModel>(acter));
            }
            catch (DbUpdateException)
            {
                if (!ActerExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
        }
        
        [HttpDelete("{id}")] // DELETE: api/acters/5
        public ActionResult<DeleteActerViewModel> DeleteActer(int id)
        {
            var acter = _context.Acters.Find(id);
            if (acter == null) return NotFound();  
            _context.Acters.Remove(acter);
            _context.SaveChanges();
            return Ok(_mapper.Map<DeleteActerViewModel>(acter));
        }

        private bool ActerExists(int id)
        {
            return _context.Acters.Any(e => e.Id == id);
        }
    }
}