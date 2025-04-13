using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using IC_BikeTrainer_Backend.Services;
using IC_BikeTrainer_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

namespace IC_BikeTrainer_Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
    
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Authenticates a user using the provided credentials.
        /// </summary>
        /// <remarks>
        /// This method validates the login credentials included in the request and returns the user's ID if authentication is successful.
        /// </remarks>
        /// <param name="request">An object containing the username and password for login.</param>
        /// <response code="200">User successfully logged in.</response>
        /// <response code="400">If the request is missing required fields or the credentials are invalid.</response>
        /// <response code="403">If the user does not have proper authorization.</response>
        /// <response code="404">If the specified user could not be found.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [EnableRateLimiting("LoginLimiter")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
                return BadRequest(new { error = "Missing 'username' field." });

            if (string.IsNullOrEmpty(request.Password))
                return BadRequest(new { error = "Missing 'password' field." });

            var user = await _userService.AuthenticateUserAsync(request.Username, request.Password);
            if (user == null)
                return BadRequest(new { error = "Invalid 'username' or 'password'." });
            
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return BadRequest(new { error = "Invalid 'username' or 'password'." });

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                authToken = token,
                userId = user.Id
            });
        }
    
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <remarks>
        /// This method creates a new user using the details provided in the registration request. The input is validated for completeness and uniqueness before creating the user.
        /// </remarks>
        /// <param name="request">An object containing the necessary registration details including username, password, firstname, lastname, email, weight, and height.</param>
        /// <response code="201">User successfully created.</response>
        /// <response code="400">If the request is missing required fields or the provided data is invalid.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
                return BadRequest(new { error = "Missing 'username' field." });
            if (string.IsNullOrEmpty(request.Password))
                return BadRequest(new { error = "Missing 'password' field." });
            if (string.IsNullOrEmpty(request.Firstname))
                return BadRequest(new { error = "Missing 'firstname' field." });
            if (string.IsNullOrEmpty(request.Lastname))
                return BadRequest(new { error = "Missing 'lastname' field." });
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest(new { error = "Missing 'email' field." });
            if (request.Weight == null)
                return BadRequest(new { error = "Missing 'weight' field." });
            if (request.Height == null)
                return BadRequest(new { error = "Missing 'height' field." });
    
            if (await _userService.UserExistsByUsernameAsync(request.Username))
                return BadRequest(new { error = $"Username '{request.Username}' is already taken." });
    
            if (await _userService.UserExistsByEmailAsync(request.Email))
                return BadRequest(new { error = $"Email '{request.Email}' is already taken." });
    
            try
            {
                request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var userId = await _userService.RegisterAsync(request);
                var user = await _userService.GetUserByUsernameAsync(request.Username);
                var token = GenerateJwtToken(user);

                return Created("", new
                {
                    message = $"User '{request.Username}' created.",
                    userId,
                    authToken = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Error creating user '{request.Username}'.", details = ex.Message });
            }
        }
    
        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <remarks>
        /// This method updates the specified user's information such as firstname, lastname, email, weight, and height using the provided update request data. If the user is not found or no changes are made, the appropriate response is returned.
        /// </remarks>
        /// <param name="username">The username of the user to be updated.</param>
        /// <param name="request">An object containing updated details for the user.</param>
        /// <response code="200">User successfully updated.</response>
        /// <response code="404">If the user is not found or no changes were made.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [Authorize]
        [HttpPut("{username}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var changes = await _userService.UpdateUserAsync(username, request);
                if (changes == 0)
                    return NotFound(new { error = $"User '{username}' not found or no changes made." });
    
                return Ok(new { message = $"User '{username}' updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error updating user '{username}'.", details = ex.Message });
            }
        }
        
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var jwtKey = _configuration["Jwt:Key"]
                         ?? throw new InvalidOperationException("JWT key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
