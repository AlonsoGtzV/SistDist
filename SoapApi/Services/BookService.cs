using System.ServiceModel;
using SoapApi.Contracts;
using SoapApi.Dtos;
using SoapApi.Repositories;
using SoapApi.Mappers;
using SoapApi.Infrastructure.Entities;

namespace SoapApi.Services;

public class BookService : IBookContract
{
    private readonly IBookRepository _bookRepository;

    // Inyección de dependencias en el constructor
    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
    }

    // Método para obtener libros por nombre
    public async Task<IList<BookResponseDto>> GetBookByName(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new FaultException("No se encontraron libros con ese nombre.");

        }

        var books = await _bookRepository.GetBookByNameAsync(name, cancellationToken);
        
        if (books == null || !books.Any())
        {
            throw new FaultException("No se encontraron libros con ese nombre.");
        }

        return books.Select(book => book.ToDto()).ToList();
    }
}
