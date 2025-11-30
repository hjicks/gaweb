using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ISender _sender;
        public BaseController(ISender sender)
        {
            _sender = sender;
        }
    }
}
