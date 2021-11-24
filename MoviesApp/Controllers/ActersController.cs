using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class ActersController : Controller
    {
        private readonly MoviesContext _context;
        //спросить про MoviesController ILogger
        private readonly ILogger<ActersController> _logger;

        public ActersController(MoviesContext context, ILogger<ActersController> logger)
        {
            this._context = context;
            this._logger = logger;

        }
        
        // GET: Acters
        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Acters.Select(a => new ActerViewModel
            {
                Id = a.Id,
                Name = a.Name,
                LastName = a.LastName,
                BirthdayDate = a.BirthdayDate
            }).ToList());
        }
        
        // GET: Acter/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = _context.Acters
                .Where(a => a.Id == id)
                .Select(a => new ActerViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    LastName = a.LastName,
                    BirthdayDate = a.BirthdayDate
                }).FirstOrDefault();
            if (viewModel == null)
            {
                return NotFound();
            }

            var acterMoviesList = _context.ActerMovies.Where(m => m.ActerId == id)
                .Select(m => new InputMovieViewModel()
                {
                    Genre = m.Movie.Genre,
                    Price = m.Movie.Price,
                    Title = m.Movie.Title,
                    ReleaseDate = m.Movie.ReleaseDate
                })
                .ToList();
            ViewBag.ActerMoviesSelectedList = acterMoviesList;

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
        public IActionResult Create([Bind("Name, LastName, BirthdayDate")] InputActerViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                _context.Acters.Add(new Acter
                {
                    Name = inputModel.Name,
                    LastName = inputModel.LastName,
                    BirthdayDate = inputModel.BirthdayDate
                });

                _context.SaveChanges();
                return RedirectToAction((nameof(Index)));
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

            var editModel = _context.Acters
                .Where(a => a.Id == id)
                .Select(a => new EditActerViewModel
                {
                    Name = a.Name,
                    LastName = a.LastName,
                    BirthdayDate = a.BirthdayDate
                }).FirstOrDefault();
            if (editModel == null)
            {
                return NotFound();
            }

            
            var acterMovieList = _context.ActerMovies.Where(a => a.ActerId == id).Select(m => new ActerMovieViewModel
            {
                Id = m.Movie.Id,
                Title = m.Movie.Title
            }).ToList();

            var moviesList = _context.Movies
                .Select(m => new ActerMovieViewModel
            {
                Id = m.Id,
                Title = m.Title
            }).ToList();

            //Спросить как сделать по человечески
            var buffer = new ActerMovieViewModel();
            foreach (var a in acterMovieList)
            {
                foreach (var m in moviesList)
                {
                    if (a.CompareTo(m) == 0)
                    {
                        buffer = m;
                    }
                }

                if (buffer != null)
                {
                    moviesList.Remove(buffer);
                    //Console.WriteLine($"elemet deleted m {buffer.Id} - {buffer.Title}");
                    buffer = null;
                }
            }
            
            ViewBag.MoviesSelectList = new SelectList(moviesList, "Id", "Title");
            
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Name, LastName, BirthdayDate,IsDeleteAllMovies")] EditActerViewModel editModel, int[] movieId)
        {
            if (editModel.IsDeleteAllMovies)
            {
                var deleteMovie = _context.ActerMovies.Where(m => m.ActerId == id).ToList();
                foreach (var am in deleteMovie)
                {
                    _context.Remove(am);
                }

                _context.SaveChanges();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var acter = new Acter
                    {
                        Id = id,
                        Name = editModel.Name,
                        LastName = editModel.LastName,
                        BirthdayDate = editModel.BirthdayDate
                    };

                    _context.Update(acter);
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (!ActerExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (movieId.Length > 0)
                {
                    foreach (var m in movieId)
                    {
                        _context.Add(new ActerMovie
                            {
                                MovieId = m, 
                                ActerId = id
                            });
                    }

                    _context.SaveChanges();
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

            var deleteModel = _context.Acters.Where(a => a.Id == id).Select(a => new DeleteActerViewModel
            {
                Name = a.Name,
                LastName = a.LastName,
                BirthdayDate = a.BirthdayDate
            }).FirstOrDefault();
            
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
            var acter = _context.Acters.Find(id);
            _context.Acters.Remove(acter);
            _context.SaveChanges();
            _logger.LogError($"Movie with id {acter.Id} has been deleted!");
            return RedirectToAction(nameof(Index));
        }
        private bool ActerExists(int id)
        {
            return _context.Acters.Any(a => a.Id == id);
        }
    }
}