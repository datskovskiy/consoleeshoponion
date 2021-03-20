using EShopOnion.DataAccess.Entities;
using EShopOnion.DataAccess.Interfaces;
using EShopOnion.Repository.Interfaces;
using EShopOnion.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EShopOnion.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IUnitOfWork unitOfWork)
        {
            _userRepository = unitOfWork.Repository<User>();
        }
        public User Login(string username, string password)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username), "cant be null.");

            if (username.Trim() == string.Empty)
                throw new ArgumentException("Username cant be empty.");

            if (password == null)
                throw new ArgumentNullException(nameof(password), "cant be null.");

            if (password.Trim() == string.Empty)
                throw new ArgumentException("Password cant be empty.");         

            var user = _userRepository.List(x => x.UserName == username).FirstOrDefault();

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public User Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            if (user == null)
                throw new ArgumentNullException(nameof(user), "cant be null.");

            if (password == null)
                throw new ArgumentNullException(nameof(password), "cant be null.");

            if (password.Trim() == string.Empty)
                throw new ArgumentException("Password cant be empty.");

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            int maxIndex = !_userRepository.List().Any() ? 0 : _userRepository.List().Max(u => u.Id);

            user.Id = maxIndex + 1;

            _userRepository.Create(user);

            return user;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password), "cant be null.");

            if (password.Trim() == string.Empty)
                throw new ArgumentException("Password cant be empty.");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool UserExists(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username), "cant be null.");

            if (username.Trim() == string.Empty)
                throw new ArgumentException("Username cant be empty.");

            return _userRepository.List(x => x.UserName == username).FirstOrDefault() != null;
        }

        public void UpdateUser(IUser user, string newEmail)
        {
            user.Email = newEmail;
        }

        public IReadOnlyList<User> GetUsers()
        {
            return _userRepository.List().ToList();
        }
    }
}
