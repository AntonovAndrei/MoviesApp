using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;

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
            return View(_context.Acters.Select(new => A))
        }
        
        

    }
}