using pump_go.pump_go.Bussiness.DTOs.User;
using pump_go.pump_go.Bussiness.DTOs.UserDTO;

namespace pump_go.pump_go.Bussiness.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserDTO> RegisterNewUserAsync(UserRegistrationDTO registrationDTO);
        Task<string> AuthenticateAsync(LoginDTO loginDto);
        Task<UserDTO> GetUserInfoAsync(Guid userId);
    }
}
