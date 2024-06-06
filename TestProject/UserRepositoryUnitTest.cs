using Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class UserRepositoryUnitTest
    {
        [Fact]
        public async Task GetById_ExistingUser_ReturnsUser()
        {
            // Arrange
            int userId = 1;
            var user = new User { UserId = userId, Email = "test@example.com", Password = "password" };

            var mockContext = new Mock<AdoNetUsers214956807Context>();
            mockContext.Setup(x => x.Users.FindAsync(userId)).ReturnsAsync(user);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.GetById(userId);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsUser()
        {
            // Arrange
            var user = new User { FirstName = "Test", LastName = "Test", Email = "test2@example.com", Password = "password" };

            var users = new List<User>();
            var mockContext = new Mock<AdoNetUsers214956807Context>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.Register(user);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var user = new User { Email = "login@example.com", Password = "password" };

            var mockContext = new Mock<AdoNetUsers214956807Context>();
            var users = new List<User> { user };
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            var userLogin = new UserLogin { Email = user.Email, Password = user.Password };

            // Act
            var result = await userRepository.Login(userLogin);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Update_ExistingUser_UpdatesUser()
        {
            // Arrange
            var user = new User { FirstName = "dvori", LastName = "rottman", Email = "dvori@gmail.com", Password = "password" };
            var updatedUser = new User { FirstName = "updated", LastName = "user", Email = "updated@gmail.com", Password = "newpassword" };

            var mockSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<AdoNetUsers214956807Context>();

            mockSet.Setup(m => m.Update(It.IsAny<User>()));
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.Update(user.UserId, updatedUser);

            // Assert
            Assert.Equal("updated@gmail.com", result.Email);
        }
    }
}