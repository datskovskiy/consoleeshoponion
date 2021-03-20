using EShopOnion.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopOnion.DataAccess.Interfaces
{
    public interface IUser : IBaseEntity
    {
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public UserRoles UserRole { get; set; }
        public string Email { get; set; }
    }
}
