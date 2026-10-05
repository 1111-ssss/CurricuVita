namespace Domain.Interfaces.Services;

public interface IPositionApiTokenService
{
    string Create(int positionId);
    bool TryResolve(string token, out int positionId);
}
