using AutoMapper;
using Core.Dtos;
using Core.Dtos.UserDtos;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public UsersController(
            IUserService userService,
            IPasswordService passwordService,
            IMapper mapper)
        {
            _userService = userService;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<PageDto<UserReadDto>>> GetAsync([FromQuery] PageParameters pageParams)
        {
            var users = await _userService.GetAllAsync(pageParams.PageNumber, pageParams.PageSize);
            var pageDto = _mapper.Map<PageDto<UserReadDto>>(users);

            return Ok(pageDto);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<UserReadDto>> GetAsync([FromRoute] int id)
        {
            var user = await _userService.GetByIdAsync(id);
            var readDto = _mapper.Map<UserReadDto>(user);

            return Ok(readDto);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserReadDto>> RegisterAsync([FromBody] UserRegisterDto registerDto)
        {
            _passwordService.CreatePasswordHash(registerDto.Password!, out byte[] hash, out byte[] salt);

            User user = await _userService.ConstructUserAsync(registerDto.Username!, hash, salt, registerDto.ProfilePicture);
            await _userService.CreateAsync(user);

            var readDto = _mapper.Map<UserReadDto>(user);

            return CreatedAtAction(nameof(GetAsync), new { Id = readDto.Id }, readDto);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserWithTokenReadDto>> LoginAsync([FromBody] UserLoginDto loginDto)
        {
            var user = await _userService.GetByNameAsync(loginDto.Username!);
            _passwordService.VerifyPasswordHash(loginDto.Password!, user.PasswordHash!, user.PasswordSalt!);

            string token = _passwordService.CreateToken(user);
            var readDto = _mapper.Map<UserWithTokenReadDto>(user) with { Token = token };

            return Ok(readDto);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] UserBaseDto updateDto)
        {
            var user = await _userService.GetByIdAsync(id);

            _userService.VerifyUserAccessRights(user, User.Identity!, User.Claims!);
            _mapper.Map(updateDto, user);
            await _userService.UpdateAsync(user);

            return NoContent();
        }

        [HttpPut("{id:int:min(1)}/changePassword")]
        public async Task<ActionResult<string>> ChangePasswordAsync(
            [FromRoute] int id,
            [FromBody] UserChangePasswordDto changePasswordDto)
        {
            var player = await _userService.GetByIdAsync(id);

            _userService.VerifyUserAccessRights(player, User.Identity!, User.Claims!);
            _passwordService.CreatePasswordHash(changePasswordDto.Password!, out byte[] hash, out byte[] salt);
            _userService.ChangePasswordData(player, hash, salt);
            await _userService.UpdateAsync(player);

            string token = _passwordService.CreateToken(player);

            return Ok(token);
        }

        [HttpPut("{id:int:min(1)}/changeRole")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> ChangeRoleAsync([FromRoute] int id, [FromBody] UserChangeRoleDto changeRoleDto)
        {
            var user = await _userService.GetByIdAsync(id);

            _mapper.Map(changeRoleDto, user);
            await _userService.UpdateAsync(user);

            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var user = await _userService.GetByIdAsync(id);

            _userService.VerifyUserAccessRights(user, User.Identity!, User.Claims!);
            await _userService.DeleteAsync(user);

            return NoContent();
        }
    }
}
