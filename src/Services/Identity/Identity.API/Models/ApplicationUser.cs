// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string PersonalNumber { get; set; }
    public ApplicationUserStatus Status { get; set; }
}

public enum ApplicationUserStatus
{
    Active,
    Passive
}