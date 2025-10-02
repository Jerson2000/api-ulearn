using System.Net;
using ULearn.Application.DTOs;
using ULearn.Application.DTOs.Mappers;
using ULearn.Application.Interfaces;
using ULearn.Domain.Entities;
using ULearn.Domain.Enums;
using ULearn.Domain.Interfaces.Repository;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;

namespace ULearn.Application.Services;

public class UserService(IUserRepository repository, IEmailService emailService) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IEmailService _emailService = emailService;
    public async Task<Result<Guid>> CreateAsync(CreateUserDto dto)
    {
        var user = dto.DtoToUserEntity();
        await _repository.CreateAsync(user);
        return Result.Success(user.Id);
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var user = _repository.GetByIdAsync(id);
        if (user is null) Result.Failure(new Error(ErroCodeEnum.BadRequest, "Couldn't delete."));
        await _repository.DeleteAsync(id);
        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<UserDto>>> GetAllAsync()
    {
        var users = (await _repository.GetAllAsync()).Select(x => x.ToUserDto())
            .ToList()
            .AsReadOnly();
        return Result.Success<IReadOnlyList<UserDto>>(users);
    }

    public async Task<Result<UserDto?>> GetByEmailAsync(string email)
    {

        var user = await _repository.GetByEmailAsync(email);
        return Result.Success(user?.ToUserDto());

    }

    public async Task<Result<UserDto?>> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        return Result.Success(user?.ToUserDto());
    }

    public async Task<Result> UpdateAsync(Guid id, CreateUserDto dto)
    {
        var exist = await _repository.GetByIdAsync(id);
        if (exist is null) Result.Failure(new Error(ErroCodeEnum.BadRequest, "Couldn't update."));        
        await _repository.UpdateAsync(dto.DtoToUserEntity(id));
        return Result.Success();
    }
}