using Business.Abstracts.Blacklists;
using Core.CrossCuttingConcerns.Rules;
using Core.Exceptions.Types;
using DataAccess.Abstracts;

namespace Business.Rules;

public class ApplicationBusinessRules: BaseBusinessRules
{
    private readonly IApplicantRepository _repository;
    private readonly IBlacklistService _blacklistService;

    public ApplicationBusinessRules(IApplicantRepository repository)
    {
        _repository = repository;
    }
    public async Task CheckIfApplicantIsBlacklisted(int id)
    {
        var item = await _blacklistService.GetByApplicantIdAsync(id); if (item.Data != null)
        {
            throw new BusinessException("Applicant is blacklisted");
        }
    }
}
