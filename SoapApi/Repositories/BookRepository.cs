using Microsoft.EntityFrameworkCore;
using SoapApi.Infrastructure;
using SoapApi.Models;
using SoapApi.Mappers;
using SoapApi.Infrastructure.Entities;
using System.ServiceModel;
using System.IdentityModel.Tokens;

namespace SoapApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly RelationalDbContext _dbContext;

    public BookRepository(RelationalDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IList<BookModel>> GetBookByNameAsync(string name, CancellationToken cancellationToken)
    {
        var books = await _dbContext.Books.AsNoTracking().Where(books => books.Title.Contains(name)).ToListAsync(cancellationToken);
        return books.Select(book => book.ToModel()).ToList();}
}