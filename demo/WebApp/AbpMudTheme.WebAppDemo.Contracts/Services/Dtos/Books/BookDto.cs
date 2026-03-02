using System;
using Volo.Abp.Application.Dtos;
using AbpMudTheme.WebAppDemo.Entities.Books;

namespace AbpMudTheme.WebAppDemo.Services.Dtos.Books;

public class BookDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }
}