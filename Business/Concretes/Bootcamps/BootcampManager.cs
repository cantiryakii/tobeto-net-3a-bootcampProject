using AutoMapper;
using Business.Abstracts.Bootcamps;
using Business.Constants;
using Business.Requests.ApplicationStates;
using Business.Requests.Bootcamps;
using Business.Responses.ApplicationStates;
using Business.Responses.Bootcamps;
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

namespace Business.Concretes.Bootcamps;

public class BootcampManager:IBootcampService
{
    private readonly IBootcampRepository _bootcampRepository;
    private readonly IMapper _mapper;
    private readonly BootcampBusinessRules _bootcampBusinessRules;

    public BootcampManager(IBootcampRepository bootcampRepository, IMapper mapper, BootcampBusinessRules bootcampBusinessRules)
    {
        _bootcampRepository = bootcampRepository;
        _mapper = mapper;
        _bootcampBusinessRules = bootcampBusinessRules;
    }

    public async Task<IDataResult<CreatedBootcampResponse>> AddAsync(CreateBootcampRequest request)
    {
        Bootcamp bootcamp = _mapper.Map<Bootcamp>(request);
        await _bootcampRepository.AddAsync(bootcamp);
        CreatedBootcampResponse response = _mapper.Map<CreatedBootcampResponse>(bootcamp);
        return new SuccessDataResult<CreatedBootcampResponse>(response, BootcampMessages.BootcampAdded);
    }

    public async Task<IResult> DeleteAsync(DeleteBootcampRequest request)
    {
        await _bootcampBusinessRules.CheckIdIfNotExist(request.Id);

        var item = await _bootcampRepository.GetAsync(x=> x.Id == request.Id);  
        await _bootcampRepository.DeleteAsync(item);
        
        return new SuccessResult("Deleted Successfully");
    }

    public async Task<IDataResult<List<GetAllBootcampResponse>>> GetAllAsync()
    {
        var list = await _bootcampRepository.GetAllAsync();
        List<GetAllBootcampResponse> response = _mapper.Map<List<GetAllBootcampResponse>>(list);
        return new SuccessDataResult<List<GetAllBootcampResponse>>(response, BootcampMessages.BootcampListed);
    }

    public async Task<IDataResult<GetByIdBootcampResponse>> GetByIdAsync(int id)
    {
        await _bootcampBusinessRules.CheckIdIfNotExist(id);

        var item = await _bootcampRepository.GetAsync(x => x.Id == id);

        GetByIdBootcampResponse response = _mapper.Map<GetByIdBootcampResponse>(item);

            return new SuccessDataResult<GetByIdBootcampResponse>(response, BootcampMessages.BootcampFound);
       
    }

    public async Task<IDataResult<UpdatedBootcampResponse>> UpdateAsync(UpdateBootcampRequest request)
    {
        var item = await _bootcampRepository.GetAsync(p => p.Id == request.Id);
        if (request.Id == 0 || item == null)
        {
            return new ErrorDataResult<UpdatedBootcampResponse>(BootcampMessages.BootcampNotFound);
        }

        _mapper.Map(request, item);
        await _bootcampRepository.UpdateAsync(item);

        UpdatedBootcampResponse response = _mapper.Map<UpdatedBootcampResponse>(item);
        return new SuccessDataResult<UpdatedBootcampResponse>(response, BootcampMessages.BootcampUpdated);
    }

}
