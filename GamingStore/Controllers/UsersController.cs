using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Security.Claims;
using GamingStore.Data;
using GamingStore.Models;
using GamingStore.Dtos.User;
using GamingStore.Repositories.Interfaces;
using AutoMapper;

namespace GamingStore.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserControllable _repo;
        private readonly IMapper _mapper;
        private static readonly JsonSerializerOptions serializerOptions = new()
        {
            WriteIndented = true,
            Converters =
            {
                new ByteArrayConverter()
            }
        };

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

            await SetDefaultProfilePicture(user);
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
            List<User> users = await _repo.GetByNameAsync(loginDto.Username);

            if (users.Count == 0)
            {
                return NotFound("User not found");
            }

            foreach (User user in users)
            {
                if (_repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash!, user.PasswordSalt!))
                {
                    string token = _repo.GenerateToken(user);
                    var readDto = _mapper.Map<UserWithTokenReadDto>(user) with { Token = token };

                    return Ok(readDto);
                }
            }

            return BadRequest("Incorrect password");
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

            if (!IsAdminOrOwner(user))
            {
                return Forbid();
            }

            _mapper.Map(updateDto with { ProfilePicture = null! }, user);
            var profilePicture = JsonSerializer.Deserialize<byte[]>(updateDto.ProfilePicture, serializerOptions);
            user.ProfilePicture = profilePicture;

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

            if (!IsAdminOrOwner(user))
            {
                return Forbid();
            }

            _repo.Remove(user);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        private bool IsAdminOrOwner(User user)
        {
            if (User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role 
                && c.Value == UserRole.Admin.ToString()) is not null
                || User.Identity!.Name == user.Username)
            {
                return true;
            }

            return false;
        }

        private async Task SetDefaultProfilePicture(User user)
        {
            string picPath = _repo.GetDefaultPicturePath();

            using (FileStream stream = new(picPath, FileMode.Open, FileAccess.Read))
            {
                user.ProfilePicture = await System.IO.File.ReadAllBytesAsync(picPath);
                await stream.ReadAsync(user.ProfilePicture, 0, Convert.ToInt32(stream.Length));
            }
        }
    }
}
