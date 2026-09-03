using System.Net;
using Domain.ResultPattern.Result;

namespace Domain.ResultPattern.Errors;

public static class Errors
{
    public static Error ExternalLoginError = new(HttpStatusCode.BadRequest, "ExternalLoginError", "External login error.");
    
}