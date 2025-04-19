using Microsoft.AspNetCore.Mvc;
using IC_BikeTrainer_Backend.Services;
using IC_BikeTrainer_Backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace IC_BikeTrainer_Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class GodModeController : ControllerBase
    {
        private readonly IUserService _userService;
    
        public GodModeController(IUserService userService)
        {
            _userService = userService;
        }
        
        /// <summary>
        /// Retrieves a specific user by username.
        /// </summary>
        /// <remarks>
        /// This method returns the user information for the specified username.
        /// </remarks>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <response code="200">Returns the user details.</response>
        /// <response code="404">If the user with the specified username is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [Authorize(Roles = nameof(AuthRoles.Admin))]
        [HttpGet("{username}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user == null)
                    return NotFound(new { error = $"User '{username}' not found." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error.", details = ex.Message });
            }
        }
    
        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <remarks>
        /// This method returns a list of all registered users.
        /// </remarks>
        /// <response code="200">Returns the list of all users.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [Authorize(Roles = nameof(AuthRoles.Admin))]
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error.", details = ex.Message });
            }
        }
        
        /// <summary>
        /// Deletes a specific user by username.
        /// </summary>
        /// <remarks>
        /// This method allows admins to delete a specific user by username.
        /// </remarks>
        /// <param name="username">The username of the user to delete.</param>
        /// <response code="204">User successfully deleted.</response>
        /// <response code="404">If the user with the specified username is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [Authorize(Roles = nameof(AuthRoles.Admin))]
        [HttpDelete("{username}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try
            {
                await _userService.DeleteUserAsync(username);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound(new { error = $"User '{username}' not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error.", details = ex.Message });
            }
        }

        /// <summary>
        /// Deletes all users.
        /// </summary>
        /// <remarks>
        /// This method allows admins to delete all users in the system.
        /// </remarks>
        /// <response code="204">All users successfully deleted.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [Authorize(Roles = nameof(AuthRoles.Admin))]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAllUsers()
        {
            try
            {
                await _userService.DeleteAllUsersAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error.", details = ex.Message });
            }
        }
    }
}