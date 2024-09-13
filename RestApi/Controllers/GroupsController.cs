using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Mappers;
using RestApi.Services;

namespace RestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    //localhost:port/groups/(id)
    [HttpGet("{id}")]
    public async Task<ActionResult<GroupResponse>> GetGroupByID(string id, CancellationToken cancellationToken){
        var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);
        if(group is null)
        {
            return NotFound();
        }else{
            return Ok(group.ToDto());
        }
    }
}