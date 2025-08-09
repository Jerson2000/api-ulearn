using Moq;
using ULearn.Application.DTOs;
using ULearn.Application.Services;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repository;
using FluentAssertions;

namespace ULearn.UnitTests;


public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repo = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(_repo.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_CallsRepositoryAndReturnsId()
    {

        var dto = new CreateUserDto("A", "B", "a@b.com", "pass");
        var newId = Guid.NewGuid();
        _repo.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(newId);

        var result = await _service.CreateAsync(dto);

        result.Should().Be(newId);
        _repo.Verify(r => r.CreateAsync(It.Is<User>(u =>
            u.FirstName == dto.FirstName &&
            u.LastName == dto.LastName &&
            u.Email == dto.Email &&
            u.Password == dto.Password)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepository()
    {
        var id = Guid.NewGuid();

        await _service.DeleteAsync(id);

        _repo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), FirstName = "X", LastName = "Y", Email = "x@y.com", Password = "p", CreatedAt = DateTime.UtcNow },
            new User { Id = Guid.NewGuid(), FirstName = "P", LastName = "Q", Email = "p@q.com", Password = "p", CreatedAt = DateTime.UtcNow }
        };
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Select(u => u.Email).Should().Contain(new[] { "x@y.com", "p@q.com" });
    }

    [Fact]
    public async Task GetByIdAsync_UserExists_ReturnsDto()
    {
        var user = new User { Id = Guid.NewGuid(), FirstName = "Justin", LastName = "Nabunturan", Email = "justin@player.com", Password = "pw", CreatedAt = DateTime.UtcNow };
        _repo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var result = await _service.GetByIdAsync(user.Id);

        result.Should().NotBeNull();
        result.Email.Should().Be("justin@player.com");
    }

    [Fact]
    public async Task GetByIdAsync_UserNotExist_ReturnsNull()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_UserExists_ReturnsDto()
    {
        var user = new User { Id = Guid.NewGuid(), FirstName = "X", LastName = "Y", Email = "xy@example.com", Password = "pw", CreatedAt = DateTime.UtcNow };
        _repo.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

        var result = await _service.GetByEmailAsync(user.Email);

        result.Should().NotBeNull();
        result.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_UserNotExist_ReturnsNull()
    {
        _repo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var result = await _service.GetByEmailAsync("not@exists.com");

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ValidDto_CallsRepositoryWithCorrectUser()
    {
        var id = Guid.NewGuid();
        var dto = new CreateUserDto("U", "S", "u@s.com", "pwd");

        await _service.UpdateAsync(id, dto);

        _repo.Verify(r => r.UpdateAsync(It.Is<User>(u =>
            u.Id == id &&
            u.FirstName == dto.FirstName &&
            u.Email == dto.Email)), Times.Once);
    }
}