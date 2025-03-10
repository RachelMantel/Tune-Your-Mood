using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TuneYourMood.Core.DTOs;
using TuneYourMood.Core.InterfaceService;
using TuneYourMood.Core.Entities;

using TuneYourMood.Api.PostModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TuneYourMood.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IMapper mapper) : ControllerBase
    {
        readonly IUserService _userService=userService;
        private readonly IMapper _mapper=mapper;

        // GET: api/<UserController>
        [HttpGet]
        public async Task<List<UserDto>> Get()
        {
            return (List<UserDto>)await _userService.getallAsync();
        }

        // GET: api/<UserController>
        [HttpGet("songs/{userId}")]
        public  List<SongDto> GetSongsByUserId(int userId)
        {
            return _userService.GetSongsByUserId(userId);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            if (await _userService.getByIdAsync(id) == null)
                return NotFound();
            return Ok(_userService.getByIdAsync(id));
        }


        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] UserPostModel user)
        {
            var userDto = _mapper.Map<UserDto>(user);
            userDto = await _userService.addAsync(userDto);
            return Ok(userDto);

        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UserPostModel user)
        {
            var userDto = _mapper.Map<UserDto>(user);
            userDto = await _userService.updateAsync(id, userDto);
            return Ok(userDto);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _userService.deleteAsync(id))
                return NotFound();
            return Ok();
        }
    }
}
