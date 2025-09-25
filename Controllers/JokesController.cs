using JudgeYourJokes.Data;
using Microsoft.AspNetCore.Mvc;
using JudgeYourJokes.Models;
using Microsoft.EntityFrameworkCore;
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
            var jokes = _context.Jokes.Include(j => j.User).ToList();
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upvote(int id)
        {
            var userId = _userManager.GetUserId(User);

            bool alreadyVoted = _context.Votes.Any(v => v.JokeId == id && v.UserId == userId);

            if (!alreadyVoted)
            { 
                var joke = await _context.Jokes.FindAsync(id);
                if (joke != null)
                {
                    joke.Votes += 1;
                    var vote = new Vote
                    {
                        JokeId = id,
                        UserId = userId
                    };
                    _context.Votes.Add(vote);

                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
