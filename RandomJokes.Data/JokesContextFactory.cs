using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RandomJokes.Data
{
    public class JokesContextFactory : IDesignTimeDbContextFactory<JokesDataContext>
    {
        public JokesDataContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}RandomJokes.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new JokesDataContext(config.GetConnectionString("ConStr"));
        }
    }
}
