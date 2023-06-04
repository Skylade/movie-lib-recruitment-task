using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core;
using System.Net;

namespace MovieLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ActionResult SendResponse(ServiceResponse response)
        {
            switch (response.ResponseType)
            {
                case HttpStatusCode.OK:
                    return Ok();

                case HttpStatusCode.Unauthorized:
                    return Unauthorized();

                case HttpStatusCode.Forbidden:
                    return Forbid();

                case HttpStatusCode.NotFound:
                    return NotFound(response.Errors);

                case HttpStatusCode.NoContent:
                    return NoContent();

                default:
                    return BadRequest(response.Errors);
            }
        }

        protected ActionResult SendResponse<T>(ServiceResponse<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.Payload);

                case HttpStatusCode.Unauthorized:
                    return Unauthorized();

                case HttpStatusCode.NotFound:
                    return NotFound(response.Errors);

                case HttpStatusCode.Forbidden:
                    return Forbid();

                case HttpStatusCode.Created:
                    return Created(response.CreatedUrlLocation, response.Payload);

                default:
                    return BadRequest(response.Errors);
            }
        }
    }
}
