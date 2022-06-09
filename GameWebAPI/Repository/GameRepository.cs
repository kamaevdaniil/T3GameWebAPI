using Microsoft.EntityFrameworkCore;
using GameWebAPI.Model;

namespace GameWebAPI.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }

        public Task<List<Game>> Get() => _context.Games.Include(x => x.Genres).ToListAsync();


        public async Task<Game> GetById(int id)
        {
           var game =  await _context.Games.Include(x=>x.Genres).SingleAsync(g=>g.Id == id);
            return game;
        }
            
        public async Task AddGame(Game game) => await _context.Games.AddAsync(game);


        public async  Task  UpdateGame(Game game)
        {
            var GameFromDb = await _context.Games.Include(x=>x.Genres).SingleAsync(g => g.Id == game.Id);
            if  (GameFromDb == null) return;
            GameFromDb.Name = game.Name;
            GameFromDb.StudioDeveloper = game.StudioDeveloper;
            GameFromDb.Genres = game.Genres;
        }

        public async Task DeleteGame(int id)
        {
            var gameFromDb = await _context.Games.FindAsync(new object[] { id });
            if(gameFromDb == null) return;
            _context.Games.Remove(gameFromDb);
        }
       
        public async Task SaveAsync()=>await _context.SaveChangesAsync();

        private bool _dispose = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_dispose)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _dispose = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

 
       
    }
}
