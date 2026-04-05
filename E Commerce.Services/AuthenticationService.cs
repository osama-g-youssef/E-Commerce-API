using AutoMapper;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTO;
using E_Commerce.Shared.DTOs.OrderDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration,IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        public async Task<bool> CheckEmailAsync(string Email)
        {
            var User = await userManager.FindByEmailAsync(Email);
            return User != null;
        }


        public async Task<Result<UserDTO>> GetUserByEmailAsync(string Email)
        {
            var User =await userManager.FindByEmailAsync(Email); // currently loged in user
            if (User == null)
                return Error.NotFound("User.NotFound",$"No user with email{Email} was found");
            return new UserDTO(User.Email!, User.DisplayName, await CreateTokenAsync(User));
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null)
                return Error.InvalidCredintials("User.InvalidCrendentials");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordValid)
                return Error.InvalidCredintials("User.InvalidCrendentials");

            var Token = await CreateTokenAsync(user);
            return new UserDTO(user.Email!, user.DisplayName, Token);
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber,
            };

            var identityResult = await userManager.CreateAsync(user, registerDTO.Password);
            if (identityResult.Succeeded)
            {
                var Token = await CreateTokenAsync(user);
                return new UserDTO(user.Email!, user.DisplayName, Token);
            }

            return identityResult.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            // Token [Issuer, Audience, Claims, Expire, SigningCredintials(signature)]
            var Claims = new List<Claim>()
            {
               new Claim(JwtRegisteredClaimNames.Email, user.Email!),  // use consturctor that holds key and value
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            };
            //Roles
            var Roles = await userManager.GetRolesAsync(user);
            foreach (var role in Roles)
                Claims.Add(new Claim(ClaimTypes.Role, role));

            var SecurityKey = configuration["JWTOptions:SecurityKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
            var Cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            
            var Token = new JwtSecurityToken(
                issuer: configuration["JWTOptions:Issuer"],
                audience: configuration["JWTOptions:Audience"],
                claims: Claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: Cred
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);// return it on shape of string
        }
        public async Task<Result<AddressDTO>> GetUserAddressAsync(string Email)
        {
            var user = await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(X=>X.Email == Email);
            if (user == null)
                return Error.NotFound("user .Notfound", $"user with this email{Email} was not found");


            return mapper.Map<AddressDTO>(user.Address);


        }
        public async Task<Result<AddressDTO>> UpdateUserAddressAsync(string Email, AddressDTO addressDTO)
        {
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(X => X.Email == Email);
            if (user == null)
                return Error.NotFound("user .Notfound", $"user with this email{Email} was not found");

            if (user.Address is not null)
            {
                //update on the same object 
                user.Address.FirstName = addressDTO.FirstName;
                user.Address.LastName = addressDTO.LastName;
                user.Address.City = addressDTO.City;
                user.Address.Country = addressDTO.Country;
                user.Address.Street = addressDTO.Street;

            }
            else // create new address and assign it to the user
            {
                user.Address= mapper.Map<Address>(addressDTO);
            }

             await userManager.UpdateAsync(user);
             return mapper.Map<AddressDTO>(user.Address);



        }

    }
}
