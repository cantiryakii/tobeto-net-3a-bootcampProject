using Core.CrossCuttingConcerns.Rules;
using Core.Exceptions.Types;
using Core.Utilities.Helpers;
using DataAccess.Abstracts;
using DataAccess.Concretes.Repositories;

namespace Business.Rules;

public class EmployeeBusinessRules: BaseBusinessRules
{
    private readonly IEmployeeRepository _repository;

    public EmployeeBusinessRules(IEmployeeRepository repository)
    {
        _repository = repository;
    }
    public async Task CheckUserNameIfExist(string userName, int? id)
    {

        var item = await _repository.GetAsync(x => x.UserName == SeoHelper.ToSeoUrl(userName) && x.Id != id);
        if (item != null)
        {
            throw new BusinessException("Username already exist");
        }
    }

    public async Task CheckIdIfNotExist(int id)
    {
        var item = await _repository.GetAsync(x => x.Id == id);
        if (item == null)
        {
            throw new NotFoundException("ID could not be found.");
        }
    }
}
