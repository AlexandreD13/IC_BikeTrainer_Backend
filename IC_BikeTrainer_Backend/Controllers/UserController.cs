using Microsoft.AspNetCore.Mvc;
using IC_BikeTrainer_Backend.Services;
using IC_BikeTrainer_Backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace IC_BikeTrainer_Backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        /// <summary>
        /// Updates the authenticated user's information.
        /// </summary>
        /// <remarks>
        /// This method updates the authenticated user's information such as firstname, lastname, email, weight, and height using the provided update request data.
        /// </remarks>
        /// <param name="request">An object containing updated details for the user.</param>
        /// <response code="200">User successfully updated.</response>
        /// <response code="404">If the user is not found or no changes were made.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [Authorize]
        [HttpPut("updateProfile")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOwnProfile([FromBody] UpdateUserRequest request)
        {
            try
            {
                var username = User.Identity?.Name;

                if (string.IsNullOrWhiteSpace(username))
                {
                    return StatusCode(500, new { error = "Invalid JWT token." });
                }

                var changes = await _userService.UpdateUserAsync(username, request);

                if (changes == 0)
                    return NotFound(new { error = "User not found or no changes made." });

                return Ok(new { message = "Your profile was updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error updating profile.", details = ex.Message });
            }
        }
    }
}
