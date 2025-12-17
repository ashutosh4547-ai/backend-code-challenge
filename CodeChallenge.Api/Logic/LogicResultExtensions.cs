using Microsoft.AspNetCore.Mvc;
using CodeChallenge.Api.Logic.Results;

namespace CodeChallenge.Api.Logic
{
    public static class LogicResultExtensions
    {
        public static IActionResult ToActionResult<T>(this LogicResult<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            if (result.IsNotFound)
            {
                return new NotFoundObjectResult(result.Error);
            }

            if (result.IsInvalid)
            {
                return new BadRequestObjectResult(result.Error);
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
