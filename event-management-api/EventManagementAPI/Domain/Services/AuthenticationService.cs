using EventManagementAPI.Core.Interfaces.Authentication;
using EventManagementAPI.Domain.Models.Authentication;
using EventManagementAPI.ViewModels.Authentication;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.ViewModels.Common;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Constants;
using EventManagementAPI.Domain.Entities;
using System.Net;

namespace EventManagementAPI.Domain.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationInfrastructureService _infrastructureService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(
        IAuthenticationInfrastructureService infrastructureService,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _infrastructureService = infrastructureService;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResponseViewModel>> Register(RegisterDomainModel domainModel, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(domainModel.Password))
        {
            return Result.Failure<AuthResponseViewModel>(new Error(HttpStatusCode.BadRequest, "Password is empty"));
        }

        var hashedPassword = _passwordHasher.Hash(domainModel.Password);

        var userResult = UserDomainModel.Create(
            domainModel.Username,
            domainModel.Email,
            hashedPassword,
            DateTime.UtcNow,
            [Roles.User]);

        if (userResult.IsFailure)
        {
            return Result.Failure<AuthResponseViewModel>(userResult.Error);
        }

        var user = userResult.Payload;

        var existingUser = await _infrastructureService.GetUserByEmailAndUsernameAsync(user.Email, user.Username, cancellation);

        if (existingUser != null)
        {
            return Result.Failure<AuthResponseViewModel>(new Error(HttpStatusCode.Conflict, "User with this email or username already exists"));
        }

        await _infrastructureService.CreateUserAsync(user, cancellation);
        await _infrastructureService.SaveChangesAsync(cancellation);

        var token = _jwtTokenGenerator.GenerateJwtToken(user.Id, user.Username, user.Roles);

        var userResponse = new UserResponseViewModel(user.Id, user.Username, user.Email, user.CreatedAt, user.Roles);
        var response = new AuthResponseViewModel(userResponse, token);

        return Result.Success(response);
    }

    public async Task<Result<AuthResponseViewModel>> Login(LoginDomainModel domainModel, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(domainModel.Password))
        {
            return Result.Failure<AuthResponseViewModel>(new Error(HttpStatusCode.BadRequest, "Password is not specified"));
        }

        if (string.IsNullOrWhiteSpace(domainModel.Email))
        {
            return Result.Failure<AuthResponseViewModel>(new Error(HttpStatusCode.BadRequest, "Email is not specified"));
        }

        var user = await _infrastructureService.GetUserByEmailAsync(domainModel.Email, cancellation);

        if (user == null || !_passwordHasher.Verify(domainModel.Password, user.HashedPassword))
        {
            return Result.Failure<AuthResponseViewModel>(new Error(HttpStatusCode.Unauthorized, "Wrong credentials"));
        }

        var token = _jwtTokenGenerator.GenerateJwtToken(user.Id, user.Username, user.Roles);

        var userResponse = new UserResponseViewModel(user.Id, user.Username, user.Email, user.CreatedAt, user.Roles);
        var response = new AuthResponseViewModel(userResponse, token);

        return Result.Success(response);
    }
}
