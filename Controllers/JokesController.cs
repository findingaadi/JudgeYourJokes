using JudgeYourJokes.Data;
using Microsoft.AspNetCore.Mvc;
using JudgeYourJokes.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JudgeYourJokes.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;


        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var jokes = _context.Jokes.ToList();
            return View(jokes);
        }
    }
}
