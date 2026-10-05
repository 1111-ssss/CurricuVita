using System.Text;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Services;

public class PositionApiTokenProtector : IPositionApiTokenService
{
    private const string Purpose = "PositionApiToken.v1";
    private const string Prefix = "position:";

    private readonly IDataProtector _protector;

    public PositionApiTokenProtector(
        IDataProtectionProvider provider
    )
    {
        _protector = provider.CreateProtector(Purpose);
    }

    public string Create(
        int positionId
    )
    {
        var protectedPayload = _protector.Protect(Prefix + positionId);
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(protectedPayload));
    }

    public bool TryResolve(
        string token,
        out int positionId
    )
    {
        positionId = 0;
        if (string.IsNullOrWhiteSpace(token) || token.Length > 1024)
        {
            return false;
        }

        string unprotected;
        try
        {
            var protectedPayload = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token.Trim()));
            unprotected = _protector.Unprotect(protectedPayload);
        }
        catch
        {
            return false;
        }

        if (!unprotected.StartsWith(Prefix, StringComparison.Ordinal))
        {
            return false;
        }

        return int.TryParse(unprotected[Prefix.Length..], out positionId) && positionId > 0;
    }
}
