using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MoviesApp.Filters;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class MoviesController: Controller
    {
        private readonly IMovieService _movieService;
        private readonly IActorService _actorService;
        private readonly IActorMovieService _actorMovieService;
        private readonly ILogger<HomeController> _logger;
        private readonly Mapper _mapper;


        public MoviesController(IActorMovieService actorMovieService, IActorService actorService, IMovieService movieService, ILogger<HomeController> logger, Mapper mapper)
        {
            _actorMovieService = actorMovieService;
            _actorService = actorService;
            _movieService = movieService;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: Movies
        [HttpGet]
        public IActionResult Index()
        {
            var movies = _mapper.Map<IEnumerable<MovieDto>, IEnumerable<MovieViewModel>>(_movieService.GetAllMovies());
            return View(movies);
        }

        // GET: Movies/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var viewModel = _mapper.Map<MovieViewModel>(_movieService.GetMovie((int) id));

            if (viewModel == null)
            {
                return NotFound();
            }
            
            ViewBag.MovieActersSelectedList = _mapper.Map<IEnumerable<ActerDto>, IEnumerable<InputActerViewModel>>
                (_actorService.GetAllActorByMovieId((int) id));

            return View(viewModel);
        }
        
        // GET: Movies/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureReleaseDateBeforeNow]
        public IActionResult Create([Bind("Title,ReleaseDate,Genre,Price")] InputMovieViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                _movieService.AddMovie(_mapper.Map<MovieDto>(inputModel));
                return RedirectToAction(nameof(Index));
            }
            return View(inputModel);
        }
        
        [HttpGet]
        // GET: Movies/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editModel = _mapper.Map<EditMovieViewModel>(_movieService.GetMovie((int) id));
            
            if (editModel == null)
            {
                return NotFound();
            }
            
            ViewBag.ActersSelectList = new SelectList(_actorService.GetNotFilmedActersByMovieId((int) id), "Id", "Name");

            return View(editModel);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureReleaseDateBeforeNow]
        public IActionResult Edit(int id, [Bind("Title,ReleaseDate,Genre,Price,IsDeleteAllActer")] EditMovieViewModel editModel, int[] acterIds)
        {
            if (editModel.IsDeleteAllActer)
            {
                _actorMovieService.DeleteAllActorsFilmedInMovieByMovieId(id);
            }
            
            if (ModelState.IsValid)
            {
                var movie = _mapper.Map<EditMovieViewModel, MovieDto>(editModel);
                movie.Id = id;
                var result = _movieService.UpdateMovie(movie);
                
                if (result == null)
                {
                    return NotFound();
                }
                
                if (acterIds.Length > 0)
                {
                    List<ActerMovieDto> links = new List<ActerMovieDto>();
                    foreach (var a in acterIds)
                    {
                        links.Add(new ActerMovieDto(){ActerId = a, MovieId = id});
                    }

                    _actorMovieService.AddActerMovieLinks(links);
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(editModel);
        }
        
        [HttpGet]
        // GET: Movies/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deleteModel = _mapper.Map<MovieDto, DeleteMovieViewModel>(_movieService.DeleteMovie((int) id));
            
            if (deleteModel == null)
            {
                return NotFound();
            }

            _actorMovieService.DeleteAllActorsFilmedInMovieByMovieId((int) id);

            return View(deleteModel);
        }
        
        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _movieService.DeleteMovie(id);
            if (movie==null)
            {
                return NotFound();
            }
            
            _actorMovieService.DeleteAllActorsFilmedInMovieByMovieId((int) id);
            
            _logger.LogTrace($"Movie with id {movie.Id} has been deleted!\n" +
                             $"");
            return RedirectToAction(nameof(Index));
        }
    }
}