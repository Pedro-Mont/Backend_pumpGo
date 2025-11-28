using BCrypt.Net;
using pump_go.Entities.Enums;
using pump_go.Entities;
using pump_go.Interfaces.IRepositories;
using pump_go.pump_go.Bussiness.DTOs.User;
using pump_go.pump_go.Bussiness.DTOs.UserDTO;
using pump_go.pump_go.Bussiness.Interfaces.IRepositories;
using pump_go.pump_go.Bussiness.Interfaces.IServices;
using pump_go.pump_go.Bussiness.Exceptions;


namespace pump_go.pump_go.Bussiness.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISignatureRepository _signatureRepository;
        private readonly IPlanRepository _planRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ISignatureRepository signatureRepository, IPlanRepository planRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _signatureRepository = signatureRepository;
            _planRepository = planRepository;
            _tokenService = tokenService;
        }

        public async Task<string> AuthenticateAsync(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Usuário não encontrado.");
            }

            bool passwordIsCorrect = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);

            if (!passwordIsCorrect)
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            var token = _tokenService.GerarToken(user);
            return token;

        }

        public async Task<UserDTO> GetUserInfoAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("Usuário não encontrado");
            }

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserDTO> RegisterNewUserAsync(UserRegistrationDTO registrationDTO)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registrationDTO.Email);
            if (existingUser != null)
            {
                throw new UnauthorizedAccessException("O e-mail informado já está em uso.");
            }

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(registrationDTO.Password);

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = registrationDTO.Name,
                Email = registrationDTO.Email,
                Password = hashPassword,
                RegistrationDate = DateTime.UtcNow
            };

            await _userRepository.AddAsync(newUser);

            var basicPlan = await _planRepository.GetByNameAsync("Básico");
            if (basicPlan == null)
            {
                throw new ApplicationException("Configuração de plano 'Básico' não encontrada.");
            }

            var newSignature = new Signature
            {
                Id = Guid.NewGuid(),
                UserId = newUser.Id,
                PlanId = basicPlan.Id,
                StartDate = DateTime.UtcNow,
                ExpirationDate = DateTime.MaxValue,
                Status = SignatureStatus.Active,
            };

            await _signatureRepository.AddAsync(newSignature);

            return new UserDTO
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email
            };

        }
    }
}
