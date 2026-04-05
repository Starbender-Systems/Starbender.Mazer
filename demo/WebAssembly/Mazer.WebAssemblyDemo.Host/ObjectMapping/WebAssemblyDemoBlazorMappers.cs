using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Mazer.WebAssemblyDemo.Entities.Books;
using Mazer.WebAssemblyDemo.Services.Dtos.Books;

namespace Mazer.WebAssemblyDemo.ObjectMapping;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebAssemblyDemoBookToBookDtoMapper : MapperBase<Book, BookDto>
{
    public override partial BookDto Map(Book source);

    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebAssemblyDemoCreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
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
public partial class WebAssemblyDemoBookDtoToCreateUpdateBookDtoMapper : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);

    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}
