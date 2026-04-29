using App.Domain.Entities;
using App.Infrastructure.Data.Entities;
using App.Infrastructure.Mappings;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Drawing.Drawing2D;
using Xunit;

namespace App.Infrastructure.Tests
{
    public class UserTokenMappingTests
    {
        [Fact]
        public void UserTokenMapping_ShouldMapRefreshTokenExpiryTime()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { { "Jwt:RefreshTokenExpiryInDays", "7" } })
                .Build();

            // Set up DI container
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>(); // For AutoMapper's internal needs
            services.AddTransient<RefreshTokenExpiryResolverInfra>(); // Explicitly register the resolver
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserTokenMappingExtensionInfra>();
            });
            var serviceProvider = services.BuildServiceProvider();

            // Create mapper with DI
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            var source = new UserTokenDomain();

            // Act
            var result = mapper.Map<UserToken>(source);

            // Assert
            Assert.Equal(DateTime.UtcNow.AddDays(7).Date, result.RefreshTokenExpiryTime?.Date);
        }
    }
}