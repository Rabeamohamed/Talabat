using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            , ITokenService tokenService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email already exists"));
            }

            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
            };

            var Result = await _userManager.CreateAsync(User, model.Password);

            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));

            var ReturnedUser = new UserDto()
            {

                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokesAsync(User, _userManager)

            };
            return Ok(ReturnedUser);


        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User == null) return Unauthorized(new ApiResponse(401));

            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);

            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));


            return Ok(new UserDto()
            {

                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokesAsync(User, _userManager)

            });

        }


        [Authorize]
        [HttpGet("GetCurrentUser")] // /api/Accounts/Getcurrentuser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);

            var ReturnedObject = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokesAsync(user, _userManager)
            };
            return Ok(ReturnedObject);
        }

        [Authorize]
        [HttpGet("Address")] // /api/Accounts/Address

        public async Task<ActionResult<Address>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(MappedAddress);

        }

        [Authorize]
        [HttpPut("Address")] // /api/Accounts/Address

        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto UpdatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<AddressDto, Address>(UpdatedAddress);
            MappedAddress.Id = user.Address.Id;
            user.Address = MappedAddress;
            var Result = await _userManager.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedAddress);
        }

        [HttpGet("EmailExists")] // /api/Accounts/EmailExists
        public async Task<ActionResult<bool>> CheckEmailExists(string Email)
        {
            //var User = await _userManager.FindByEmailAsync(email);
            //if (User is null) return false;
            //return true;

            return await _userManager.FindByEmailAsync(Email) is not null;
        }
    }
}