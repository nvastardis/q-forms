using AutoMapper;

namespace QForms;

public class QFormsAppServiceBase
{
    protected readonly IMapper ObjectMapper;

    protected QFormsAppServiceBase(
        IMapper objectMapper)
    {
        ObjectMapper = objectMapper;
    }
}