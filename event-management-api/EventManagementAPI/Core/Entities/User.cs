﻿namespace EventManagementAPI.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<UserRole> UserRoles { get; set; } = [];
}
