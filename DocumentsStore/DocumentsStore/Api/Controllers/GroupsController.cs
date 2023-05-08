using DocumentsStore.Api.DTOs;
using DocumentsStore.UseCases.Groups.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsStore.Api.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : BaseController
    {
        private readonly IGetGroupById _getGroupById;
        private readonly ICreateGroup _createGroup;
        private readonly IGetAllGroups _getAllGroups;
        private readonly IUpdateGroup _updateGroup;
        private readonly IDeleteGroup _deleteGroup;

        public GroupsController(
            IGetAllGroups getAllGroups,
            IGetGroupById getGroupById,
            ICreateGroup createGroup, 
            IUpdateGroup updateGroup, 
            IDeleteGroup deleteGroup)
        {
            _getAllGroups = getAllGroups;
            _getGroupById = getGroupById;
            _createGroup = createGroup;
            _updateGroup = updateGroup;
            _deleteGroup = deleteGroup;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GroupDto>), 200)]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken,
            [FromQuery] int take = 100,
            [FromQuery] int skip = 0)
        {
            var result = await _getAllGroups.ExecuteAsync(take, skip, cancellationToken);

            return UseCaseActionResult(result, GroupDto.CreateFromGroups);
        }

        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(GroupDto), 200)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _getGroupById.ExecuteAsync(id, cancellationToken);

            return UseCaseActionResult(result, GroupDto.CreateFromGroup);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GroupDto), 200)]
        public async Task<IActionResult> Post([FromBody] GroupDto Group, CancellationToken cancellationToken)
        {
            var result = await _createGroup.ExecuteAsync(
                Group.ConvertToGroup(), 
                cancellationToken);

            return UseCaseActionResult(result, GroupDto.CreateFromGroup);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GroupDto), 200)]
        public async Task<IActionResult> Put(int id, [FromBody] GroupDto Group, CancellationToken cancellationToken)
        {
            var result = await _updateGroup.ExecuteAsync(
                id,
                Group.ConvertToGroup(),
                cancellationToken);

            return UseCaseActionResult(result, GroupDto.CreateFromGroup);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(GroupDto), 200)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _deleteGroup.ExecuteAsync(id, cancellationToken);

            return UseCaseActionResult(result, GroupDto.CreateFromGroup);
        }
    }
}