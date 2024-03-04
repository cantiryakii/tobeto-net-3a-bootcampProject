using AutoMapper;
using Business.Abstracts.ApplicationStates;
using Business.Constants;
using Business.Requests.ApplicationStates;
using Business.Responses.ApplicationStates;
using Business.Rules;
using Core.Exceptions.Types;
using Core.Utilities.Results;
using DataAccess.Abstracts;
using Entities.Concretes;

namespace Business.Concretes.ApplicationStates;

public class ApplicationStateManager : IApplicationStateService
{

    private readonly IApplicationStateRepository _applicationStateRepository;
    private readonly IMapper _mapper;
    private readonly ApplicationStateBusinessRules _applicationStateBusinessRules;

    public ApplicationStateManager(IApplicationStateRepository applicationStateRepository, IMapper mapper, ApplicationStateBusinessRules applicationStateBusinessRules)
    {
        _applicationStateRepository = applicationStateRepository;
        _mapper = mapper;
        _applicationStateBusinessRules = applicationStateBusinessRules;
    }

    public async Task<IDataResult<CreatedApplicationStateResponse>> AddAsync(CreateApplicationStateRequest request)
    {
        ApplicationState applicationState = _mapper.Map<ApplicationState>(request);
        await _applicationStateRepository.AddAsync(applicationState);
        CreatedApplicationStateResponse response = _mapper.Map<CreatedApplicationStateResponse>(applicationState);
        return new SuccessDataResult<CreatedApplicationStateResponse>(response, ApplicationStateMessages.ApplicationStateAdded);
    }

    public async Task<IResult> DeleteAsync(DeleteApplicationStateRequest request)
    {
        await _applicationStateBusinessRules.CheckIdIfNotExist(request.Id);

        var item = await _applicationStateRepository.GetAsync(x => x.Id == request.Id);
        await _applicationStateRepository.DeleteAsync(item);

        return new SuccessResult("Deleted Successfully");
    }

    public async Task<IDataResult<List<GetAllApplicationStateResponse>>> GetAllAsync()
    {
        var list = await _applicationStateRepository.GetAllAsync();
        List<GetAllApplicationStateResponse> response = _mapper.Map<List<GetAllApplicationStateResponse>>(list);
        return new SuccessDataResult<List<GetAllApplicationStateResponse>>(response, ApplicationStateMessages.ApplicationStateListed);
    }

    public async Task<IDataResult<GetByIdApplicationStateResponse>> GetByIdAsync(int id)
    {
        await _applicationStateBusinessRules.CheckIdIfNotExist(id);

        var item = await _applicationStateRepository.GetAsync(x => x.Id == id);

        GetByIdApplicationStateResponse response = _mapper.Map<GetByIdApplicationStateResponse>(item);

            return new SuccessDataResult<GetByIdApplicationStateResponse>(response, ApplicationStateMessages.ApplicationStateFound);
       
        
    }

    public async Task<IDataResult<UpdatedApplicationStateResponse>> UpdateAsync(UpdateApplicationStateRequest request)
    {
        var item = await _applicationStateRepository.GetAsync(p => p.Id == request.Id);
        if (request.Id == 0 || item == null)
        {
            return new ErrorDataResult<UpdatedApplicationStateResponse>(ApplicationStateMessages.ApplicationStateNotFound);
        }

        _mapper.Map(request, item);
        await _applicationStateRepository.UpdateAsync(item);

        UpdatedApplicationStateResponse response = _mapper.Map<UpdatedApplicationStateResponse>(item);
        return new SuccessDataResult<UpdatedApplicationStateResponse>(response, ApplicationStateMessages.ApplicationStateUpdated);
    }


}
