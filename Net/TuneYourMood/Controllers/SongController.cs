using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TuneYourMood.Api.PostModels;
using TuneYourMood.Core.DTOs;
using TuneYourMood.Core.InterfaceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TuneYourMood.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController(IService<SongDto> songService, IMapper mapper) : ControllerBase
    {
        readonly IService<SongDto> _songService = songService;
        private readonly IMapper _mapper = mapper;

        // GET: api/<SongController>
        [HttpGet]
        public async Task<List<SongDto>> Get()
        {
            return (List<SongDto>)await _songService.getallAsync();
        }


        // GET api/<SongController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SongDto>> Get(int id)
        {
            if (await _songService.getByIdAsync(id) == null)
                return NotFound();
            return Ok(_songService.getByIdAsync(id));
        }


        // POST api/<SongController>
        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] SongPostModel song)
        {
            var songDto = _mapper.Map<SongDto>(song);
            songDto = await _songService.addAsync(songDto);
            return Ok(songDto);

        }

        // PUT api/<SongController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] SongPostModel song)
        {
            var songDto = _mapper.Map<SongDto>(song);
            songDto = await _songService.updateAsync(id, songDto);
            return Ok(songDto);
        }

        // DELETE api/<SongController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _songService.deleteAsync(id))
                return NotFound();
            return Ok();
        }
    }
}
