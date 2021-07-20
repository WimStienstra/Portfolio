using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Learntennas.Repositories;
using MySql.Data.MySqlClient;
using WimStienstra.Models;

namespace WimStienstra.Repositories
{
    public class AboutRepository
    {
        public static About GetAbout()
        {
            using var connect = DbUtils.GetDbConnection();

            var about = connect.QuerySingleOrDefault<About>("SELECT * FROM about " +
                                                "JOIN image ON about.image_id = image.id" +
                                                "JOIN translation_link ON about.link_id = translation_link.id AND image.link_id = translation_link.id" +
                                                "JOIN translation ON translation_link.id = translation.id"
            );
            return about;
        }
        public static bool UpdateAbout(Translation about)
        {
            using var connect = DbUtils.GetDbConnection();
            var currentAbout = GetAbout();
            try
            {
                var translationId = LanguageRepository.GetTranslation(currentAbout.Link_Id);


                var updateAbout = connect.Execute("UPDATE translation SET title = @Brand_Name", new
                {
                    Image_Id = about.Title,
                    Link_Id = about.Description
                });

                return updateAbout != 0;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }
    }
}
