using backend.Interface.Services;
using backend.Model.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices services;

        public UserController(IUserServices services)
        {
            this.services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await services.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpGet("UserId")]
        public async Task<IActionResult> GetUserById(string UserId)
        {
            var result = await services.GetUserByIdAsync(UserId);
            return Ok(result);
        }

        [HttpGet("Name")]
        public async Task<IActionResult> GetUserByName(string Name)
        {
            var result = await services.GetUserByNameAsync(Name);
            return Ok(result);
        }

        [HttpGet("UserName")]
        public async Task<IActionResult> GetUserByUserName(string UserName)
        {
            var result = await services.GetUserByUserNameAsync(UserName);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserRequestDto user)
        {
            var result = await services.UpdateUserAsync(user);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await services.DeleteUserAsync(id);
            return Ok(result);
        }


        [HttpGet("Search")]
        public async Task<IActionResult> SearchUser(string search)
        {
            var result = await services.SearchUserAsync(search);
            return Ok(result);
        }
    }
}
