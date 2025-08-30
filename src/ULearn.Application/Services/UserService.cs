using System.Net;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Domain.Entities;
using ULearn.Domain.Enums;
using ULearn.Domain.Interfaces.Repository;
using ULearn.Domain.Shared;

namespace ULearn.Application.Services;

public class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _repository = repository;
    public async Task<Result<Guid>> CreateAsync(CreateUserDto dto)
    {
        var user = new User { Id = Guid.NewGuid(), FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = dto.Password, CreatedAt = DateTime.Now };
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
        var users = (await _repository.GetAllAsync()).Select(x => new UserDto(x.Id, x.FirstName, x.LastName, x.Email, x.Password, x.CreatedAt))
            .ToList()
            .AsReadOnly();    
        return Result.Success<IReadOnlyList<UserDto>>(users);
    }

    public async Task<Result<UserDto?>> GetByEmailAsync(string email)
    {

        var user = await _repository.GetByEmailAsync(email);
        return Result.Success(user is null ? null : new UserDto(user.Id, user.FirstName, user.LastName, user.Email, user.Password, user.CreatedAt));

    }

    public async Task<Result<UserDto?>> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        return Result.Success(user is null ? null : new UserDto(user.Id, user.FirstName, user.LastName, user.Email, user.Password, user.CreatedAt));
    }

    public async Task<Result> UpdateAsync(Guid id, CreateUserDto dto)
    {
        var exist = await _repository.GetByIdAsync(id);
        if (exist is null) Result.Failure(new Error(ErroCodeEnum.BadRequest, "Couldn't update."));
        var user = new User { Id = id, FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = dto.Password, CreatedAt = DateTime.Now };
        await _repository.UpdateAsync(user);
        return Result.Success();
    }
}