using Microsoft.AspNetCore.Identity;

namespace JudgeYourJokes.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public int JokeId { get; set; }
        public Jokes? Joke { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; } 
    }
}
