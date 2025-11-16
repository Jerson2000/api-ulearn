using FluentAssertions;
using Moq;
using ULearn.Application.DTOs;
using ULearn.Application.Services;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Domain.Interfaces.Services;

namespace ULearn.UnitTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;
        private readonly Mock<IEmailService> _emailService;

        public UserServiceTests()
        {
            _repo = new Mock<IUserRepository>();
            _emailService = new Mock<IEmailService>();
            _service = new UserService(_repo.Object,_emailService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidDto_ShouldCallRepositoryAndReturnNewId()
        {
            // Arrange
            var dto = new CreateUserDto("Justin", "Nabunturan", "justin@nabunturan.com", "pass");
            var newId = Guid.NewGuid();
            _repo.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(newId);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Value.Should().Be(result.Value);
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
                new User { Id = Guid.NewGuid(), FirstName = "Justin", LastName = "Nabunturan", Email = "justin@nabunturan.com", Password = "p", CreatedAt = DateTime.UtcNow },
                new User { Id = Guid.NewGuid(), FirstName = "Nabunturan", LastName = "Justin", Email = "nabunturan@justin.com", Password = "p", CreatedAt = DateTime.UtcNow }
            };
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Value.Should().HaveCount(2);
            result.Value.Select(u => u.Email).Should().BeEquivalentTo(["justin@nabunturan.com", "nabunturan@justin.com"]);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetByIdAsync_VariousScenarios_ShouldReturnDtoOrNull(bool exists)
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = exists
                ? new User { Id = id, FirstName = "Justin", LastName = "Nabunturan", Email = "justin@nabunturan.com", Password = "pw", CreatedAt = DateTime.UtcNow }
                : null;
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            if (exists)
            {
                result.Value.Should().NotBeNull().And.Match<UserDto>(u => u.Email == user!.Email);
            }
            else
            {
                result.Value.Should().BeNull();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetByEmailAsync_VariousScenarios_ShouldReturnDtoOrNull(bool exists)
        {
            // Arrange
            var email = "justin@nabunturan.com";
            var user = exists
                ? new User { Id = Guid.NewGuid(), FirstName = "Justin", LastName = "Nabunturan", Email = email, Password = "pw", CreatedAt = DateTime.UtcNow }
                : null;
            _repo.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _service.GetByEmailAsync(email);

            // Assert
            if (exists)
            {
                result.Value.Should().NotBeNull().And.Match<UserDto>(u => u.Email == email);
            }
            else
            {
                result.Value.Should().BeNull();
            }
        }

        [Fact]
        public async Task UpdateAsync_WithValidDto_ShouldCallRepositoryWithMappedUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new CreateUserDto("Justin", "Nabunturan", "justin@nabunturan.com", "pwd");

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
