using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers.Base
{
    public class BaseController(
        IMediator mediator
        ) : Controller
    {
        protected readonly IMediator _mediator = mediator;
    }
}
