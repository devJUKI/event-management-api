﻿namespace EventManagementAPI.Domain.Models.UserManagement;

public class UpdateUserRequestDomainModel
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
