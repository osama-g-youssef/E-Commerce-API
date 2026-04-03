using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.IdentityDTO;
using E_Commerce.Shared.DTOs.OrderDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        // Login 
        // post BaseUrl/api/authentication/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var result = await authenticationService.LoginAsync(loginDTO);
            return HandleResult(result);
        }
        //Register
        // post BaseUrl/api/authentication/Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await authenticationService.RegisterAsync(registerDTO);
            return HandleResult(result);
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var exists = await authenticationService.CheckEmailAsync(email);
            return Ok(exists);
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser() //get user then get email from claims
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Result = await authenticationService.GetUserByEmailAsync(Email!);
            return HandleResult(Result);
        }
        [ProducesResponseType<AddressDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("Address")]
        //baseUrl/api/authentication/Address
        public async Task<ActionResult<AddressDTO>> GetAddress()// will get the email from the token
        {
            var Email = GetEmailFromToken();
            var Result = await authenticationService.GetUserAddressAsync(Email);
            return HandleResult(Result);

        }
        [Authorize]
        [HttpPut("Address")]
        //baseUrl/api/authentication/Address
        public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO addressDTO)// will get the email from the token
        {
            var Email = GetEmailFromToken();
            var Result = await authenticationService.UpdateUserAddressAsync(Email, addressDTO);
            return HandleResult(Result);
        }
    }
}
