using EShopOnion.DataAccess.Enums;
using EShopOnion.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopOnion.DataAccess.Entities
{
    public class User : BaseEntity, IUser
    {
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public UserRoles UserRole { get; set; } = UserRoles.User;
        public string Email { get; set; }
    }
}
