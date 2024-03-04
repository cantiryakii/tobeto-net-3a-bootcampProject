using Core.CrossCuttingConcerns.Rules;
using Core.Exceptions.Types;
using DataAccess.Abstracts;
using DataAccess.Concretes.Repositories;

namespace Business.Rules;

public class BootcampStateBusinessRules:BaseBusinessRules
{
    private readonly IBootcampStateRepository _repository;

    public BootcampStateBusinessRules(IBootcampStateRepository repository)
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
