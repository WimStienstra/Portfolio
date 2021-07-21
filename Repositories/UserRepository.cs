using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brides.Pages.Shared;
using Dapper;
using Learntennas.Repositories;
using WimStienstra.Models;

namespace WimStienstra.Repositories
{
    public class UserRepository
    {

        public static User AddUser(User user)
        {
            using var db = DbUtils.GetDbConnection();
            user.Id = db.ExecuteScalar<int>(
                @"INSERT INTO user (email, password, salt) 
                    VALUES (@Email, @Password, @Salt); SELECT LAST_INSERT_ID()", new
                {
                    Email = user.Email,
                    Password = user.Password,
                    Salt = user.Salt
                });

            return user;
        }

        public static int GetUserByLogin(string email, string password)
        {
            using var db = DbUtils.GetDbConnection();
            User result = db.QueryFirstOrDefault<User>(
                "SELECT id, password, salt FROM users WHERE email = @Email", new
                {
                    Email = email
                });
            if (result != null)
            {
                string encryptedPass = Convert.ToBase64String(UserMethods.GenerateSaltedHash(Encoding.ASCII.GetBytes(password), Convert.FromBase64String(result.Salt)));
                if (result.Password == encryptedPass)
                    return result.Id;
            }

            return 0;
        }
    }
}
