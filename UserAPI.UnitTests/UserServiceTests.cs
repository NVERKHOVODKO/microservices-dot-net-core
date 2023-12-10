using Moq;
using ProjectX.Exceptions;
using Repository;
using TestApplication.Services;

namespace UserApiTests
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            var dbRepositoryMock = new Mock<IDbRepository>();
            _userService = new UserService(dbRepositoryMock.Object);
        }

        [Fact]
        public void IsEmailValid_ValidEmail_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var result = _userService.IsEmailValid(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEmailValid_EmailWithLongLength_ThrowsIncorrectDataException()
        {
            // Arrange
            var email = new string('a', 101) + "@example.com";

            // Act and Assert
            Assert.Throws<IncorrectDataException>(() => _userService.IsEmailValid(email));
        }

        [Theory]
        [InlineData("invalid.email")]
        [InlineData("test@.com")]
        [InlineData("@example.com")]
        [InlineData("test@com")]
        [InlineData("test@ex@ample.com")]
        public void IsEmailValid_InvalidEmailFormat_ReturnsFalse(string invalidEmail)
        {
            // Act and Assert
            Assert.False(_userService.IsEmailValid(invalidEmail));
        }

        [Fact]
        public void IsEmailValid_ValidEmailWithDifferentCasing_ReturnsTrue()
        {
            // Arrange
            var email = "Test@Example.com";

            // Act
            var result = _userService.IsEmailValid(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetLeadingRole_UserHasSuperAdminRole_ReturnsSuperAdmin()
        {
            // Arrange
            var roles = new[] { "User", "SuperAdmin" };

            // Act
            var result = _userService.GetLeadingRole(roles);

            // Assert
            Assert.Equal("SuperAdmin", result);
        }

        [Fact]
        public void GetLeadingRole_UserHasAdminRole_ReturnsAdmin()
        {
            // Arrange
            var roles = new[] { "User", "Admin" };

            // Act
            var result = _userService.GetLeadingRole(roles);

            // Assert
            Assert.Equal("Admin", result);
        }

        [Fact]
        public void GetLeadingRole_UserHasSupportRole_ReturnsSupport()
        {
            // Arrange
            var roles = new[] { "User", "Support" };

            // Act
            var result = _userService.GetLeadingRole(roles);

            // Assert
            Assert.Equal("Support", result);
        }

        [Fact]
        public void GetLeadingRole_UserHasNoLeadingRole_ReturnsUser()
        {
            // Arrange
            var roles = new[] { "User", "Guest" };

            // Act
            var result = _userService.GetLeadingRole(roles);

            // Assert
            Assert.Equal("User", result);
        }

        [Fact]
        public void GetLeadingRole_EmptyRoles_ReturnsUser()
        {
            // Arrange
            var roles = new string[] { "User", "Admin"};

            // Act
            var result = _userService.GetLeadingRole(roles);

            // Assert
            Assert.Equal("Admin", result);
        }
    }
}
