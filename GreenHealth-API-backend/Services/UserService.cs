using GreenHealth_API_backend.Data;
using GreenHealth_API_backend.Helpers;
using GreenHealth_API_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GreenHealth_API_backend.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly DataContext _context;

        public UserService(DataContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.User.FindAsync(id);
        }

		public async Task<User> GetUserWithOrganisation(int id)
		{
			return await _context.User.Include(u => u.Organisation).FirstOrDefaultAsync(u => u.Id == id);
		}

        public async Task<User> PutUser(int id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<User> PostUser(User user)
        {
			HashAlgorithm hashAlgorithm = SHA256.Create();
			hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
			user.Password = Encoding.UTF8.GetString(hashAlgorithm.Hash);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public (User user, string token) Authenticate(string username, string password)
        {
			HashAlgorithm hashAlgorithm = SHA256.Create();
			hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
			var user = _context.User.SingleOrDefault(x => x.Email == username && x.Password == Encoding.UTF8.GetString(hashAlgorithm.Hash));
            // return null if user not found
            if (user == null)
                return (user, null);
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHolder = tokenHandler.CreateToken(tokenDescriptor);
			//user.Token = tokenHandler.WriteToken(token);
			var token = tokenHandler.WriteToken(tokenHolder);
            // remove password before returning
            user.Password = null;
            return new (user, token);
        }
    }
}
