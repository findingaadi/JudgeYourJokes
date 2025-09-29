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

            var vote = _context.Votes.FirstOrDefault(v => v.JokeId == id && v.UserId == userId);

            var joke = await _context.Jokes.FindAsync(id);

            if (vote == null && joke != null)
            {
                joke.Votes += 1;
                _context.Votes.Add(new Vote { JokeId = id, UserId = userId });
            }
            else if (vote != null && joke != null)
            {
                joke.Votes -= 1;
                _context.Votes.Remove(vote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Jokes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var joke = await _context.Jokes.FirstOrDefaultAsync(j => j.Id == id && j.UserID == userId);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        // POST: Jokes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Jokes editedJoke)
        {
            var userId = _userManager.GetUserId(User);
            var joke = await _context.Jokes.FirstOrDefaultAsync(j => j.Id == id && j.UserID == userId);
            if (joke == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                joke.JokeTitle = editedJoke.JokeTitle;
                joke.JokeDescription = editedJoke.JokeDescription;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyJokes));
            }
            return View(editedJoke);
        }

        [Authorize]
        public IActionResult MyJokes()
        {
            var userId = _userManager.GetUserId(User);
            var jokes = _context.Jokes
                .Where(j => j.UserID == userId)
                .ToList();
            return View(jokes);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var joke = await _context.Jokes.FirstOrDefaultAsync(j => j.Id == id && j.UserID == userId);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var joke = await _context.Jokes.FirstOrDefaultAsync(j => j.Id == id && j.UserID == userId);
            if (joke == null)
            {
                return NotFound();
            }
            _context.Jokes.Remove(joke);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyJokes));
        }

    }

}
