using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Portfolio.Models;

namespace Portfolio.Repositories
{
    public class ImageRepository
    {
        /// <summary>
        /// Adds an image to the database
        /// </summary>
        /// <param name="image">Image information</param>
        /// <returns>Id of the added image</returns>
        public static int AddImage(Image image)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var result = connect.ExecuteScalar<int>(
                    "INSERT INTO image (Link_Id, Location) VALUES (@Link_Id, @Location); SELECT LAST_INSERT_ID();", new
                    {
                        Link_Id = image.Link_Id == 0 ? null : image.Link_Id,
                        image.Location
                    });
                return result;
            }
            catch (MySqlException e)
            {
                return 0;
            }
        }
    }
}
