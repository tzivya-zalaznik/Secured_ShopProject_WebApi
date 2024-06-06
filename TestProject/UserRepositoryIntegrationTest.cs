using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class UserRepositoryIntegrationTest: IClassFixture<DatabaseFixture>
    {
        private readonly AdoNetUsers214956807Context _dbContext;
        private readonly UserRepository _userRepository;

        public UserRepositoryIntegrationTest(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _userRepository = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task GetById_ExistingUserId_ReturnsUser()
        {
            // Arrange
            var user = new User { FirstName = "Test", LastName = "Test", Email = "test2@example.com", Password = "password" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            int userId = user.UserId;

            // Act
            var result = await _userRepository.GetById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task Register_ValidUser_SavesAndReturnsUser()
        {
            // Arrange
            var user = new User { FirstName = "New", LastName = "User", Email = "newuser@example.com", Password = "password" };

            // Act
            var result = await _userRepository.Register(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var email = "loginuser@example.com";
            var password = "password";
            User user = new User { FirstName = "Login", LastName = "User", Email = email, Password = password };
            var userLogin = new UserLogin { Email = email, Password = password };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.Login(userLogin);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task Update_ValidIdAndUser_UpdatesAndReturnsUser()
        {
            //Arrange
            var user = new User { Email = "updateuser@example.com", Password = "password", FirstName = "Eve", LastName = "Johnson" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var updatedUser = new User { Email = "updateduser@example.com", Password = "newpassword", FirstName = "UpdatedName", LastName = "UpdatedLastName" };
            _dbContext.Entry(user).State = EntityState.Detached;
            updatedUser.UserId = user.UserId;
            //Act
            var result = await _userRepository.Update(user.UserId, updatedUser);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("updateduser@example.com", result.Email);
        }
    }
}

