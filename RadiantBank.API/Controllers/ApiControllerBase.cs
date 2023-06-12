using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RadiantBank.API.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ApiControllerBase: ControllerBase
{
    private ISender _mediator = null!;
    
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}