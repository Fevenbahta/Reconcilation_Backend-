using LIB.API.Application.CQRS.Login.Request.Command;
using LIB.API.Application.CQRS.Login.Request.Queries;
using LIB.API.Application.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
namespace LIB_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public LoginController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            var use = await _mediator.Send(new GetLoginList());
            return Ok(use);
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {



            var user = await _mediator.Send(command);
            if (user != null)
            {

                var tokenString = GenerateJwtToken(user.UserName);
                // Additional data to be returned along with the token
                var responseData = new
                {
                    Token = tokenString,
                    UserData = user // Assuming user is the data you want to return
                };
                return Ok(responseData);
            }
            // If user is null, return unauthorized
            return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username), "Username cannot be null or empty.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            // Generate a random 16-byte key
            var keyBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            // Include username in claims only if it's provided
            tokenDescriptor.Subject = new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.Name, username)
            });

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        [HttpPut]
        public async Task<ActionResult> Register([FromBody] UserDto userDto)
        {
            userDto.Password=BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            var command = new RegisterCommand { userDto = userDto };
            await _mediator.Send(command);
            return Ok(command);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var query = new GetLoginRequest { UserId = userId };
            var user = await _mediator.Send(query);

            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }
        [HttpPut("Admin/{id}")]

        public async Task<ActionResult> Put([FromBody] UserDto use)
        {
            // Check if the password is less than 10 characters
            if (use.Password.Length < 10)
            {
                // Hash the password if it's less than 10 characters
                use.Password = BCrypt.Net.BCrypt.HashPassword(use.Password);
            }

            var command = new UpdateCommand { userDto = use };
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(long id, [FromBody] UserDto use, string old, string oldInput)
        {
            try
            {

                // Hash the old password inputted by the user
                // string hashedOldInput = BCrypt.Net.BCrypt.HashPassword(oldInput);

                // Compare hashedOldInput with old
                if (!BCrypt.Net.BCrypt.Verify(oldInput, old))
                {
                    return BadRequest("Old password is incorrect.");
                }
                // Hash the new password
                use.Password = BCrypt.Net.BCrypt.HashPassword(use.Password);

                // Update the user using mediator pattern (assuming _mediator is properly configured)
                var command = new UpdateCommand { userDto = use };
                await _mediator.Send(command);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // DELETE api/<BlacklistController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteCommand { Id = id };
            await _mediator.Send(command);
               return NoContent();
        }


    }
}
