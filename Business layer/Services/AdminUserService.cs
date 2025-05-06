using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using Business.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IRepository<AdminUser> AdminUser;
        IMapper mapper;

        public AdminUserService(IRepository<AdminUser> adminUserRepository, IMapper mapper)
        {
            this.AdminUser = adminUserRepository;
            this.mapper = mapper;
        }

        public async Task<DTO.AdminUser> getAdminUser(string username)
        {
            var user = await AdminUser.FirstOrDefaultAsync(u => u.Username == username);

            return mapper.Map<DTO.AdminUser>(user);
        }

        public async Task changePassword(string username, string oldPassword, string newPassword)
        {
            var adminUser = await AdminUser.FirstOrDefaultAsync(u => u.Username == username);

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, adminUser.PasswordHash)) throw new ArgumentException("Old password incorrect!");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            adminUser.PasswordHash = hashedPassword;

            await AdminUser.UpdateAndSaveAsync(adminUser);
        }
    }
}
