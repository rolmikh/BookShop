using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Book_Shop.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API_Book_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Book_ShopContext _context;

        public UsersController(Book_ShopContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int? id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int? id, User user)
        {
            if (id != user.IdUser)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'Book_ShopContext.Users'  is null.");
          }
            HashAndSalt hashAndSalt = new HashAndSalt();
            user.SaltUser = hashAndSalt.CreateSalt(10);
            user.PasswordUser = hashAndSalt.GenerateHash(user.PasswordUser, user.SaltUser);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.IdUser }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int? id)
        {
            return (_context.Users?.Any(e => e.IdUser == id)).GetValueOrDefault();
        }


        [HttpPost("Autorization")]
        public IActionResult Token(User user)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            object response = null;
            HashAndSalt hashAndSalt = new HashAndSalt();
            var users = _context.Users.ToList();
            foreach (var autoUser in users)
            {
                if(autoUser.EmailUser == user.EmailUser.Trim())
                {
                    bool salt = false;
                    salt = hashAndSalt.AreEqual(user.PasswordUser, autoUser.PasswordUser, autoUser.SaltUser);
                    if(salt = true)
                    {
                        var identity = GetIdentity(user.EmailUser, autoUser);
                        if (identity == null)
                        {
                            return BadRequest(new { errorText = "Invalid username or password." });
                        }

                        var now = DateTime.UtcNow;
                        // создаем JWT-токен
                        var jwt = new JwtSecurityToken(
                                issuer: AuthOptions.ISSUER,
                                audience: AuthOptions.AUDIENCE,
                                notBefore: now,
                                claims: identity.Claims,
                                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                        response = new
                        {
                            access_token = encodedJwt,
                            username = identity.Name,
                            role = autoUser.RoleId,
                            id = autoUser.IdUser
                        };
                    }
                    else
                    {
                        return BadRequest("Неуспешная авторизация!");
                    }
                }
            }

            if(response == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(response);
            }
        }

        private ClaimsIdentity GetIdentity(string email, User user)
        {
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.EmailUser),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleId.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        [HttpPost("refresh_token")]
        public async Task<ActionResult> RefreshToken(string access_token)
        {
            var email = GetTokenInfo(access_token);
            User user = _context.Users.FirstOrDefault(x => x.EmailUser == email);

            var identity = GetIdentityRefresh(user.EmailUser, user.PasswordUser);

            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создание JWT-токен
            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
        }

        private string GetTokenInfo(string token)
        {
            var t = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return t.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
        }

        private ClaimsIdentity GetIdentityRefresh(string username, string password)
        {
            User person = _context.Users.FirstOrDefault(x => x.EmailUser == username && x.PasswordUser == password.ToString());
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.EmailUser),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.RoleId.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }



    }
}
