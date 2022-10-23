using System.Collections.Generic;

namespace RandomJokes.Data
{
    public class Joke
    {
        public int Id { get; set; }
        public string Setup { get; set; }
        public string Punchline { get; set; }
        public List<UserLikedJoke> UserLikedJokes { get; set; }

    }
}
