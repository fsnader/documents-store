using DocumentsStore.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsStore.Api.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult ErrorResult<T>(UseCaseResult<T> result) =>
        result.Error switch
        {
            ErrorType.NotFound => NotFound(result.ErrorMessage),
            ErrorType.BadRequest => BadRequest(result.ErrorMessage),
            ErrorType.Unauthorized => Unauthorized(result.ErrorMessage),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };

    protected IActionResult UseCaseActionResult<T, TOutput>(UseCaseResult<T> result,
        Func<T, TOutput> converter)
    {
        if (result.Result is null)
        {
            return ErrorResult(result);
        }

        return Ok(converter(result.Result));
    }
}