using EventManagementAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementAPI.Domain.Helpers;

public static class CustomResults
{
    public static IResult Ok()
    {
        return Results.Ok();
    }

    public static IResult Ok<T>(T payload)
    {
        return Results.Ok(payload);
    }

    public static IResult Problem(Error error)
    {
        var problemDetails = new ProblemDetails
        {
            Status = (int)error.Code,
            Extensions = { ["errors"] = new List<string> { error.Description } }
        };

        return Results.Problem(problemDetails);
    }
}
