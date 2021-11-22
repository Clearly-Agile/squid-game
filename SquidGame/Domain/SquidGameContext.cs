using Microsoft.EntityFrameworkCore;

namespace SquidGame.Domain
{
    public class SquidGameContext : DbContext
    {
        public SquidGameContext(DbContextOptions<SquidGameContext> options) : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }
    }
}
