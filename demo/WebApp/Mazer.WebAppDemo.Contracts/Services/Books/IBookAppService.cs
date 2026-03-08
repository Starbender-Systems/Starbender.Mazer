using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Mazer.WebAppDemo.Services.Dtos.Books;

namespace Mazer.WebAppDemo.Services.Books;

public interface IBookAppService :
    ICrudAppService< //Defines CRUD methods
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto> //Used to create/update a book
{

}