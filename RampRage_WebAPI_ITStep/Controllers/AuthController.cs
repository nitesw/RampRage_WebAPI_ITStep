using Core.Dtos.User;
using Core.Interfaces;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace RampRage_WebAPI_ITStep.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] UserRegisterDto dto)
        {
            try
            {
                var token = await authService.Register(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserAuthDto dto)
        {
            try
            {
                var token = await authService.Login(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
