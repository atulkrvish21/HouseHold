using HHSurvey.Data;
using HHSurvey.Models;
using HHSurvey.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace HHSurvey.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context) {
           _context = context;
        }

        public bool FindUser(string username)
        {
            if (_context.UserLogin.Any(x => x.username == username))
            {
                return true;
            }
            else
                return false;
        }

        public void CreateUser(RegisterDTO user)
        {
            var userLogin = new UserLogin() {
                username = user.username,
                emailId = user.emailId,
                hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password) ,
                phone=user.phone,
                roleId=1,
                sstatus=1,
                block= user.block,
                district=user.district,
                panchayat=user.panchayat,
                state=user.state,
            
                entryDate =DateTime.Now,
                fullName = user.fullName
            };
            _context.UserLogin.Add(userLogin);
            _context.SaveChanges();
        }

        public UserLogin Authenticate(string username, string password)
        {
             var user = _context.UserLogin.SingleOrDefault(u => u.username == username);
          
           
            if (user == null)
            {
                return null;
            }
            else if (BCrypt.Net.BCrypt.Verify(password, user.hashedPassword))
            {
                return user;
            }
            else return null;

           
        }


        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            // 1. Find the User
            // Use FindAsync for primary key lookups, or SingleOrDefaultAsync if 'userId' is a unique non-primary key field (like username)
            var user = await _context.UserLogin
                .SingleOrDefaultAsync(u => u.username == userId); // Assuming Id is the field that matches the JWT Claim

            if (user == null)
            {
                // User not found (shouldn't happen if [Authorize] is used, but good practice)
                return false;
            }

            // 2. Verify Current Password
            // This helper method checks the plain-text password against the stored hash
            if (!BCrypt.Net.BCrypt.Verify( currentPassword, user.hashedPassword))
            {
                // Current password does not match the stored hash
                return false;
            }

            // 3. Hash the New Password
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // 4. Update the Password Hash and Save
            user.hashedPassword = newPasswordHash;

            // Optionally, reset security stamp if your framework uses it
            // user.SecurityStamp = Guid.NewGuid().ToString();

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Log the error (database failure, etc.)
                // Consider logging 'ex' for debugging
                return false;
            }
        }

    }
}
