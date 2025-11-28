using pump_go.Entities;

namespace pump_go.pump_go.Bussiness.Interfaces.IServices
{
    public interface ITokenService
    {
        string GerarToken(User user);
    }
}
