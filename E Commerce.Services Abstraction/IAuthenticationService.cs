using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services_Abstraction
{
    public interface IAuthenticationService
    {
        // Login takes email and pass and if sucess => token , display name , email  ------- 2 DTOs 1 for request and 1 for response
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);

        // Register takes email , pass , display name , userName, PhoneNumber=> token , display name , email ------- 2 DTOs 1 for request and 1 for response  ====total 3 DTOs
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);

        Task<bool> CheckEmailAsync(string Email);
        Task<Result<UserDTO>> GetUserByEmailAsync(string Email);

    }
}
