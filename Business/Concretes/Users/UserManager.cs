using AutoMapper;
using Azure.Core;
using Business.Abstracts.User;
using Business.Constants;
using Business.Requests.Instructor;
using Business.Requests.User;
using Business.Responses.Instructor;
using Business.Responses.User;
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

namespace Business.Concretes;

public class UserManager : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly UserBusinessRules _userBusinessRules;

    public UserManager(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userBusinessRules = userBusinessRules;
    }

    public async Task<IDataResult<CreatedUserResponse>> AddAsync(CreateUserRequest request)
    {
        User user = _mapper.Map<User>(request);
        await _userRepository.AddAsync(user);
        CreatedUserResponse response = _mapper.Map<CreatedUserResponse>(user);
        return new SuccessDataResult<CreatedUserResponse>(response,UserMessages.UserAdded);
    }

    public async Task<IResult> DeleteAsync(DeleteUserRequest request)
    {
        await _userBusinessRules.CheckIdIfNotExist(request.Id);
        var item = await _userRepository.GetAsync(x=> x.Id == request.Id);
        await _userRepository.DeleteAsync(item);
        
        return new SuccessResult("Deleted Successfully");
    }

    public async Task<IDataResult<List<GetAllUserResponse>>> GetAllAsync()
    {
        var list = await _userRepository.GetAllAsync();
        List<GetAllUserResponse> response = _mapper.Map<List<GetAllUserResponse>>(list);
        return new SuccessDataResult<List<GetAllUserResponse>>(response, UserMessages.UserListed);
    }

    public async Task<IDataResult<GetByIdUserResponse>> GetByIdAsync(int id)
    {
        await _userBusinessRules.CheckIdIfNotExist(id);

        var item = await _userRepository.GetAsync(x => x.Id == id);

        GetByIdUserResponse response = _mapper.Map<GetByIdUserResponse>(item);

        if (item != null)
        {
            return new SuccessDataResult<GetByIdUserResponse>(response, UserMessages.UserFound);
        }
        return new ErrorDataResult<GetByIdUserResponse>(UserMessages.UserNotFound);
    }

    public async Task<IDataResult<UpdatedUserResponse>> UpdateAsync(UpdateUserRequest request)
    {
        var item = await _userRepository.GetAsync(p => p.Id == request.Id);
        if (request.Id == 0 || item == null)
        {
            return new ErrorDataResult<UpdatedUserResponse>(UserMessages.UserNotFound);
        }

        _mapper.Map(request, item);
        await _userRepository.UpdateAsync(item);

        UpdatedUserResponse response = _mapper.Map<UpdatedUserResponse>(item);
        return new SuccessDataResult<UpdatedUserResponse>(response, UserMessages.UserUpdated);
    }

}
