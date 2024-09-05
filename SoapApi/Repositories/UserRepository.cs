using Microsoft.EntityFrameworkCore;
using SoapApi.Infrastructure;
using SoapApi.Models;
using SoapApi.Mappers;
using SoapApi.Infrastructure.Entities;
using System.ServiceModel;
using System.IdentityModel.Tokens;

namespace SoapApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RelationalDbContext _dbContext;

    public UserRepository(RelationalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return user.ToModel();
    }

    public async Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
        return users.Select(user => user.ToModel()).ToList();
    }

    public async Task<IList<UserModel>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users.AsNoTracking().Where(users => users.Email.Contains(email)).ToListAsync(cancellationToken);
        return users.Select(user => user.ToModel()).ToList();
    }

    public async Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = user.ToEntity();
        _dbContext.Users.Remove(userEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = user.ToEntity();
        userEntity.Id = Guid.NewGuid();
        await _dbContext.AddAsync(userEntity, cancellationToken);
        await _dbContext.SaveChangesAsync();
        return userEntity.ToModel();
    }

    public async Task<bool> UpdateUser(UserModel userUpdate, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(userUpdate.Id, cancellationToken);

        // Actualiza solo los campos necesarios
        user.FirstName = userUpdate.FirstName;
        user.LastName = userUpdate.LastName;
        user.Birthday = userUpdate.BirthDate;

        _dbContext.Users.Update(user);

        await _dbContext.SaveChangesAsync();
        return true;
    }  


}
