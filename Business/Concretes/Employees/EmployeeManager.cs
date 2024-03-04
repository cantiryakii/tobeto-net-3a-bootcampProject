using AutoMapper;
using Business.Abstracts.Employee;
using Business.Constants;
using Business.Requests.Applications;
using Business.Requests.Employee;
using Business.Responses.Applications;
using Business.Responses.Employee;
using Business.Rules;
using Core.Exceptions.Types;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstracts;
using DataAccess.Concretes.Repositories;
using Entities.Concretes;

namespace Business.Concretes;

public class EmployeeManager : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    private readonly EmployeeBusinessRules _employeeBusinessRules;

    public EmployeeManager(IEmployeeRepository employeeRepository, IMapper mapper, EmployeeBusinessRules employeeBusinessRules)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _employeeBusinessRules = employeeBusinessRules;
    }

    public async Task<IDataResult<CreatedEmployeeResponse>> AddAsync(CreateEmployeeRequest request)
    {
        await _employeeBusinessRules.CheckUserNameIfExist(request.UserName, null);

        Employee employee = _mapper.Map<Employee>(request);
        await _employeeRepository.AddAsync(employee);
        CreatedEmployeeResponse response = _mapper.Map<CreatedEmployeeResponse>(employee);
        return new SuccessDataResult<CreatedEmployeeResponse>(response, EmployeeMessages.EmployeeAdded);
    }

    public async Task<IResult> DeleteAsync(DeleteEmployeeRequest request)
    {
        await _employeeBusinessRules.CheckIdIfNotExist(request.Id);

        var item = await _employeeRepository.GetAsync(x=>x.Id == request.Id);
        await _employeeRepository.DeleteAsync(item);
        
        return new SuccessResult("Deleted Successfully");
    }

    public async Task<IDataResult<List<GetAllEmployeeResponse>>> GetAllAsync()
    {
        var list = await _employeeRepository.GetAllAsync();
        List<GetAllEmployeeResponse> response = _mapper.Map<List<GetAllEmployeeResponse>>(list);
        return new SuccessDataResult<List<GetAllEmployeeResponse>>(response, EmployeeMessages.EmployeeListed);
    }

    public async Task<IDataResult<GetByIdEmployeeResponse>> GetByIdAsync(int id)
    {
        await _employeeBusinessRules.CheckIdIfNotExist(id);

        var item = await _employeeRepository.GetAsync(x => x.Id == id);

        GetByIdEmployeeResponse response = _mapper.Map<GetByIdEmployeeResponse>(item);

            return new SuccessDataResult<GetByIdEmployeeResponse>(response, EmployeeMessages.EmployeeFound);
       
        
    }

    public async Task<IDataResult<UpdatedEmployeeResponse>> UpdateAsync(UpdateEmployeeRequest request)
    {
        await _employeeBusinessRules.CheckIdIfNotExist(request.Id);
        await _employeeBusinessRules.CheckUserNameIfExist(request.UserName, request.Id);

        var item = await _employeeRepository.GetAsync(p => p.Id == request.Id);

        _mapper.Map(request, item);
        await _employeeRepository.UpdateAsync(item);

        UpdatedEmployeeResponse response = _mapper.Map<UpdatedEmployeeResponse>(item);
        return new SuccessDataResult<UpdatedEmployeeResponse>(response, EmployeeMessages.EmployeeUpdated);
    }

}
