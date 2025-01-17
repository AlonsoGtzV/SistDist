using System.Globalization;
using RestApi.Exceptions;
using RestApi.Models;
using RestApi.Repositories;

namespace RestApi.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;

    private readonly IUserRepository _userRepository;

    public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<GroupUserModel> GetGroupByIdAsync(string id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);

        if(group == null)
        {
            return null;
        }

        return new GroupUserModel {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate
        };
    }
    
        public async Task<IEnumerable<GroupUserModel>> GetGroupsByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);

        var groupUserModels = await Task.WhenAll(groups.Select(async group => 
        {
            var users = await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)));
            return new GroupUserModel
            {
                Id = group.Id,
                Name = group.Name,
                CreationDate = group.CreationDate,
                Users = users.Where(user => user != null).ToList()
            };
        }));

        return groupUserModels;
    }

    public async Task DeleteGroupByIdAsync(string id, CancellationToken cancellationToken){
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        await _groupRepository.DeleteByIdAsync(id, cancellationToken);
    }

    public async Task<GroupUserModel> CreateGroupAsync(string name, Guid[] users, CancellationToken cancellationToken)
    {
        if(users.Length == 0){
            throw new InvalidGroupRequestFormatException();
        }

        var groups = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if(groups is not null){
            throw new GroupAlreadyExistsException();
        }
        foreach (var userId in users)
            {
                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
                if (user is null)
                {
                    throw new NonexistentUserId();    
                }
            }
            
        

        var group = await _groupRepository.CreateAsync(name, users, cancellationToken);
        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user !=null).ToList()

        };
    }

        public async Task<GroupUserModel> GetGroupByExactNameAsync(string name, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if (group == null)
        {
            return null;
        }

        return new GroupUserModel
        {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user != null).ToList()
        };
    }
    public async Task UpdateGroupAsync(string id, string name, Guid[] users, CancellationToken cancellationToken)
        {
            if(users.Length == 0){
                throw new InvalidGroupRequestFormatException();
            }
            var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
            if(group is null){
                throw new GroupNotFoundException();
            }
        
            var groups = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
            if(groups is not null && groups.Id != id){
                throw new GroupAlreadyExistsException();
            }

            foreach (var userId in users)
            {
                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
                if (user is null)
                {
                    throw new NonexistentUserId();    
                }
            }

            await _groupRepository.UpdateGroupAsync(id, name, users, cancellationToken);
        }


    public Task UpdateGroupAsync(string id, string name, Guid[] users, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}