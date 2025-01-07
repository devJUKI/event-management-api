using EventManagementAPI.Domain.Models.Attendance;
using EventManagementAPI.ViewModels.Attendance;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Extensions;
using EventManagementAPI.Domain.Helpers;
using EventManagementAPI.Filters;

namespace EventManagementAPI.Endpoints;

public static class AttendanceEndpoints
{
    public static void MapAttendanceEndpoints(this WebApplication app)
    {
        var mapGroup = app
            .MapGroup("/events/{eventId}")
            .WithTags("Attendance");

        mapGroup.MapPost("/attend", async (
            Guid eventId,
            CreateAttendanceRequestViewModel viewModel,
            IAttendanceService service,
            CancellationToken cancellation) =>
        {
            var domainModel = new CreateAttendanceDomainModel
            {
                EventId = eventId,
                AttendanceStatus = viewModel.Status!.Value
            };

            var response = await service.CreateAttendance(domainModel, cancellation);

            return response.Match(CustomResults.Ok, CustomResults.Problem);
        })
        .AddEndpointFilter<ValidationFilter<CreateAttendanceRequestViewModel>>()
        .RequireAuthorization();

        mapGroup.MapGet("/attendees", async (
            Guid eventId,
            int page,
            int pageSize,
            IAttendanceService service,
            CancellationToken cancellation) =>
        {
            var response = await service.GetAttendees(eventId, page, pageSize, cancellation);

            return response.Match(CustomResults.Ok, CustomResults.Problem);
        })
        .RequireAuthorization();
    }
}
