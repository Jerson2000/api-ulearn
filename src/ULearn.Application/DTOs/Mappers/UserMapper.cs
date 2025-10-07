

using ULearn.Domain.Entities;

namespace ULearn.Application.DTOs.Mappers;

public static class UserMapper
{
    public static UserDto ToUserDto(this User user)
    {
        return new UserDto(user.Id, user.FirstName, user.LastName, user.Email, user.Password, user.CreatedAt);
    }
    public static User DtoToUserEntity(this CreateUserDto dto)
    {
        return new User { Id = Guid.NewGuid(), FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = dto.Password, CreatedAt = DateTime.Now };
    }

    public static User DtoToUserEntity(this CreateUserDto dto, Guid id)
    {
        return new User { Id = id, FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = dto.Password, CreatedAt = DateTime.Now };
    }


    #region User Response Mapper

    public static UserResponseDto ToUserResponseDto(this User user)
    {
        return new UserResponseDto(user.Id, user.FirstName, user.LastName, user.Email, user.CreatedAt);
    }
    
    #endregion



}