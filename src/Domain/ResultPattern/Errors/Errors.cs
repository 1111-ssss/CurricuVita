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
    public static Error UserNotFound = new(HttpStatusCode.NotFound, "UserNotFound", "User not found.");
    public static Error AvatarUploadError = new(HttpStatusCode.BadRequest, "AvatarUploadError", "Avatar upload error.");
    public static Error EmailConfirmationTokenAlreadySent = new(HttpStatusCode.BadRequest, "EmailConfirmationTokenAlreadySent", "Email confirmation token already sent.");
    public static Error AttributeNotFound = new(HttpStatusCode.NotFound, "AttributeNotFound", "Attribute not found.");
    public static Error AttributeNameDuplicate = new(HttpStatusCode.Conflict, "AttributeNameDuplicate", "An attribute with this name already exists.");
    public static Error AttributeProtected = new(HttpStatusCode.Forbidden, "AttributeProtected", "System attributes (Me) cannot be deleted.");
    public static Error AttributeInvalidOptions = new(HttpStatusCode.BadRequest, "AttributeInvalidOptions", "Dropdown attributes require at least one option; other types must not have options.");
    public static Error ConcurrencyConflict = new(HttpStatusCode.Conflict, "ConcurrencyConflict", "The record was modified by another user. Reload and try again.");
    public static Error ValidationFailed = new(HttpStatusCode.BadRequest, "ValidationFailed", "Validation failed.");
}