using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomJokes.Data
{
    public class JokeRepo
    {
        private readonly string _connectionString;
        public JokeRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Joke AddJoke(Joke joke)
        {
            using var ctx = new JokesDataContext(_connectionString);
            if(!JokeAlreadyExist(joke))
            {
                joke.Id = 0;
                ctx.Jokes.Add(joke);
                ctx.SaveChanges();
            }
            else
            {
                joke = ctx.Jokes.FirstOrDefault(a => a.Setup == joke.Setup);
            }
            return joke;

        }
        public List<Joke> GetJokes()
        {
            using var ctx = new JokesDataContext(_connectionString);
            var jokes = ctx.Jokes.ToList();
            foreach(var j in jokes )
            {
                j.UserLikedJokes = GetLikes(j.Id);
            }
            return jokes.Where(x => x.UserLikedJokes.Count() > 0).ToList();
        }
        public void AddLike(Joke joke, int userId )
        {
            using var ctx = new JokesDataContext(_connectionString);
            var alreadyLiked = ctx.UserLikedJokes.FirstOrDefault(u => u.UserId == userId && u.JokeId == joke.Id);
            if(alreadyLiked!=null)
            {
                ctx.Database.ExecuteSqlInterpolated($"update UserLikedJokes set liked = {!alreadyLiked.Liked} where UserId={userId} and JokeId = {joke.Id}");
            }
            else
            {
                ctx.UserLikedJokes.Add(new UserLikedJoke
                {
                    UserId = userId,
                    JokeId = joke.Id,
                    Date = DateTime.Now,
                    Liked = true
                });
            }
            ctx.SaveChanges();
        }
        public void AddDislike(Joke joke, int userId)
        {
            using var ctx = new JokesDataContext(_connectionString);
            var alreadyLiked = ctx.UserLikedJokes.FirstOrDefault(u => u.UserId == userId && u.JokeId == joke.Id);
            if (alreadyLiked != null)
            {
                ctx.Database.ExecuteSqlInterpolated($"update UserLikedJokes set liked = {!alreadyLiked.Liked} where UserId={userId} and JokeId = {joke.Id}");
            }
            else
            {
                ctx.UserLikedJokes.Add(new UserLikedJoke
                {
                    UserId = userId,
                    JokeId = joke.Id,
                    Date = DateTime.Now,
                    Liked = false
                });
            }
            ctx.SaveChanges();
        }
        public object IfLiked(int jokeId, int userId)
        {
            using var ctx = new JokesDataContext(_connectionString);
            var like = ctx.UserLikedJokes.FirstOrDefault(u => u.UserId == userId && u.JokeId == jokeId);
            if (like != null)
            {
                if (like.Date < DateTime.Now.AddMinutes(5))
                {
                    return new { liked = true, disliked = true };
                }
                return new { liked = like.Liked, disliked = like.Liked };
            }
            else
            {
                return new { liked = false, disliked = false };
            }
        }

        public bool JokeAlreadyExist(Joke joke)
        {
            using var ctx = new JokesDataContext(_connectionString);
            return ctx.Jokes.Any(s => s.Setup == joke.Setup);
        }
        public List<UserLikedJoke> GetLikes(int jokeId)
        {
            using var ctx = new JokesDataContext(_connectionString);
            return ctx.UserLikedJokes.Where(x => x.JokeId == jokeId).ToList();
        }
        public void AddUser(User user, string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;
            using var ctx = new JokesDataContext(_connectionString);
            ctx.Users.Add(user);
            ctx.SaveChanges();
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            var isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValidPassword)
            {
                return null;
            }

            return user;

        }

        public User GetByEmail(string email)
        {
            using var ctx = new JokesDataContext(_connectionString);
            return ctx.Users.FirstOrDefault(u => u.Email == email);
        }
      
    }
}

