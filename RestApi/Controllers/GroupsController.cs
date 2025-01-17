using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Services;
using RestApi.Mappers;
using RestApi.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace RestApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;
    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    // GET /groups/{id}
    [HttpGet("{id}")]
    [Authorize(Policy = "Read")]
    public async Task<ActionResult<GroupResponse>> GetGroupById(string id, CancellationToken cancellationToken)
    {
        var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group.ToDto());
    }
    
    // GET /groups?name={name}
    [HttpGet]
    [Authorize(Policy = "Read")]
    public async Task<ActionResult<IEnumerable<GroupResponse>>> GetGroupsByName(
        CancellationToken cancellationToken,
        [FromQuery] string name, 
        [FromQuery] int pageIndex = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string orderBy = "name")
    {
        var groups = await _groupService.GetGroupsByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);
        
        if(groups == null || !groups.Any())
        {
            return Ok(new List<GroupResponse>());
        }

        return Ok(groups.Select(group => group.ToDto()));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Write")]
    public async Task<IActionResult> DeleteGroup(string id, CancellationToken cancellationToken)
    {
        try
        {
            await _groupService.DeleteGroupByIdAsync(id, cancellationToken);
            return NoContent();
        }
        catch (GroupNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize(Policy = "Write")]
    public async Task<ActionResult<GroupResponse>> CreateGroup([FromBody] CreateGroupRequest groupRequest, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _groupService.CreateGroupAsync(groupRequest.Name, groupRequest.Users, cancellationToken);
            return CreatedAtAction(nameof(GetGroupById), new{id = group.Id}, group.ToDto());
        }
        catch (InvalidGroupRequestFormatException)
        {
            return BadRequest(NewValidationProblemDetails("One or more validation errors occurred.", HttpStatusCode.BadRequest, new Dictionary<string, string[]>{
                {"Groups", ["Users array is empty"]}
            }));
        }
        catch(GroupAlreadyExistsException){
                return Conflict(NewValidationProblemDetails("One or more validation errors occurred.", HttpStatusCode.Conflict, new Dictionary<string, string[]>{
                {"Groups", ["Group with same name already exists"]}
            }));        
        }
        catch(NonexistentUserId){
            return NotFound(NewValidationProblemDetails("One or more validation errors occurred.", HttpStatusCode.NotFound, new Dictionary<string, string[]>{
                {"Groups", ["User ID not found"]}
            })); 
        }
    }

    private static ValidationProblemDetails NewValidationProblemDetails(string title, 
    HttpStatusCode statusCode, Dictionary<string, string[]> errors){
        return new ValidationProblemDetails{
            Title = title,
            Status = (int) statusCode,
            Errors = errors
        };
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Write")]
    public async Task<IActionResult> UpdateGroup(string id, [FromBody] UpdateGroupRequest groupRequest, CancellationToken cancellationToken){
        try
        {
            await _groupService.UpdateGroupAsync(id, groupRequest.Name, groupRequest.Users, cancellationToken);
            return NoContent();
        }
        catch(GroupNotFoundException){
            return NotFound();
        }
        catch (InvalidGroupRequestFormatException)
        {
            return BadRequest(NewValidationProblemDetails("One or more validation errors occurred.", HttpStatusCode.BadRequest, new Dictionary<string, string[]>{
                {"Groups", ["Users array is empty"]}
            }));
        }
        catch(GroupAlreadyExistsException){
                return Conflict(NewValidationProblemDetails("One or more validation errors occurred.", HttpStatusCode.Conflict, new Dictionary<string, string[]>{
                {"Groups", ["Group with same name already exists"]}
            }));        }
    }
}