using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Filters;
using MoviesApp.Models;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class MoviesController: Controller
    {
        private readonly IMovieService _service;
        private readonly IActorService _actorService;
        private readonly ILogger<HomeController> _logger;
        private readonly Mapper _mapper;


        public MoviesController(IActorService actorService, IMovieService service, ILogger<HomeController> logger, Mapper mapper)
        {
            _actorService = actorService;
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: Movies
        [HttpGet]
        public IActionResult Index()
        {
            var movies = _mapper.Map<IEnumerable<MovieDto>, IEnumerable<MovieViewModel>>(_service.GetAllMovies());
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
            
            var viewModel = _mapper.Map<MovieViewModel>(_service.GetMovie((int) id));

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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureReleaseDateBeforeNow]
        public IActionResult Create([Bind("Title,ReleaseDate,Genre,Price")] InputMovieViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                _service.AddMovie(_mapper.Map<MovieDto>(inputModel));
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

            var editModel = _mapper.Map<EditMovieViewModel>(_service.GetMovie((int) id));
            
            if (editModel == null)
            {
                return NotFound();
            }
            
            ViewBag.ActersSelectList = new SelectList(_actorService.GetNotFilmedActersByMovieId((int) id), "Id", "Name");

            return View(editModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureReleaseDateBeforeNow]
        public IActionResult Edit(int id, [Bind("Title,ReleaseDate,Genre,Price,IsDeleteAllActer")] EditMovieViewModel editModel, int[] acterIds)
        {
            if (editModel.IsDeleteAllActer)
            {
                _service.DeleteAllActorsFilmedInMovies(id);
            }
            //так нужно обрабатывать все ошибки выкидываемые сервисом?
            if (ModelState.IsValid)
            {
                try
                {
                    _service.UpdateMovie(_mapper.Map<EditMovieViewModel, MovieDto>(editModel));
                }
                catch (DbUpdateException)
                {
                    if (!MovieExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (acterIds.Length > 0)
                {
                    foreach (var a in acterIds)
                    {
                        
                    }
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

            var deleteModel = _context.Movies.Where(m => m.Id == id).Select(m => new DeleteMovieViewModel
            {
                Genre = m.Genre,
                Price = m.Price,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate
            }).FirstOrDefault();
            
            if (deleteModel == null)
            {
                return NotFound();
            }

            return View(deleteModel);
        }
        
        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.Find(id);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            _logger.LogError($"Movie with id {movie.Id} has been deleted!");
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}