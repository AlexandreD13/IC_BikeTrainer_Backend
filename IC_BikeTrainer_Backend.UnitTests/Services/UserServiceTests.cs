using IC_BikeTrainer_Backend.Models;
using IC_BikeTrainer_Backend.Repositories;
using IC_BikeTrainer_Backend.Services;
using IC_BikeTrainer_Backend.Tests;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace IC_BikeTrainer_Backend.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IContext> _mockContext;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockContext = new Mock<IContext>();
            _userService = new UserService(_mockContext.Object);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesAndSaves_WhenUserExists()
        {
            var user = new User { Username = "john", Firstname = "OldName", Weight = 75, Height = 180 };
            var users = new List<User> { user }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<User>(users.GetEnumerator()));

            mockSet.As<IQueryable<User>>()
                .Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
            mockSet.As<IQueryable<User>>()
                .Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>()
                .Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>()
                .Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var mockContext = new Mock<IContext>();
            mockContext.Setup(c => c.UsersTable).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var userService = new UserService(mockContext.Object);

            var request = new UpdateUserRequest { Firstname = "Bill" };

            var result = await userService.UpdateUserAsync("john", request);

            Assert.Equal(1, result);
            Assert.Equal("Bill", user.Firstname);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsList()
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

            var result = await _userService.GetAllUsersAsync();

            Assert.Single(result);
            Assert.Equal("john", result.First().Username);
        }
    }
}
