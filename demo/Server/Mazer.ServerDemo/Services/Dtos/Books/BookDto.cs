using System;
using Volo.Abp.Application.Dtos;
using Mazer.ServerDemo.Entities.Books;

namespace Mazer.ServerDemo.Services.Dtos.Books;

public class BookDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }
}
