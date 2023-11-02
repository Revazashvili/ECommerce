namespace Identity.API.Models;

public class ApplicationUserResponse
{
    public ApplicationUserResponse(string id, string? userName, string? email, 
        bool emailConfirmed, string? phoneNumber, string personalNumber, ApplicationUserStatus status)
    {
        Id = id;
        UserName = userName;
        Email = email;
        EmailConfirmed = emailConfirmed;
        PhoneNumber = phoneNumber;
        PersonalNumber = personalNumber;
        Status = status;
    }

    public string Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public string PersonalNumber { get; set; }
    public ApplicationUserStatus Status { get; set; }
}