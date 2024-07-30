using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Repositories;

namespace super_ticketing_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public LoginController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult<LoginDto>> Login([FromBody]LoginRequestDto loginRequest)
        {
            var user = await _userRepository.Login(loginRequest);

            if (user == null)
            {
                return NotFound();
            }

            var loginDto = _mapper.Map<LoginDto>(user);

            return loginDto;
        }
    }
}
