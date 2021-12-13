using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Filters;
using MoviesApp.Models;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public partial class ActersController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IActorService _actorService;
        private readonly IActorMovieService _actorMovieService;        
        private readonly ILogger<ActersController> _logger;
        private readonly IMapper _mapper;

        public ActersController(IActorMovieService actorMovieService, IActorService actorService, IMovieService movieService, ILogger<ActersController> logger, IMapper mapper)
        {
            _actorMovieService = actorMovieService;
            _movieService = movieService;
            _actorService = actorService;
            _logger = logger;            
            _mapper = mapper;
        }
        
        // GET: Acters
        [HttpGet]
        public IActionResult Index()
        {
            var actors = _mapper.Map<IEnumerable<ActerDto>, IEnumerable<ActerViewModel>>
                (_actorService.GetAllActors());
            return View(actors.ToList());
        }
        
        // GET: Acter/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<ActerViewModel>(_actorService.GetActor((int) id));
            if (viewModel == null)
            {
                return NotFound();
            }

            ViewBag.ActerMoviesSelectedList = _mapper.Map<IEnumerable<MovieDto>, IEnumerable<InputMovieViewModel>>
                (_movieService.GetAllMoviesByActorId((int) id));

            return View(viewModel);
        }
        
        // GET: Acters/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActorAgeFilter]
        public IActionResult Create([Bind("Name, LastName, BirthdayDate")] InputActerViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                _actorService.AddActor(_mapper.Map <ActerDto>(inputModel));
                return RedirectToAction(nameof(Index));
            }
            return View(inputModel);
        }
        
        // GET: Acters/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var editModel = _mapper.Map<EditActerViewModel>(_actorService.GetActor((int) id));

            if (editModel == null)
            {
                return NotFound();
            }

            
            editModel.SelectMovies = _mapper.Map<IEnumerable<ActerMovieViewModel>>(_movieService.GetActersNotFilmedFilmsByActerId((int) id));
            
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActorAgeFilter]
        public IActionResult Edit(int id, [Bind("Name, LastName, BirthdayDate,IsDeleteAllMovies")] EditActerViewModel editModel, List<int> movieId)
        {
            if (ModelState.IsValid)
            {
                var actor = _mapper.Map<EditActerViewModel, ActerDto>(editModel);
                actor.Id = id;
                var result = _actorService.UpdateActor(actor);
                
                if (result == null)
                {
                    return NotFound();
                }
                
                if (editModel.IsDeleteAllMovies)
                {
                    _actorMovieService.DeleteAllMoviesWhereFilmedActorByActorId(id);

                }

                if (movieId.Count > 0)
                {
                    List<ActerMovieDto> links = new List<ActerMovieDto>();
                    foreach (var m in movieId)
                    {
                        links.Add(new ActerMovieDto(){ActerId = id, MovieId = m});
                    }

                    _actorMovieService.AddActerMovieLinks(links);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(editModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deleteModel = _mapper.Map<ActerDto, DeleteActerViewModel>(_actorService.GetActor((int) id));

            if (deleteModel == null)
            {
                return NotFound();
            }

            return View(deleteModel);
        }
        
        // POST: Acters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var actor = _actorService.DeleteActor(id);
            if (actor==null)
            {
                return NotFound();
            }
            
            _actorMovieService.DeleteAllMoviesWhereFilmedActorByActorId(id);
            
            _logger.LogError($"Acter with id {actor.Id} has been deleted!");
            
            return RedirectToAction(nameof(Index));
        }
    }
}