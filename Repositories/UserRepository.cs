using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Portfolio.Repositories;
using Portfolio.Models;
using Portfolio.Pages.Shared;

namespace Portfolio.Repositories
{
    public class UserRepository
    {

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user">New user information</param>
        /// <returns>True if success</returns>
        public static bool AddUser(User user)
        {
            using var db = DbUtils.GetDbConnection();
            var registerUser = db.Execute(
                @"INSERT INTO user (email, password, salt) 
                    VALUES (@Email, @Password, @Salt)", new
                {
                    Email = user.Email,
                    Password = user.Password,
                    Salt = user.Salt
                });

            return registerUser == 1;
        }

        /// <summary>
        /// Gets a userid by login details
        /// </summary>
        /// <param name="email">Login email</param>
        /// <param name="password">Login password</param>
        /// <returns>0 if no user is found, userId if found</returns>
        public static int GetUserByLogin(string email, string password)
        {
            using var db = DbUtils.GetDbConnection();
            User result = db.QueryFirstOrDefault<User>(
                "SELECT id, password, salt FROM user WHERE email = @Email", new
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