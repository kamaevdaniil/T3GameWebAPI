using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameWebAPI.Model;
using GameWebAPI.Repository;
using System.Net.Mime;

namespace GameWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class GameController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IGameRepository _repository;

        public GameController(DataContext context,IGameRepository repository)
        {
            _context = context;
            _repository = repository;
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Game>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Getters")]
        [Produces("application/json")]
        public async Task<ActionResult<List<Game>>> Get()
        {
            
            var listGames = await _repository.Get();
            if(listGames == null)
            {
                return NotFound("Пусто");
            }
            

            return Ok(listGames);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Game>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Getters")]
        [Produces("application/json")]
        public async Task<ActionResult<List<Game>>> GetById(int id)
        {

            var game = await _repository.GetById(id);
            //var rez = game.Find(x => x.Id == id);
            if (game == null)
            {
                return BadRequest("Game not Found");
            }
            return Ok(game);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Tags("Creators")]
        [Produces("application/json")]
        public async Task<ActionResult<Game>> AddGame(Game game)
        {
            //Deserialize json in object
            //var  jsonGame = JsonSerializer.Serialize(game);
            List<Genre> listGenre = new List<Genre>();
            ICollection<Genre>? gameGenre = game.Genres;//client

            List<Genre> currentGenre = new List<Genre>();
            List<Genre> newGenre = new List<Genre>();

            var dbGenre = await _context.Genres.ToListAsync();
            foreach (var gen in gameGenre)
            {   //Если нет такого жанра
                if (dbGenre.Where(dbG => dbG.Name == gen.Name).FirstOrDefault()==null)
                {
                    listGenre.Add(gen);
                  
                }//Если есть то удаляем обьект из game 
                else
                {
                    //Поиск сущ жанра и добавление в коллекцию
                    var objGen = dbGenre.Find(x =>x.Name==gen.Name);
                    currentGenre.Add(gen);
                    newGenre.Add(objGen);
                    
                }
                
            }
            foreach (var item in currentGenre)
            {
                game.Genres.Remove(item);
            }

            foreach (var item in newGenre)
            {
                game.Genres.Add(item);
            }

           

            await _context.Genres.AddRangeAsync(listGenre);
            await _context.SaveChangesAsync();
          
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return Ok(await _context.Games.ToListAsync());
        }

        [HttpPut("Up")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Tags("Updaters")]
        [Produces("application/json")]
        public async Task<ActionResult<Game>> UpdateGame(Game gameNew)
        {
           

            //List<int> i = new List<int>();
            List<int> newIdGenre = gameNew.Genres.Select(g =>g.Id).ToList();

            var Db =  _context.Games.Include(x=>x.Genres).Single(g =>g.Id ==gameNew.Id);
                
            var newGenres = _context.Genres.Where(g => newIdGenre.Contains(g.Id));
            //var oDbgame = Db.Find(x=>x.Id==gameNew.Id);

            Db.Name = gameNew.Name;
            Db.StudioDeveloper = gameNew.StudioDeveloper;
            Db.Id = gameNew.Id;

            Db.Genres.Clear();

            foreach (var item in newGenres)
            {
                Db.Genres.Add(item);
            }
            _context.SaveChanges();
            
            return Ok(Db);
            

        }

       
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Tags("Deleters")]
        [Produces("application/json")]
        public async Task<ActionResult<List<Game>>> DeleteGame(int id)
        {

            try
            {
                await _repository.DeleteGame(id);
                await _repository.SaveAsync();
            }
            catch(DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();

           
        }



    }
}
