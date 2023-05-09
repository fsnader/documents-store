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

        private readonly IAddUserToGroup _addUserToGroup;
        private readonly IRemoveUserFromGroup _removeUserFromGroup;

        public UsersController(
            IGetAllUsers getAllUsers,
            IGetUserById getUserById,
            ICreateUser createUser,
            IUpdateUser updateUser,
            IDeleteUser deleteUser,
            IAddUserToGroup addUserToGroup,
            IRemoveUserFromGroup removeUserFromGroup)
        {
            _getAllUsers = getAllUsers;
            _getUserById = getUserById;
            _createUser = createUser;
            _updateUser = updateUser;
            _deleteUser = deleteUser;
            _addUserToGroup = addUserToGroup;
            _removeUserFromGroup = removeUserFromGroup;
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

        [HttpGet("{id}", Name = nameof(GetUserById))]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
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
        
        [HttpPost("{userId}/add-to-group/{groupId}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> AddUserToGroup(int userId, int groupId, CancellationToken cancellationToken)
        {
            var result = await _addUserToGroup.ExecuteAsync(
                userId,
                groupId,
                cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUser);
        }
        
        [HttpPost("{userId}/remove-from-group/{groupId}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> RemoveUserFromGroup(int userId, int groupId, CancellationToken cancellationToken)
        {
            var result = await _removeUserFromGroup.ExecuteAsync(
                userId,
                groupId,
                cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUser);
        }
    }
}