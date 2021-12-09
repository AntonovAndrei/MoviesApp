using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Filters;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class MoviesController: Controller
    {
        private readonly MoviesContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly Mapper _mapper;


        public MoviesController(MoviesContext context, ILogger<HomeController> logger, Mapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: Movies
        [HttpGet]
        public IActionResult Index()
        {
            var movies = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(_context.Movies.ToList());
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

            var viewModel = _mapper.Map<Movie, MovieViewModel>(_context.Movies.Where(m => m.Id == id).FirstOrDefault());
            
            if (viewModel == null)
            {
                return NotFound();
            }
            
            var movieActersList = _context.ActerMovies.Where(m => m.MovieId == id)
                .Select(m => new InputActerViewModel()
                {
                    Name = m.Acter.Name,
                    LastName = m.Acter.LastName,
                    BirthdayDate = m.Acter.BirthdayDate,
                })
                .ToList();
            ViewBag.MovieActersSelectedList = movieActersList;

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
                _context.Add(new Movie
                {
                    Genre = inputModel.Genre,
                    Price = inputModel.Price,
                    Title = inputModel.Title,
                    ReleaseDate = inputModel.ReleaseDate
                });
                _context.SaveChanges();

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

            var editModel = _context.Movies.Where(m => m.Id == id).Select(m => new EditMovieViewModel
            {
                Genre = m.Genre,
                Price = m.Price,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate
            }).FirstOrDefault();
            
            if (editModel == null)
            {
                return NotFound();
            }
            
            var movieActerList = _context.ActerMovies.Where(a => a.MovieId == id).Select(m => new MovieActerViewModel()
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
            foreach (var a in movieActerList)
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


            ViewBag.ActersSelectList = new SelectList(actersList, "Id", "Name");

            return View(editModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureReleaseDateBeforeNow]
        public IActionResult Edit(int id, [Bind("Title,ReleaseDate,Genre,Price,IsDeleteAllActer")] EditMovieViewModel editModel, int[] acterId)
        {
            if (editModel.IsDeleteAllActer)
            {
                var deleteActer = _context.ActerMovies.Where(m => m.MovieId == id).ToList();
                foreach (var am in deleteActer)
                {
                    _context.Remove(am);
                }

                _context.SaveChanges();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var movie = new Movie
                    {
                        Id = id,
                        Genre = editModel.Genre,
                        Price = editModel.Price,
                        Title = editModel.Title,
                        ReleaseDate = editModel.ReleaseDate
                    };
                    
                    _context.Update(movie);
                    _context.SaveChanges();
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

                if (acterId.Length > 0)
                {
                    foreach (var a in acterId)
                    {
                        _context.Add(new ActerMovie
                        {
                            MovieId = id, 
                            ActerId = a
                        });
                    }

                    _context.SaveChanges();
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