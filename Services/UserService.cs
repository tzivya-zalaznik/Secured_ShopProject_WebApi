using Repository;
using Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using System.Text.Json;

namespace Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public int CheckPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;
        }
        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);

        }
        public async Task<User> Register(User user)
        {
            return await _userRepository.Register(user);
        }
        public async Task<User> Login(UserLogin userLogin)
        {
            var user = await _userRepository.Login(userLogin);
            if(user!=null) user.Token = generateJwtToken(user);
            return user;
        }
        public async Task<User> Update(int id, User userToUpdate)
        {
            return await _userRepository.Update(id, userToUpdate);
        }

        private string generateJwtToken(User user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("key").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
