using System.Net;
using Domain.ResultPattern.Result;

namespace Domain.ResultPattern.Errors;

public static class Errors
{
    public static Error ExternalLoginError = new(HttpStatusCode.BadRequest, "ExternalLoginError", "External login error.");
    public static Error InvalidCredentials = new(HttpStatusCode.BadRequest, "InvalidCredentials", "Invalid credentials.");
    public static Error EmailNotConfirmed = new(HttpStatusCode.BadRequest, "EmailNotConfirmed", "Email not confirmed.");
    public static Error UserLockedOut = new(HttpStatusCode.BadRequest, "UserLockedOut", "User locked out.");
    public static Error LoginNotAllowed = new(HttpStatusCode.BadRequest, "LoginNotAllowed", "Login not allowed.");
    public static Error Unauthorized = new(HttpStatusCode.Unauthorized, "Unauthorized", "Unauthorized.");
    public static Error NotFound = new(HttpStatusCode.NotFound, "NotFound", "Not found.");
    public static Error AvatarUploadError = new(HttpStatusCode.BadRequest, "AvatarUploadError", "Avatar upload error.");
}