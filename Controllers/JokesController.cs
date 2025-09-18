using JudgeYourJokes.Data;
using Microsoft.AspNetCore.Mvc;
using JudgeYourJokes.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JudgeYourJokes.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public JokesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var jokes = _context.Jokes.ToList();
            return View(jokes);
        }

        //Get: Jokes/Create
        public IActionResult Create()
        {
            return View();
        }


        //Post: Jokes/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Jokes joke)
        {
           joke.UserID = _userManager.GetUserId(User);  
           joke.Date = DateTime.Now;
           joke.Votes = 0;

           _context.Jokes.Add(joke);
           await _context.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
        }
    }
}
