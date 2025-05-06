using DTO;

namespace Business.Interfaces
{
    public interface IAdminUserService
    {
        Task<AdminUser> getAdminUser(string username);
        public Task changePassword(string username, string password);
    }
}