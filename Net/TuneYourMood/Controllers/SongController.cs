using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TuneYourMood.Api.PostModels;
using TuneYourMood.Core.DTOs;
using TuneYourMood.Core.InterfaceService;

namespace TuneYourMood.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // דורש אימות בברירת מחדל לכל הפעולות
    public class SongController(IService<SongDto> songService, IMapper mapper) : ControllerBase
    {
        private readonly IService<SongDto> _songService = songService;
        private readonly IMapper _mapper = mapper;

        // GET: api/Song - שליפת כל השירים (זמין לכל משתמש מחובר)
        [HttpGet]
        public async Task<ActionResult<List<SongDto>>> Get()
        {
            return Ok(await _songService.getallAsync());
        }

        // GET api/Song/{id} - שליפת שיר לפי ID
        [HttpGet("{id}")]
        public async Task<ActionResult<SongDto>> Get(int id)
        {
            var song = await _songService.getByIdAsync(id);
            if (song == null)
                return NotFound();

            return Ok(song);
        }

        // POST api/Song
        [HttpPost]
        public async Task<ActionResult<SongDto>> Post([FromBody] SongPostModel song)
        {
            var songDto = _mapper.Map<SongDto>(song);
            songDto = await _songService.addAsync(songDto);
            return Ok(songDto);
        }

        // PUT api/Song/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put(int id, [FromBody] SongPostModel song)
        {
            var songDto = _mapper.Map<SongDto>(song);
            songDto = await _songService.updateAsync(id, songDto);
            return Ok(songDto);
        }

        // DELETE api/Song/{id} 
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _songService.deleteAsync(id))
                return NotFound();
            return Ok();
        }
    }
}
