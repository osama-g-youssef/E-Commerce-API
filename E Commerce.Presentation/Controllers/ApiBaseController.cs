using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")] //BaseUrl/api/products
    public class ApiBaseController : ControllerBase
    {
        // # handle Result without value 
        // Result is success Return NoContent 204
        // Result is failed Return Problem with status code  and error details

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProblem(result.Errors);

        }
        protected string GetEmailFromToken()=> User.FindFirstValue(ClaimTypes.Email)!;

        // # handle result with value
        // Result is success Return Okj 200 with value
        // Result is failed Return Problem with status code  and error details

        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            else
               return HandleProblem(result.Errors);


        }


        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            // if no errors provided return 500 error 
            if(errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unexpected Error",
                    detail: "An unexpected error has occured but no error details were provided");

            // if all errors are validation errors , handle them as validation problem 
            if (errors.All(e => e.Type == ErrorType.Validation))
                return HandleValidationProblem(errors);
            // if there is only one error , handle it as a single error 
            return HandleSignleErrorProblem(errors[0]);

        }

        private ActionResult HandleSignleErrorProblem(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: MapErrorTypeToStatusCode(error.Type)


                );
        }
        private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.InvalidCredintials => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();
            foreach (var error in errors)
                modelState.AddModelError(error.Code, error.Description);
            
            return ValidationProblem(modelState);
        }
    }
}
