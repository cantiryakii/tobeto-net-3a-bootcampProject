using Core.CrossCuttingConcerns.Rules;
using Core.Exceptions.Types;
using DataAccess.Abstracts;
using DataAccess.Concretes.Repositories;

namespace Business.Rules;

public class UserBusinessRules: BaseBusinessRules
{
    private readonly IUserRepository _repository;

    public UserBusinessRules(IUserRepository repository)
    {
        _repository = repository;
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
