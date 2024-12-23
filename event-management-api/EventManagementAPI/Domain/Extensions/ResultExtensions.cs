using EventManagementAPI.Domain.Entities;

namespace EventManagementAPI.Domain.Extensions;

public static class ResultExtensions
{
    public static TResult Match<TResult, TPayload>(
        this Result<TPayload> result,
        Func<TPayload, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Payload) : onFailure(result.Error);
    }

    public static TResult Match<TResult>(
        this Result result,
        Func<TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }
}
