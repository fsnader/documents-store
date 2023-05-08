using DocumentsStore.Api.DTOs;
using DocumentsStore.UseCases.Users;
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

        public UsersController(
            IGetAllUsers getAllUsers,
            IGetUserById getUserById,
            ICreateUser createUser)
        {
            _getAllUsers = getAllUsers;
            _getUserById = getUserById;
            _createUser = createUser;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<ActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _getAllUsers.ExecuteAsync(cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUsers);
        }

        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<ActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _getUserById.ExecuteAsync(id, cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUser);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<ActionResult> Post([FromBody] UserDto user, CancellationToken cancellationToken)
        {
            var result = await _createUser.ExecuteAsync(
                user.ConvertToUser(), 
                cancellationToken);

            return UseCaseActionResult(result, UserDto.CreateFromUser);
        }

        [HttpPost("{id}/add-to-group/{groupId}", Name = "AddUserToGroup")]
        public async Task<ActionResult> AddToGroup(int id, int groupId, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserDto user)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
