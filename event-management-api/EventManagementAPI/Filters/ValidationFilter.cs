using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace EventManagementAPI.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T)) is not T obj)
        {
            return Results.BadRequest();
        }

        var validationResult = await _validator.ValidateAsync(obj);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);

            var problemDetails = new ProblemDetails
            {
                Status = 400,
                Extensions = { ["errors"] = errors }
            };

            return Results.Problem(problemDetails);
        }

        return await next(context);
    }
}
