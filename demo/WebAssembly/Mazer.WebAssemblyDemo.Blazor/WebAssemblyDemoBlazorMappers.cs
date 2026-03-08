using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Mazer.WebAssemblyDemo.Services.Dtos.Books;

namespace Mazer.WebAssemblyDemo;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebAssemblyDemoBlazorMappers : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);
    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}
