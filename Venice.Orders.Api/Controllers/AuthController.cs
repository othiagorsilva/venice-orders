using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Venice.Orders.Api.Auth;

namespace Venice.Orders.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService jwtTokenService;

        public AuthController(JwtTokenService jwtTokenService)
        {
            this.jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(Auth.LoginRequest), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] Auth.LoginRequest request)
        {
            if (jwtTokenService.UserValidator(request))
                return Unauthorized(new { Message = "Usuário ou senha inválidos" });

            var token = jwtTokenService.GenerateJwtToken(request);
            return Ok(new { Token = token });
        }
    }
}