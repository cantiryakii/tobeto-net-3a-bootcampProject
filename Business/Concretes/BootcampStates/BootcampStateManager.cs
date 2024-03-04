using AutoMapper;
using Business.Abstracts.Bootcamps;
using Business.Abstracts.BootcampStates;
using Business.Constants;
using Business.Requests.Bootcamps;
using Business.Requests.BootcampStates;
using Business.Responses.Bootcamps;
using Business.Responses.BootcampStates;
using Business.Rules;
using Core.Exceptions.Types;
using Core.Utilities.Results;
using DataAccess.Abstracts;
using DataAccess.Concretes.Repositories;
using Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concretes.BootcampStates;

public class BootcampStateManager:IBootcampStateService
{
    private readonly IBootcampStateRepository _bootcampStateRepository;
    private readonly IMapper _mapper;
    private readonly BootcampStateBusinessRules _bootcampStateBusinessRules;

    public BootcampStateManager(IBootcampStateRepository bootcampStateRepository, IMapper mapper, BootcampStateBusinessRules bootcampStateBusinessRules)
    {
        _bootcampStateRepository = bootcampStateRepository;
        _mapper = mapper;
        _bootcampStateBusinessRules = bootcampStateBusinessRules;
    }

    public async Task<IDataResult<CreatedBootcampStateResponse>> AddAsync(CreateBootcampStateRequest request)
    {
        BootcampState bootcampState = _mapper.Map<BootcampState>(request);
        await _bootcampStateRepository.AddAsync(bootcampState);
        CreatedBootcampStateResponse response = _mapper.Map<CreatedBootcampStateResponse>(bootcampState);
        return new SuccessDataResult<CreatedBootcampStateResponse>(response, BootcampStateMessages.BootcampStateAdded);
    }

    public async Task<IResult> DeleteAsync(DeleteBootcampStateRequest request)
    {
        await _bootcampStateBusinessRules.CheckIdIfNotExist(request.Id);

        var item = await _bootcampStateRepository.GetAsync(x=>x.Id == request.Id);
        await _bootcampStateRepository.DeleteAsync(item);
       
        return new SuccessResult("Deleted Successfully");
    }

    public async Task<IDataResult<List<GetAllBootcampStateResponse>>> GetAllAsync()
    {
        var list = await _bootcampStateRepository.GetAllAsync();
        List<GetAllBootcampStateResponse> response = _mapper.Map<List<GetAllBootcampStateResponse>>(list);
        return new SuccessDataResult<List<GetAllBootcampStateResponse>>(response, BootcampStateMessages.BootcampStateListed);
    }

    public async Task<IDataResult<GetByIdBootcampStateResponse>> GetByIdAsync(int id)
    {
        var item = await _bootcampStateRepository.GetAsync(x => x.Id == id);

        GetByIdBootcampStateResponse response = _mapper.Map<GetByIdBootcampStateResponse>(item);

        if (item != null)
        {
            return new SuccessDataResult<GetByIdBootcampStateResponse>(response, BootcampStateMessages.BootcampStateFound);
        }
        return new ErrorDataResult<GetByIdBootcampStateResponse>(BootcampStateMessages.BootcampStateNotFound);
    }

    public async Task<IDataResult<UpdatedBootcampStateResponse>> UpdateAsync(UpdateBootcampStateRequest request)
    {
        var item = await _bootcampStateRepository.GetAsync(p => p.Id == request.Id);
        if (request.Id == 0 || item == null)
        {
            return new ErrorDataResult<UpdatedBootcampStateResponse>(BootcampStateMessages.BootcampStateNotFound);
        }

        _mapper.Map(request, item);
        await _bootcampStateRepository.UpdateAsync(item);

        UpdatedBootcampStateResponse response = _mapper.Map<UpdatedBootcampStateResponse>(item);
        return new SuccessDataResult<UpdatedBootcampStateResponse>(response, BootcampStateMessages.BootcampStateUpdated);
    }


}
