using System.ComponentModel;
using FluentValidation;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repository;

namespace ULearn.Application.Services;

public class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _repository = repository;


    public async Task<Guid> CreateAsync(CreateUserDto dto)
    {
        var user = new User { Id = Guid.NewGuid(), FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = dto.Password, CreatedAt = DateTime.Now };
        return await _repository.CreateAsync(user);
    }

    public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);

    public async Task<IEnumerable<UserDto>> GetAllAsync() => (await _repository.GetAllAsync()).Select(x => new UserDto(x.Id, x.FirstName, x.LastName, x.Email, x.Password, x.CreatedAt));

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? null : new UserDto(item.Id, item.FirstName, item.LastName, item.Email, item.Password, item.CreatedAt);
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var item = await _repository.GetByEmailAsync(email);
        return item is null ? null : new UserDto(item.Id, item.FirstName, item.LastName, item.Email, item.Password, item.CreatedAt);
    }

    public Task UpdateAsync(Guid id, CreateUserDto dto)
    {
        var user = new User { Id = id, FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = dto.Password, CreatedAt = DateTime.Now };
        return _repository.UpdateAsync(user);
    }
}