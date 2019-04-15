using AdminSystem.Application;
using AdminSystem.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AdminSystem.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class JsdOrderController : ControllerBase
    {
        IMediator _mediator;
        public JsdOrderController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpPost]
        public string GetJsd()
        {
            return "sdf";
        }
    }
}
