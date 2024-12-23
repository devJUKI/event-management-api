using System.Net;

namespace EventManagementAPI.Domain.Entities;

public sealed record Error(HttpStatusCode Code, string Description)
{
    public static readonly Error None = new(HttpStatusCode.OK, string.Empty);
}
