using System;
using Volo.Abp.Application.Dtos;
using AbpMudTheme.MvcDemo.Entities.Books;

namespace AbpMudTheme.MvcDemo.Services.Dtos.Books;

public class BookDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }
}