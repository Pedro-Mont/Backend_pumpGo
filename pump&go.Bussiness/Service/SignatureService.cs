using pump_go.Entities.Enums;
using pump_go.Interfaces.IRepositories;
using pump_go.pump_go.Bussiness.DTOs.Signature;
using pump_go.pump_go.Bussiness.Exceptions;
using pump_go.pump_go.Bussiness.Interfaces.IRepositories;
using pump_go.pump_go.Bussiness.Interfaces.IServices;
using pump_go.pump_go.Data.Repositories;

namespace pump_go.pump_go.Bussiness.Service
{
    public class SignatureService : ISignatureService
    {
        private readonly ISignatureRepository _signatureRepository;
        private readonly IPlanRepository _planRepository;

        public SignatureService(ISignatureRepository signatureRepository, IPlanRepository planRepository)
        {
            _signatureRepository = signatureRepository;
            _planRepository = planRepository;
        }


        public async Task<SignatureDTO> GetUsersSignatureAsync(Guid userId)
        {
            var signature = await _signatureRepository.GetByUserIdAsync(userId);

            if (signature == null)
            {
                throw new Exception("Assinatura do usuário não encontrada. Ocorreu um erro de integridade de dados.");
            }

            var plan = await _planRepository.GetByIdAsync(signature.PlanId);
            if (plan == null)
            {
                throw new Exception($"Plano com ID '{signature.PlanId}' não encontrado. Ocorreu um erro de configuração.");
            }

            return new SignatureDTO
            {
                PlanName = plan.Name,
                ExpirationDate = signature.ExpirationDate,
                Status = signature.Status
            };
        }

        public async Task<bool> UserHasAccessToProfessional(Guid userId, ProfessionalType type)
        {
            var signatureDTO = await GetUsersSignatureAsync(userId);

            if (signatureDTO.Status != SignatureStatus.Active)
            {
                return false;
            }

            switch (signatureDTO.PlanName)
            {
                case "Ultra":
                    return true;

                case "Pro":
                    return type == ProfessionalType.Personal;

                case "Básico":
                default:
                    return false;
            }
        }

        public async Task<SignatureDTO> UpgradeSignatureAsync(Guid userId, string planName)
        {
            var newPlan = await _planRepository.GetByNameAsync(planName);
            if (newPlan == null)
            {
                throw new NotFoundException($"Plano '{planName}' não encontrado.");
            }

            var signature = await _signatureRepository.GetByUserIdAsync(userId);
            if (signature == null)
            {
                throw new NotFoundException("Assinatura do usuário não encontrada.");
            }

            signature.PlanId = newPlan.Id;
            signature.Status = SignatureStatus.Active;
            signature.StartDate = DateTime.UtcNow;
            signature.ExpirationDate = DateTime.UtcNow.AddYears(1); 

            await _signatureRepository.UpdateAsync(signature);

            return new SignatureDTO
            {
                PlanName = newPlan.Name,
                ExpirationDate = signature.ExpirationDate,
                Status = signature.Status
            };
        }
    }
}