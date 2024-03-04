﻿using AutoMapper;
using Business.Abstracts.Applications;
using Business.Abstracts.Blacklists;
using Business.Constants;
using Business.Requests.Applications;
using Business.Responses.Applications;
using Business.Rules;
using Core.Exceptions.Types;
using Core.Utilities.Results;
using DataAccess.Abstracts;
using DataAccess.Concretes.Repositories;
using Entities.Concretes;

namespace Business.Concretes.Applications;

public class ApplicationManager : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;
    private readonly ApplicationBusinessRules _applicationBusinessRules;

    public ApplicationManager(IApplicationRepository applicationRepository, IMapper mapper, ApplicationBusinessRules applicationBusinessRules)
    {
        _applicationRepository = applicationRepository;
        _mapper = mapper;
        _applicationBusinessRules = applicationBusinessRules;
    }

    public async Task<IDataResult<CreatedApplicationResponse>> AddAsync(CreateApplicationRequest request)
    {
        await _applicationBusinessRules.CheckIfApplicantIsBlacklisted(request.ApplicantId);

        Application application = _mapper.Map<Application>(request);
        await _applicationRepository.AddAsync(application);
        CreatedApplicationResponse response = _mapper.Map<CreatedApplicationResponse>(application);
        return new SuccessDataResult<CreatedApplicationResponse>(response, ApplicationMessages.ApplicationAdded);
    }

    public async Task<IResult> DeleteAsync(DeleteApplicationRequest request)
    {
        var item = await _applicationRepository.GetAsync(x=>x.Id == request.Id);
        await _applicationRepository.DeleteAsync(item);
        
        return new SuccessResult("Deleted Successfully");
    }

    public async Task<IDataResult<List<GetAllApplicationResponse>>> GetAllAsync()
    {
        var list = await _applicationRepository.GetAllAsync();
        List<GetAllApplicationResponse> response = _mapper.Map<List<GetAllApplicationResponse>>(list);
        return new SuccessDataResult<List<GetAllApplicationResponse>>(response, ApplicationMessages.ApplicationListed);
    }

    public async Task<IDataResult<GetByIdApplicationResponse>> GetByIdAsync(int id)
    {
        var item = await _applicationRepository.GetAsync(x => x.Id == id);

        GetByIdApplicationResponse response = _mapper.Map<GetByIdApplicationResponse>(item);

        if (item != null)
        {
            return new SuccessDataResult<GetByIdApplicationResponse>(response, ApplicationMessages.ApplicationFound);
        }
        return new ErrorDataResult<GetByIdApplicationResponse>(ApplicationMessages.ApplicationNotFound);
    }

    public async Task<IDataResult<UpdatedApplicationResponse>> UpdateAsync(UpdateApplicationRequest request)
    {
        var item = await _applicationRepository.GetAsync(p => p.Id == request.Id);
        if (request.Id == 0 || item == null)
        {
            return new ErrorDataResult<UpdatedApplicationResponse>(ApplicationMessages.ApplicationNotFound);
        }

        _mapper.Map(request, item);
        await _applicationRepository.UpdateAsync(item);

        UpdatedApplicationResponse response = _mapper.Map<UpdatedApplicationResponse>(item);
        return new SuccessDataResult<UpdatedApplicationResponse>(response, ApplicationMessages.ApplicationUpdated);
    }




}
