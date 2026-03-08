using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Mazer.ServerDemo.Services.Dtos.Books;
using Mazer.ServerDemo.Entities.Books;

namespace Mazer.ServerDemo.Services.Books;

public interface IBookAppService :
    ICrudAppService< //Defines CRUD methods
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto> //Used to create/update a book
{

}