using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Models.Attendance;
using EventManagementAPI.ViewModels.Attendance;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Extensions;
using EventManagementAPI.Domain.Entities;
using System.Net;
using EventManagementAPI.ViewModels.Common;
using EventManagementAPI.Domain.Constants;

namespace EventManagementAPI.Domain.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceInfrastructureService _attendanceInfrastructureService;
    private readonly IEventInfrastructureService _eventInfrastructureService;
    private readonly IUserInfrastructureService _userInfrastructureService;
    private readonly IHttpContextAccessor _contextAccessor;

    public AttendanceService(
        IAttendanceInfrastructureService attendanceInfrastructureService,
        IEventInfrastructureService eventInfrastructureService,
        IUserInfrastructureService userInfrastructureService,
        IHttpContextAccessor contextAccessor)
    {
        _attendanceInfrastructureService = attendanceInfrastructureService;
        _eventInfrastructureService = eventInfrastructureService;
        _userInfrastructureService = userInfrastructureService;
        _contextAccessor = contextAccessor;
    }

    public async Task<Result<AttendanceResponseViewModel>> CreateAttendance(CreateAttendanceDomainModel domainModel, CancellationToken cancellation = default)
    {
        var @event = await _eventInfrastructureService.GetEventAsync(domainModel.EventId, cancellation);

        if (@event == null)
        {
            return Result.Failure<AttendanceResponseViewModel>(new Error(HttpStatusCode.NotFound, "Event was not found."));
        }

        Guid userId = _contextAccessor.GetUserId();
        var user = await _userInfrastructureService.GetUserAsync(userId, cancellation);

        if (user == null)
        {
            return Result.Failure<AttendanceResponseViewModel>(new Error(HttpStatusCode.NotFound, "User was not found."));
        }

        var existingAttendance = await _attendanceInfrastructureService.GetAttendanceAsync(domainModel.EventId, userId, cancellation);

        if (existingAttendance != null)
        {
            return Result.Failure<AttendanceResponseViewModel>(new Error(HttpStatusCode.Conflict, "User is already marked as attending."));
        }

        var attendance = new AttendanceDomainModel
        {
            Event = @event,
            User = user,
            Status = domainModel.AttendanceStatus,
            Timestamp = DateTime.UtcNow
        };

        await _attendanceInfrastructureService.CreateAttendanceAsync(attendance, cancellation);
        await _attendanceInfrastructureService.SaveChangesAsync(cancellation);

        var response = new AttendanceResponseViewModel(
            attendance.Event.Id,
            attendance.User.Id,
            attendance.Status.ToString(),
            attendance.Timestamp);

        return Result.Success(response);
    }

    public async Task<Result<PaginatedListResponseViewModel<UserAttendanceResponseViewModel>>> GetAttendees(Guid eventId, int page, int pageSize, CancellationToken cancellation = default)
    {
        if (pageSize > AttendeesData.MaxPageSize)
        {
            return Result.Failure<PaginatedListResponseViewModel<UserAttendanceResponseViewModel>>(
                new Error(HttpStatusCode.BadRequest,
                $"Page size is too big. Max size is {AttendeesData.MaxPageSize}"));
        }

        var paginatedAttendances = await _attendanceInfrastructureService.GetAttendancesAsync(eventId, pageSize, page, cancellation);

        var userAttendanceList = paginatedAttendances.Items.Select(a =>
            new UserAttendanceResponseViewModel(
                a.User.Id,
                a.User.Username,
                a.Status.ToString(),
                a.Timestamp))
            .ToList();

        var response = new PaginatedListResponseViewModel<UserAttendanceResponseViewModel>(
            paginatedAttendances.Page,
            paginatedAttendances.PageSize,
            paginatedAttendances.TotalPages,
            paginatedAttendances.TotalCount,
            paginatedAttendances.HasPreviousPage,
            paginatedAttendances.HasNextPage,
            userAttendanceList);

        return Result.Success(response);
    }
}
