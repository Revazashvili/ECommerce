namespace Identity.API.Models;

public class ApplicationUserResponse(string id, string? userName, string? email, 
    bool emailConfirmed, string? phoneNumber, string personalNumber, ApplicationUserStatus status)
{
    public string Id { get; set; } = id;
    public string? UserName { get; set; } = userName;
    public string? Email { get; set; } = email;
    public bool EmailConfirmed { get; set; } = emailConfirmed;
    public string? PhoneNumber { get; set; } = phoneNumber;
    public string PersonalNumber { get; set; } = personalNumber;
    public ApplicationUserStatus Status { get; set; } = status;
}