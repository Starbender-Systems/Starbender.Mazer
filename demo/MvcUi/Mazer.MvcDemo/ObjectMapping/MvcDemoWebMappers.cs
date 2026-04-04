using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Mazer.MvcDemo.Entities.Books;
using Mazer.MvcDemo.Services.Dtos.Books;

namespace Mazer.MvcDemo.ObjectMapping;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MvcDemoBookToBookDtoMapper : MapperBase<Book, BookDto>
{
    public override partial BookDto Map(Book source);

    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MvcDemoCreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
{
    [MapperIgnoreTarget(nameof(Book.CreationTime))]
    [MapperIgnoreTarget(nameof(Book.CreatorId))]
    [MapperIgnoreTarget(nameof(Book.LastModificationTime))]
    [MapperIgnoreTarget(nameof(Book.LastModifierId))]
    [MapperIgnoreTarget(nameof(Book.ConcurrencyStamp))]
    public override partial Book Map(CreateUpdateBookDto source);

    [MapperIgnoreTarget(nameof(Book.CreationTime))]
    [MapperIgnoreTarget(nameof(Book.CreatorId))]
    [MapperIgnoreTarget(nameof(Book.LastModificationTime))]
    [MapperIgnoreTarget(nameof(Book.LastModifierId))]
    [MapperIgnoreTarget(nameof(Book.ConcurrencyStamp))]
    public override partial void Map(CreateUpdateBookDto source, Book destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MvcDemoBookDtoToCreateUpdateBookDtoMapper : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);

    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}
