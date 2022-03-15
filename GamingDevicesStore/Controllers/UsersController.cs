using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GamingDevicesStore.Models;
using GamingDevicesStore.Dtos.User;
using GamingDevicesStore.Repositories.Interfaces;
using AutoMapper;

namespace GamingDevicesStore.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserControllable _repo;
        private readonly IMapper _mapper;

        public UsersController(IUserControllable repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsersAsync()
        {
            IEnumerable<User> users = await _repo.GetAllAsync();
            var readDtos = _mapper.Map<IEnumerable<UserReadDto>>(users);

            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetUserAsync(Guid id)
        {
            User? user = await _repo.GetByIdAsync(id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            var readDto = _mapper.Map<UserReadDto>(user);

            return Ok(readDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserReadDto>> RegisterUserAsync(UserRegisterDto registerDto)
        {
            User user = _mapper.Map<User>(registerDto);

            _repo.GeneratePasswordHash(registerDto.Password, out byte[] hash, out byte[] salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            _repo.Add(user);
            await _repo.SaveChangesAsync();

            var readDto = _mapper.Map<UserReadDto>(user);

            return CreatedAtAction(nameof(GetUserAsync), new { Id = readDto.Id }, readDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserWithTokenReadDto>> LoginUserAsync(UserLoginDto loginDto)
        {
            User? user = await _repo.GetByNameAsync(loginDto.Username);

            if (user is null)
            {
                return NotFound("User not found");
            }

            if (!_repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                return BadRequest("Incorrect password");
            }

            string token = _repo.GenerateToken(user);
            var readDto = _mapper.Map<UserWithTokenReadDto>(user) with { Token = token };

            return Ok(readDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateUserAsync(Guid id, UserUpdateDto updateDto)
        {
            User? user = await _repo.GetByIdAsync(id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            _mapper.Map(updateDto, user);
            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            User? user = await _repo.GetByIdAsync(id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            _repo.Remove(user);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
