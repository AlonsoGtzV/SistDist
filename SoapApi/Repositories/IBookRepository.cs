using SoapApi.Infrastructure.Entities;
using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IBookRepository
{
    Task<IList<BookModel>> GetBookByNameAsync(string name, CancellationToken cancellationToken);
}
