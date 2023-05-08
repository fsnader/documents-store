using DocumentsStore.Api.DTOs;
using DocumentsStore.UseCases.Users.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsStore.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IGetUserById _getUserById;
        private readonly ICreateUser _createUser;
        private readonly IGetAllUsers _getAllUsers;
        private readonly IUpdateUser _updateUser;
        private readonly IDeleteUser _deleteUser;

        public UsersController(
            IGetAllUsers getAllUsers,
            IGetUserById getUserById,
            ICreateUser createUser, 
            IUpdateUser updateUser, 
            IDeleteUser deleteUser)
        {
            _getAllUsers = getAllUsers;
            _getUserById = getUserById;
            _createUser = createUser;
            _updateUser = updateUser;
            _deleteUser = deleteUser;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken,
            [FromQuery] int take = 100,
            [FromQuery] int skip = 0)
        {
            var result = await _getAllUsers.ExecuteAsync(take, skip, cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUsers);
        }

        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _getUserById.ExecuteAsync(id, cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUser);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Post([FromBody] UserDto user, CancellationToken cancellationToken)
        {
            var result = await _createUser.ExecuteAsync(
                user.ConvertToUser(), 
                cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUser);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto user, CancellationToken cancellationToken)
        {
            var result = await _updateUser.ExecuteAsync(
                id,
                user.ConvertToUser(),
                cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUser);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _deleteUser.ExecuteAsync(id, cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUser);
        }
    }
}
