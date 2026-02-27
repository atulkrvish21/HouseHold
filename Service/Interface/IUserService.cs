using HHSurvey.Models;

namespace HHSurvey.Service.Interface
{
    public interface IUserService
    {
        public void CreateUser(RegisterDTO user);
        public bool FindUser(string username);
        public UserLogin Authenticate(string username, string password);
        public Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    }
}
