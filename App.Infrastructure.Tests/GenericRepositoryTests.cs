using App.Domain.Entities;
using App.Domain.Interfaces.Repositories;
using App.Infrastructure.Data.Entities;
using App.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class GenericRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly Mock<ApplicationDBContext> _mockDbContext;
    private readonly Mock<DbSet<User>> _mockUserSet;
    private readonly IGenericRepository<UserDomain> _repository;

    public GenericRepositoryTests()
    {
        // 1️⃣ Configure DI container
        var services = new ServiceCollection();

        // AutoMapper via DI
        services.AddAutoMapper(cfg =>
        {
            cfg.CreateMap<User, UserDomain>()
               .ForMember(dest => dest.UserRoles,
                          opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)));

            cfg.CreateMap<UserDomain, User>()
               .ForMember(dest => dest.Roles, opt => opt.Ignore());
        });

        // 2️⃣ Mock DbContext and DbSet<User>
        _mockUserSet = new Mock<DbSet<User>>();
        _mockDbContext = new Mock<ApplicationDBContext>(new DbContextOptions<ApplicationDBContext>());
        _mockDbContext.Setup(c => c.Set<User>()).Returns(_mockUserSet.Object);

        services.AddSingleton(_mockDbContext.Object);
        services.AddScoped<IGenericRepository<UserDomain>, GenericRepository<UserDomain, User>>();

        _serviceProvider = services.BuildServiceProvider();

        // Resolve repository from DI
        _repository = _serviceProvider.GetRequiredService<IGenericRepository<UserDomain>>();
    }

    [Fact]
    public async Task AddAsync_ShouldMapAndAddEntity()
    {
        // Arrange
        var userDomain = new UserDomain
        {
            Email = "newuser@example.com",
            UserRoles = new List<string> { "User" }
        };

        _mockUserSet.Setup(u => u.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync((User u, System.Threading.CancellationToken _) => null);

        // Act
        await _repository.AddAsync(userDomain);

        // Assert
        _mockUserSet.Verify(u => u.AddAsync(It.IsAny<User>(), default), Times.Once);
    }

    [Fact]
    public async Task CommitAsync_ShouldCallSaveChanges()
    {
        // Arrange
        _mockDbContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _repository.CommitAsync();

        // Assert
        Assert.Equal(1, result);
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedDomainEntity_WhenEntityExists()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@example.com" };
        _mockUserSet.Setup(u => u.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(user);

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        // Arrange
        _mockUserSet.Setup(u => u.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _repository.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity_WhenEntityExists()
    {
        // Arrange
        var user = new User { Id = 1, Email = "delete@example.com" };
        _mockUserSet.Setup(u => u.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(user);

        // Act
        await _repository.DeleteAsync(1);

        // Assert
        _mockUserSet.Verify(u => u.Remove(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenEntityNotFound()
    {
        // Arrange
        _mockUserSet.Setup(u => u.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.DeleteAsync(99));
    }
}
