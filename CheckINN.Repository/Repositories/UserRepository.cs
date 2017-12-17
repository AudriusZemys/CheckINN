using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CheckINN.Repository.Contexts;
using CheckINN.Repository.Entities;

namespace CheckINN.Repository.Repositories
{
    /// <summary>
    /// This is only here to stop this repository and it's tests from failing.
    /// It has other use whatsoever.
    /// Swap this with an actual context with the actual entity to put it back into working code.
    /// </summary>
    public class DerrivedCheckINNContext : CheckINNContext
    {
        public virtual IDbSet<User> Users { get; set; }
    }

    public class UserRepository : IRepository<User>
    {
        private readonly Func<DerrivedCheckINNContext> _contextFactory;

        public UserRepository() {}

        public UserRepository(Func<DerrivedCheckINNContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Save(User item)
        {
            using (var context = _contextFactory.Invoke())
            {
                context.Users.Add(item);
                context.SaveChanges();
            }
        }

        public void SaveMany(IEnumerable<User> items)
        {
            throw new NotImplementedException();
        }

        private Tuple<byte[], byte[]> ComputeHash(string password, byte[] salt = null)
        {
            if (salt == null)
            {
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[256]);
            }
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var hashAlgo = new SHA512CryptoServiceProvider();
            var combined = new byte[salt.Length + passwordBytes.Length];
            passwordBytes.CopyTo(combined, 0);
            salt.CopyTo(combined, passwordBytes.Length);

            var hash = hashAlgo.ComputeHash(combined);

            return new Tuple<byte[], byte[]>(hash, salt);
        }

        public void NewUser(string username, string password)
        {
            var hash = ComputeHash(password);   
            var user = new User
            {
                Username = username,
                PasswordHash = hash.Item1,
                Salt = hash.Item2
            };

            Save(user);
        }

        public bool Authenticate(string username, string password)
        {
            using (var context = _contextFactory.Invoke())
            {
                var userRecord = context.Users.Single(user => string.Equals(username, user.Username));
                var expected = ComputeHash(password, userRecord.Salt);
                return userRecord.PasswordHash.SequenceEqual(expected.Item1);
            }
        }
    }
}
