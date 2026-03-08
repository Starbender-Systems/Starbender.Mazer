using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Mazer.WebAppDemo.Services.Dtos.Books;

namespace Mazer.WebAppDemo.ObjectMapping;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebAppDemoBlazorMappers : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);
    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}
