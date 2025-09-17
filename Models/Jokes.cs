using Microsoft.AspNetCore.Identity;

namespace JudgeYourJokes.Models
{
    public class Jokes
    {
        public int Id { get; set; }
        public string? JokeTitle { get; set; }
        public string? UserID { get; set; } //foreign key
        public IdentityUser? User { get; set; } //navigation property
        public string? JokeDescription{ get; set; }
        public int Votes { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
