using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GraphLabs.Backend.Api.Auth
{
    public sealed class UserService
    {
        private readonly GraphLabsContext _ctx;
        private readonly PasswordHashCalculator _hashCalculator;
        private readonly AuthSettings _appSettings;

        public UserService(IOptions<AuthSettings> appSettings, GraphLabsContext ctx, PasswordHashCalculator hashCalculator)
        {
            _ctx = ctx;
            _hashCalculator = hashCalculator;
            _appSettings = appSettings.Value;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest login)
        {
            var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Email == login.Email);
            if (user == null)
                return null;
            
            var hash = _hashCalculator.Calculate(login.Password, user.PasswordSalt);
            if (!hash.SequenceEqual(user.PasswordHash))
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new LoginResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}