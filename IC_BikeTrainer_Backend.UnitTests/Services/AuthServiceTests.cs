using IC_BikeTrainer_Backend.Models;
using IC_BikeTrainer_Backend.Repositories;
using IC_BikeTrainer_Backend.Services;
using IC_BikeTrainer_Backend.Tests;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace IC_BikeTrainer_Backend.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IContext> _mockContext;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockContext = new Mock<IContext>();
            _authService = new AuthService(_mockContext.Object);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ReturnsUser_WhenCredentialsAreValid()
        {
            var username = "john";
            var password = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User { Username = username, Password = hashedPassword };
            _mockContext.Setup(c => c.GetByUsernameAsync(username))
                        .ReturnsAsync(user);

            var result = await _authService.AuthenticateUserAsync(username, password);

            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ReturnsNull_WhenUserNotFound()
        {
            _mockContext.Setup(c => c.GetByUsernameAsync(It.IsAny<string>()))
                        .ReturnsAsync((User)null!);

            var result = await _authService.AuthenticateUserAsync("wrong", "pass");
            Assert.Null(result);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ReturnsNull_WhenPasswordIsWrong()
        {
            var username = "john";
            var user = new User { Username = username, Password = BCrypt.Net.BCrypt.HashPassword("correctpass") };

            _mockContext.Setup(c => c.GetByUsernameAsync(username))
                        .ReturnsAsync(user);

            var result = await _authService.AuthenticateUserAsync(username, "wrongpass");
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterAsync_AddsUserAndReturnsId()
        {
            var mockSet = new Mock<DbSet<User>>();
            var users = new List<User>();
            mockSet.Setup(m => m.AddAsync(It.IsAny<User>(), default))
                   .Callback<User, CancellationToken>((u, _) => {
                       u.Id = 42;
                       users.Add(u);
                   })
                   .ReturnsAsync((User u, CancellationToken _) => new Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>(null!));

            _mockContext.Setup(m => m.UsersTable).Returns(mockSet.Object);
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            var request = new RegisterRequest
            {
                Username = "test",
                Password = "testpass",
                Firstname = "John",
                Lastname = "Doe",
                Email = "john@doe.com",
                Weight = 75,
                Height = 180
            };

            var result = await _authService.RegisterAsync(request);

            Assert.Equal(42, result);
        }

        [Fact]
        public async Task UserExistsByUsernameAsync_ReturnsTrue_WhenExists()
        {
            var users = new List<User>
            {
                new User { Username = "john" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
    
            mockSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<User>(users.GetEnumerator()));
    
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<User>(users.Provider));
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression)
                .Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType)
                .Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator())
                .Returns(users.GetEnumerator());
    
            _mockContext.Setup(c => c.UsersTable).Returns(mockSet.Object);

            var exists = await _authService.UserExistsByUsernameAsync("john");

            Assert.True(exists);
        }
    }
}
