

using GameWebAPI.Model;

namespace GameWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();

        }


        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //добавим стартовые значения для бд
            modelBuilder.Entity<Genre>().HasData( 
                new Genre { Id =1,Name = "Action" },
                new Genre { Id = 2, Name = "Rpg" },
                new Genre { Id = 3, Name = "Survival" },
                new Genre { Id = 4, Name = "Horror" },
                new Genre { Id = 5, Name = "Adventure" }

            );
        }
        // public DbSet<GenresGame> GenresGames { get; set; }

    }
}
