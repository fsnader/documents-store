using DocumentsStore.Api.DTOs;
using DocumentsStore.Api.DTOs.Groups;
using DocumentsStore.Api.DTOs.Users;
using DocumentsStore.UseCases.Users.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsStore.Api.Controllers
{
    [Route("api/users")]
    [ApiController, Authorize(Roles = "Admin")]
    public class UsersController : BaseController
    {
        private readonly IGetUserById _getUserById;
        private readonly ICreateUser _createUser;
        private readonly IGetAllUsers _getAllUsers;
        private readonly IUpdateUser _updateUser;
        private readonly IDeleteUser _deleteUser;

        private readonly IAddUserToGroup _addUserToGroup;
        private readonly IRemoveUserFromGroup _removeUserFromGroup;
        private readonly IGetUserGroups _getUserGroups;

        public UsersController(
            IGetAllUsers getAllUsers,
            IGetUserById getUserById,
            ICreateUser createUser,
            IUpdateUser updateUser,
            IDeleteUser deleteUser,
            IAddUserToGroup addUserToGroup,
            IRemoveUserFromGroup removeUserFromGroup,
            IGetUserGroups getUserGroups)
        {
            _getAllUsers = getAllUsers;
            _getUserById = getUserById;
            _createUser = createUser;
            _updateUser = updateUser;
            _deleteUser = deleteUser;
            _addUserToGroup = addUserToGroup;
            _removeUserFromGroup = removeUserFromGroup;
            _getUserGroups = getUserGroups;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto[]), 200)]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken,
            [FromQuery] int take = 100,
            [FromQuery] int skip = 0)
        {
            var result = await _getAllUsers.ExecuteAsync(take, skip, cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUsers);
        }

        [HttpGet("{id}", Name = nameof(GetUserById))]
        [ProducesResponseType(typeof(UserWithGroupsDto), 200)]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
        {
            var result = await _getUserById.ExecuteAsync(id, cancellationToken);

            return UseCaseActionResult(result, UserWithGroupsDto.CreateFromUser);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Post([FromBody] CreateUserDto user, CancellationToken cancellationToken)
        {
            var result = await _createUser.ExecuteAsync(
                user.ConvertToUser(),
                cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUser);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Put(int id, [FromBody] CreateUserDto user, CancellationToken cancellationToken)
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

        [HttpGet("{id}/groups")]
        [ProducesResponseType(typeof(GroupDto[]), 200)]
        public async Task<IActionResult> GetUserGroups(int id, CancellationToken cancellationToken)
        {
            var result = await _getUserGroups.ExecuteAsync(id, cancellationToken);
            
            return UseCaseActionResult(result, GroupDto.CreateFromGroups);
        }
        
        [HttpPost("{userId}/groups/{groupId}")]
        [ProducesResponseType(typeof(UserWithGroupsDto), 200)]
        public async Task<IActionResult> AddUserToGroup(int userId, int groupId, CancellationToken cancellationToken)
        {
            var result = await _addUserToGroup.ExecuteAsync(
                userId,
                groupId,
                cancellationToken);

            return UseCaseActionResult(result, UserWithGroupsDto.CreateFromUser);
        }
        
        [HttpDelete("{userId}/groups/{groupId}")]
        [ProducesResponseType(typeof(UserWithGroupsDto), 200)]
        public async Task<IActionResult> RemoveUserFromGroup(int userId, int groupId, CancellationToken cancellationToken)
        {
            var result = await _removeUserFromGroup.ExecuteAsync(
                userId,
                groupId,
                cancellationToken);

            return UseCaseActionResult(result, UserWithGroupsDto.CreateFromUser);
        }
    }
}