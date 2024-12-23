using EventManagementAPI.Domain.Entities;

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
        return Results.Problem(statusCode: (int)error.Code, detail: error.Description);
    }
}
