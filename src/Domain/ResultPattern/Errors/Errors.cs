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
    public static Error EmailConfirmationLinkFailed = new(HttpStatusCode.BadRequest, "EmailConfirmationLinkFailed", "Could not prepare the email confirmation link.");
    public static Error PositionNotFound = new(HttpStatusCode.NotFound, "PositionNotFound", "Position not found.");
    public static Error PositionInvalidTitle = new(HttpStatusCode.BadRequest, "PositionInvalidTitle", "Title is required (max 250 chars).");
    public static Error PositionInvalidDescription = new(HttpStatusCode.BadRequest, "PositionInvalidDescription", "Description must be at most 5000 characters.");
    public static Error PositionInvalidMaxProjects = new(HttpStatusCode.BadRequest, "PositionInvalidMaxProjects", "Max projects must be between 1 and 50.");
    public static Error PositionAttributeDuplicate = new(HttpStatusCode.BadRequest, "PositionAttributeDuplicate", "Each attribute can be added to a position only once.");
    public static Error PositionAttributeNotFound = new(HttpStatusCode.BadRequest, "PositionAttributeNotFound", "One of the selected attributes does not exist.");
    public static Error PositionAccessRuleInvalid = new(HttpStatusCode.BadRequest, "PositionAccessRuleInvalid", "Access rules must reference existing attributes and non-empty values.");
    public static Error CvNotFound = new(HttpStatusCode.NotFound, "CvNotFound", "CV not found.");
    public static Error CvAlreadyExists = new(HttpStatusCode.Conflict, "CvAlreadyExists", "Only one CV per position is allowed.");
    public static Error CvAccessDenied = new(HttpStatusCode.Forbidden, "CvAccessDenied", "This position is not available for you.");
    public static Error CvNotReady = new(HttpStatusCode.BadRequest, "CvNotReady", "All required attributes must be filled before publishing.");
    public static Error CvForbidden = new(HttpStatusCode.Forbidden, "CvForbidden", "You cannot modify this CV.");
    public static Error DiscussionNotFound = new(HttpStatusCode.NotFound, "DiscussionNotFound", "Discussion message not found.");
    public static Error DiscussionInvalidContent = new(HttpStatusCode.BadRequest, "DiscussionInvalidContent", "Message text is required (max 5000 chars).");
    public static Error DiscussionForbidden = new(HttpStatusCode.Forbidden, "DiscussionForbidden", "You cannot post in this discussion.");
    public static Error LikeForbidden = new(HttpStatusCode.Forbidden, "LikeForbidden", "Only recruiters can like CVs.");
    public static Error LikeNotFound = new(HttpStatusCode.NotFound, "LikeNotFound", "Like not found.");
}