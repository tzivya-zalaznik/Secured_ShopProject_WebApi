using Microsoft.AspNetCore.Mvc;
//using MyFirstWebApiSite;
using Services;
using System.Text.Json;
using Entities;
using AutoMapper;
using DTOs;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _1_13_03_2024_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        IUserService _userService;
        string filePath = "Users.txt";
        private IMapper _mapper;
        private readonly ILogger<userController> _logger;

        public userController(IUserService userService, IMapper mapper, ILogger<userController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            User user = await _userService.GetById(id);
            UserDTO userDTO = _mapper.Map<User, UserDTO>(user);
            if (userDTO != null)
                return Ok(userDTO);
            return NoContent();
        }

        [HttpPost]
        [Route("checkPassword")]
        public ActionResult<int> CheckPassword([FromBody] string password)
        {
            int result = _userService.CheckPassword(password);
            return Ok(result);
        }

        [HttpPost]
        //[Route("")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserDTO user)
        {
            int result = _userService.CheckPassword(user.Password);
            if (result <= 2)
            {
                return BadRequest();
            }
            else
            {
                User userToRegister = _mapper.Map<UserDTO, User>(user);
                User theRegisterUser = await _userService.Register(userToRegister);
                UserDTO newUser = _mapper.Map<User, UserDTO>(theRegisterUser);
                if (newUser != null)
                    return Ok(newUser);
                return NoContent();
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] UserLoginDTO user)
        {
            UserLogin userToLogin = _mapper.Map<UserLoginDTO, UserLogin>(user);
            User theUserLogin = await _userService.Login(userToLogin);
            _logger.LogInformation($"Loggin attemped with user name,{user.Email} and password {user.Password}");
            UserDTO userLogin = _mapper.Map<User, UserDTO>(theUserLogin);
            if (userLogin == null)
                return NoContent();
            else
                Response.Cookies.Append("X-Access-Token", userLogin.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok(userLogin);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> Update(int id, [FromBody] UserDTO user)
        {
            User userToUpdate = _mapper.Map<UserDTO, User>(user);
            int result = _userService.CheckPassword(userToUpdate.Password);
            if (result <= 2)
            {
                return BadRequest();
            }
            else
            {
        User updateUser = await _userService.Update(id, userToUpdate);
        UserDTO userToReturn = _mapper.Map<User, UserDTO>(updateUser);
                if (updateUser != null)
                {
                   return Ok(updateUser);
                }
                return NoContent();
            }   
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }
    }
}
