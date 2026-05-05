using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using App.Infrastructure.Data.Entities;
using App.Infrastructure.Repositories;
using App.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly Mock<ApplicationDBContext> _mockDbContext;
    private readonly Mock<DbSet<User>> _mockUserSet;
    private readonly Mock<DbSet<Role>> _mockRoleSet;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        // 1️⃣ Configure DI container
        var services = new ServiceCollection();

        // AutoMapper via DI (no manual MapperConfiguration)
        services.AddAutoMapper(cfg =>
        {
            cfg.CreateMap<User, UserDomain>()
               .ForMember(dest => dest.UserRoles,
                          opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)));

            cfg.CreateMap<UserDomain, User>()
               .ForMember(dest => dest.Roles, opt => opt.Ignore());
        });

        // 2️⃣ Mock DbContext and DbSets
        _mockUserSet = new Mock<DbSet<User>>();
        _mockRoleSet = new Mock<DbSet<Role>>();
        _mockDbContext = new Mock<ApplicationDBContext>(new DbContextOptions<ApplicationDBContext>());
        _mockDbContext.Setup(c => c.Users).Returns(_mockUserSet.Object);
        _mockDbContext.Setup(c => c.Roles).Returns(_mockRoleSet.Object);

        services.AddSingleton(_mockDbContext.Object);
        services.AddScoped<UserRepository>();

        _serviceProvider = services.BuildServiceProvider();

        // Resolve repository from DI
        _repository = _serviceProvider.GetRequiredService<UserRepository>();
    }

    [Fact]
    public async Task GetUserByEmailAsync_ReturnsMappedUser_WhenEmailExists()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "Admin" };
        var user = new User { Id = 1, Email = "test@example.com", Roles = new List<Role> { role } };
        var data = new List<User> { user }.AsQueryable();

        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        // Act
        var result = await _repository.GetUserByEmailAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
        Assert.Contains("Admin", result.UserRoles);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ReturnsNull_WhenEmailNotFound()
    {
        // Arrange
        var data = new List<User>().AsQueryable();
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        // Act
        var result = await _repository.GetUserByEmailAsync("notfound@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddUserAsync_ReturnsOne_WhenRoleExists()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "User" };
        var userDomain = new UserDomain
        {
            Email = "newuser@example.com",
            UserRoles = new List<string> { "User" }
        };

        var rolesData = new List<Role> { role }.AsQueryable();
        _mockRoleSet.As<IQueryable<Role>>().Setup(m => m.Provider).Returns(rolesData.Provider);
        _mockRoleSet.As<IQueryable<Role>>().Setup(m => m.Expression).Returns(rolesData.Expression);
        _mockRoleSet.As<IQueryable<Role>>().Setup(m => m.ElementType).Returns(rolesData.ElementType);
        _mockRoleSet.As<IQueryable<Role>>().Setup(m => m.GetEnumerator()).Returns(rolesData.GetEnumerator());

        _mockUserSet.Setup(u => u.AddAsync(It.IsAny<User>(), default)).ReturnsAsync((User u, System.Threading.CancellationToken _) => null);
        _mockDbContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _repository.AddUserAsync(userDomain);

        // Assert
        Assert.Equal(1, result);
        _mockUserSet.Verify(u => u.AddAsync(It.IsAny<User>(), default), Times.Once);
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task AddUserAsync_ReturnsZero_WhenRoleNotExists()
    {
        // Arrange
        var userDomain = new UserDomain
        {
            Email = "nouser@example.com",
            UserRoles = new List<string> { "NonExistentRole" }
        };

        var rolesData = new List<Role>().AsQueryable();
        _mockRoleSet.As<IQueryable<Role>>().Setup(m => m.Provider).Returns(rolesData.Provider);
        _mockRoleSet.As<IQueryable<Role>>().Setup(m => m.Expression).Returns(rolesData.Expression);
        _mockRoleSet.As<IQueryable<Role>>().Setup(m => m.ElementType).Returns(rolesData.ElementType);
        _mockRoleSet.As<IQueryable<Role>>().Setup(m => m.GetEnumerator()).Returns(rolesData.GetEnumerator());

        // Act
        var result = await _repository.AddUserAsync(userDomain);

        // Assert
        Assert.Equal(0, result);
        _mockUserSet.Verify(u => u.AddAsync(It.IsAny<User>(), default), Times.Never);
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }
}
