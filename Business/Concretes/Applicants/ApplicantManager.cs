using AutoMapper;
using Business.Abstracts;
using Business.Constants;
using Business.Requests.Applicant;
using Business.Responses.Applicant;
using Business.Rules;
using Core.Exceptions.Types;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstracts;
using Entities.Concretes;

namespace Business.Concretes;

public class ApplicantManager : IApplicantService
{
    private readonly IApplicantRepository _applicantRepository;
    private readonly IMapper _mapper;
    private readonly ApplicantBusinessRules _applicantBusinessRules;

    public ApplicantManager(IApplicantRepository applicantRepository, IMapper mapper, ApplicantBusinessRules applicantBusinessRules)
    {
        _applicantRepository = applicantRepository;
        _mapper = mapper;
        _applicantBusinessRules = applicantBusinessRules;
    }

    public async Task<IDataResult<CreatedApplicantResponse>> AddAsync(CreateApplicantRequest request)
    {
        await _applicantBusinessRules.CheckUserNameIfExist(request.UserName, null);

        Applicant applicant = _mapper.Map<Applicant>(request);
        await _applicantRepository.AddAsync(applicant);
        CreatedApplicantResponse response = _mapper.Map<CreatedApplicantResponse>(applicant);
        return new SuccessDataResult<CreatedApplicantResponse>(response, ApplicantMessages.ApplicantAdded);
    }

    public async Task<IResult> DeleteAsync(DeleteApplicantRequest request)
    {
        await _applicantBusinessRules.CheckIdIfNotExist(request.Id);

        var item = await _applicantRepository.GetAsync(p => p.Id == request.Id);
        await _applicantRepository.DeleteAsync(item);
        return new SuccessResult("Deleted Successfully");
    }

    public async Task<IDataResult<List<GetAllApplicantResponse>>> GetAllAsync()
    {
        var list = await _applicantRepository.GetAllAsync();
        List<GetAllApplicantResponse> response = _mapper.Map<List<GetAllApplicantResponse>>(list);
        return new SuccessDataResult<List<GetAllApplicantResponse>>(response, ApplicantMessages.ApplicantListed);
    }

    public async Task<IDataResult<GetByIdApplicantResponse>> GetByIdAsync(int id)
    {
        await _applicantBusinessRules.CheckIdIfNotExist(id);

        var item = await _applicantRepository.GetAsync(x => x.Id == id);

        GetByIdApplicantResponse response = _mapper.Map<GetByIdApplicantResponse>(item);

        return new SuccessDataResult<GetByIdApplicantResponse>(response, ApplicantMessages.ApplicantFound);

    }

    public async Task<IDataResult<UpdatedApplicantResponse>> UpdateAsync(UpdateApplicantRequest request)
    {
        await _applicantBusinessRules.CheckIdIfNotExist(request.Id);
        await _applicantBusinessRules.CheckUserNameIfExist(request.UserName, request.Id);

        var item = await _applicantRepository.GetAsync(p => p.Id == request.Id);

        _mapper.Map(request, item);
        await _applicantRepository.UpdateAsync(item);

        UpdatedApplicantResponse response = _mapper.Map<UpdatedApplicantResponse>(item);
        return new SuccessDataResult<UpdatedApplicantResponse>(response, ApplicantMessages.ApplicantUpdated);
    }

}
