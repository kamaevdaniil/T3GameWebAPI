using GameWebAPI.Model;

namespace GameWebAPI.Repository
{
    public interface IGameRepository:IDisposable
    {
        Task<List<Game>> Get();
        Task<Game> GetById(int id);
        Task AddGame(Game game);
        Task UpdateGame(Game game);
        Task DeleteGame(int id);
        Task SaveAsync();


    }
}
