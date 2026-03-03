using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using AbpMudTheme.WebAssemblyDemo.Services.Dtos.Books;

namespace AbpMudTheme.WebAssemblyDemo;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebAssemblyDemoBlazorMappers : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);
    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}
