using Microsoft.AspNetCore.Mvc;
using GameWebAPI.Model;

namespace GameWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly DataContext _context;

        public GenreController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Genre>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Getters")]
        [Produces("application/json")]
        public async Task<ActionResult<List<Genre>>> GetAll()
        {
            return Ok(await _context.Genres.ToListAsync());
        }

        [HttpGet("GamesByGenre")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Getters")]
        [Produces("application/json")]
        public async Task<ActionResult<List<string>>> GetGenres(int id)
        {
            List<string> listGameName = new List<string>();

            var DblistGame = await _context.Games.Include(x=>x.Genres).ToListAsync();

            //var selectedName = from x in DblistGame where x.Id == id select x.Name;
            foreach (var item in DblistGame)
            {
                if (item.Genres.Where(x => x.Id == id).Any())
                {
                    listGameName.Add(item.Name);
                } 

            }


            return Ok(listGameName);
        }





    }
}
