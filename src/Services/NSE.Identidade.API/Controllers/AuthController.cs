using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Extensions;
using NSE.Identidade.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }
        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {

            return StatusCode(401);
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
            if (result.Succeeded)
            {
                return CustomResponse(await GerarToken(usuarioRegistro.Email));
            }
            foreach (var erro in result.Errors)
            {
                AdicionarErroProcessamento(erro.Description);
            }
            return CustomResponse();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);
           
            if (result.Succeeded) return CustomResponse(await GerarToken(usuarioLogin.Email));

            if (result.IsLockedOut)
            {
                var x = _signInManager.UserManager.Users.Where(s => s.Email == usuarioLogin.Email).FirstOrDefault();
                AdicionarErroProcessamento($"O usuário está bloqueado por multiplas tentativas inválidas. {x.LockoutEnd}");
                return CustomResponse();
            }
            AdicionarErroProcessamento("Usuário ou senha inválidos.");
            return CustomResponse();
        }
        private async Task<UsuarioRespostaToken> GerarToken(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var claims = await _userManager.GetClaimsAsync(user);

                var identityClaims = await ObterClaimsUsuario(claims, user);

                var encodedToken = CodificarToken(identityClaims);

                return ObterRespostaToken(encodedToken, user, claims);
            }
            catch (Exception)
            {
                return new UsuarioRespostaToken();
            }
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
        {
            
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var item in roles)
            {
                claims.Add(new Claim("role", item));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);
            return identityClaims;

        }
        
        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            //Manipulador do token
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            //Crio o token com o manipulador de token
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm.FirstOrDefault(),
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoEmHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            //escrevo o token com o manipulador de token
            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }
        
        private UsuarioRespostaToken ObterRespostaToken(string encodedToken, IdentityUser user, IList<Claim> claims)
        {
            var response = new UsuarioRespostaToken
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoEmHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };
            return response;
        }


        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }
}
