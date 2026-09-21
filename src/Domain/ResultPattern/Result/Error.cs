using System.Net;

namespace Domain.ResultPattern.Result;

public record Error(
    HttpStatusCode StatusCode,
    string Code,
    string Message
);