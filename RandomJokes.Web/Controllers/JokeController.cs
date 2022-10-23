using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RandomJokes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RandomJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        private readonly string _ConnString;
        public JokeController(IConfiguration con)
        {
            _ConnString = con.GetConnectionString("ConStr");
        }
        [HttpGet]
        [Route("getjoke")]
        public Joke GetJoke()
        {
            var repo = new JokeRepo(_ConnString);
            using var client = new HttpClient();
            var json = client.GetStringAsync("https://jokesapi.lit-projects.com/jokes/programming/random").Result;
            var joke = JsonConvert.DeserializeObject<List<Joke>>(json).FirstOrDefault();

            joke = repo.AddJoke(joke);
            joke.UserLikedJokes = repo.GetLikes(joke.Id);
            return joke;

        }
        [Route("getjokes")]
        [HttpGet]
        public List<Joke> GetJokes()
        {
            var repo = new JokeRepo(_ConnString);
            return repo.GetJokes();
        }
        [Route("getlikes")]
        [HttpGet]
        public List<UserLikedJoke> GetLikes(int id)
        {
            var repo = new JokeRepo(_ConnString);
            return repo.GetLikes(id);
        }
        [Authorize]
        [Route("addlike")]
        [HttpPost]
        public void AddLike(Joke joke)
        {
            var repo = new JokeRepo(_ConnString);
            repo.AddLike(joke, repo.GetByEmail(User.Identity.Name).Id);
        }
        [Authorize]
        [Route("adddislike")]
        [HttpPost]
        public void AdDislike(Joke joke)
        {
            var repo = new JokeRepo(_ConnString);
            repo.AddDislike(joke, repo.GetByEmail(User.Identity.Name).Id);
        }
        [Route("ifliked")]
        [HttpPost]
        public void IfLiked(int jokeId)
        {
            var repo = new JokeRepo(_ConnString);
            repo.IfLiked(jokeId, repo.GetByEmail(User.Identity.Name).Id);
        }
    }
}
