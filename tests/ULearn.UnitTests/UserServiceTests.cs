using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ULearn.Application.DTOs;
using ULearn.Application.Services;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repository;
using Xunit;

namespace ULearn.UnitTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_repo.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidDto_ShouldCallRepositoryAndReturnNewId()
        {
            // Arrange
            var dto = new CreateUserDto("A", "B", "a@b.com", "pass");
            var newId = Guid.NewGuid();
            _repo.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(newId);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().Be(newId);
            _repo.Verify(r => r.CreateAsync(It.Is<User>(u =>
                u.FirstName == dto.FirstName &&
                u.LastName == dto.LastName &&
                u.Email == dto.Email &&
                u.Password == dto.Password
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_GivenValidId_ShouldCallRepositoryDelete()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _repo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_ShouldReturnAllUserDtos()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), FirstName = "X", LastName = "Y", Email = "x@y.com", Password = "p", CreatedAt = DateTime.UtcNow },
                new User { Id = Guid.NewGuid(), FirstName = "P", LastName = "Q", Email = "p@q.com", Password = "p", CreatedAt = DateTime.UtcNow }
            };
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Value.Should().HaveCount(2);
            result.Value.Select(u => u.Email).Should().BeEquivalentTo(["x@y.com", "p@q.com"]);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetByIdAsync_VariousScenarios_ShouldReturnDtoOrNull(bool exists)
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = exists
                ? new User { Id = id, FirstName = "Justin", LastName = "Nabunturan", Email = "justin@player.com", Password = "pw", CreatedAt = DateTime.UtcNow }
                : null;
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            if (exists)
            {
                result.Should().NotBeNull().And.Match<UserDto>(u => u.Email == user!.Email);
            }
            else
            {
                result.Should().BeNull();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetByEmailAsync_VariousScenarios_ShouldReturnDtoOrNull(bool exists)
        {
            // Arrange
            var email = "user@example.com";
            var user = exists
                ? new User { Id = Guid.NewGuid(), FirstName = "X", LastName = "Y", Email = email, Password = "pw", CreatedAt = DateTime.UtcNow }
                : null;
            _repo.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _service.GetByEmailAsync(email);

            // Assert
            if (exists)
            {
                result.Should().NotBeNull().And.Match<UserDto>(u => u.Email == email);
            }
            else
            {
                result.Should().BeNull();
            }
        }

        [Fact]
        public async Task UpdateAsync_WithValidDto_ShouldCallRepositoryWithMappedUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new CreateUserDto("U", "S", "u@s.com", "pwd");

            // Act
            await _service.UpdateAsync(id, dto);

            // Assert
            _repo.Verify(r => r.UpdateAsync(It.Is<User>(u =>
                u.Id == id &&
                u.FirstName == dto.FirstName &&
                u.Email == dto.Email
            )), Times.Once);
        }
    }
}
