using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mappers.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper<User, UserReadViewModel> _readMapper;
        private readonly IMapper<PaginatedList<User>, PageViewModel<UserReadViewModel>> _paginatedMapper;
        private readonly IMapper<User, UserWithTokenReadViewModel> _readWithTokenMapper;

        public UsersController(
            IUserService userService,
            IPasswordService passwordService,
            IMapper<User, UserReadViewModel> readMapper,
            IMapper<PaginatedList<User>, PageViewModel<UserReadViewModel>> paginatedMapper,
            IMapper<User, UserWithTokenReadViewModel> readWithTokenMapper)
        {
            _userService = userService;
            _passwordService = passwordService;
            _readMapper = readMapper;
            _paginatedMapper = paginatedMapper;
            _readWithTokenMapper = readWithTokenMapper;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<PageViewModel<UserReadViewModel>>> GetAsync([FromQuery] PageParameters pageParams)
        {
            var users = await _userService.GetAllAsync(pageParams.PageNumber, pageParams.PageSize);
            var readModels = _paginatedMapper.Map(users);

            return Ok(readModels);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<UserReadViewModel>> GetAsync([FromRoute] int id)
        {
            var user = await _userService.GetByIdAsync(id);
            var readModel = _readMapper.Map(user);

            return Ok(readModel);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserReadViewModel>> RegisterAsync(
            [FromBody] UserAuthorizeViewModel authorizeModel)
        {
            _passwordService.CreatePasswordHash(authorizeModel.Password!, out byte[] hash, out byte[] salt);

            User user = _userService.ConstructUser(authorizeModel.Username!, hash, salt);
            await _userService.CreateAsync(user);

            var readModel = _readMapper.Map(user);

            return CreatedAtAction(nameof(GetAsync), new { Id = readModel.Id }, readModel);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserWithTokenReadViewModel>> LoginAsync(
            [FromBody] UserAuthorizeViewModel authorizeModel)
        {
            var user = await _userService.GetByNameAsync(authorizeModel.Username!);
            _passwordService.VerifyPasswordHash(authorizeModel.Password!, user.PasswordHash!, user.PasswordSalt!);

            string token = _passwordService.CreateToken(user);
            var readModel = _readWithTokenMapper.Map(user) with { Token = token };

            return Ok(readModel);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] UserBaseViewModel updateModel)
        {
            var user = await _userService.GetByIdAsync(id);

            _userService.VerifyUserAccessRights(user, User.Identity!, User.Claims!);
            user.Username = updateModel.Username;
            await _userService.UpdateAsync(user);

            return NoContent();
        }

        [HttpPut("{id:int:min(1)}/changePassword")]
        public async Task<ActionResult<string>> ChangePasswordAsync(
            [FromRoute] int id,
            [FromBody] UserChangePasswordViewModel updateModel)
        {
            var player = await _userService.GetByIdAsync(id);

            _userService.VerifyUserAccessRights(player, User.Identity!, User.Claims!);
            _passwordService.CreatePasswordHash(updateModel.Password!, out byte[] hash, out byte[] salt);
            _userService.ChangePasswordData(player, hash, salt);
            await _userService.UpdateAsync(player);

            string token = _passwordService.CreateToken(player);

            return Ok(token);
        }

        [HttpPut("{id:int:min(1)}/changeRole")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> ChangeRoleAsync(
            [FromRoute] int id,
            [FromBody] UserChangeRoleViewModel changeRoleModel)
        {
            var player = await _userService.GetByIdAsync(id);

            player.Role = changeRoleModel.Role;
            await _userService.UpdateAsync(player);

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
