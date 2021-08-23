

using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Portfolio.Models;
using Portfolio.Repositories;

namespace Portfolio.Pages.Shared
{
    public class UserMethods
    {
        /// <summary>
        /// Generates a salt
        /// </summary>
        /// <param name="maximumSaltLength">Length of salt</param>
        /// <returns>byte[] salt</returns>
        public static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        /// <summary>
        /// Generates salted hash
        /// </summary>
        /// <param name="plainText">Password</param>
        /// <param name="salt">Salt</param>
        /// <returns>byte[] saltedhash</returns>
        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }


        /// <summary>
        /// Capitalizes every word in the string
        /// </summary>
        /// <param name="value">String to be capitalized</param>
        /// <returns>Capitalized string</returns>
        public static string CapitalizeString(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.  
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.  
            // ... Uppercase the lowercase letters following spaces.  
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        public int SaveImage(IFormFile photo, int id)
        {
            if (photo != null)
            {
                Image image = new Image();
                var path = Path.Combine(Path.GetFullPath("wwwroot"), "images", id + " - " + photo.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    photo.CopyToAsync(stream);
                    //TODO: add link id to images
                    //image.Link_Id = Image.Link_Id = TranslationLink.Id;
                    image.Location = photo.FileName;
                    return ImageRepository.AddImage(image);
                }
            }
            return 0;
        }
    }
}
