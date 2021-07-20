using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentValidation.Resources;
using Learntennas.Repositories;
using MySql.Data.MySqlClient;
using WimStienstra.Models;
using WimStienstra.Pages.Admin;

namespace WimStienstra.Repositories
{
    public class LanguageRepository
    {
        /// <summary>
        /// Gets the highest current translation link id
        /// </summary>
        /// <returns>Highest translationLink ID</returns>
        public static int GetHighestTranslationLinkId()
        {
            using var connect = DbUtils.GetDbConnection();
            return connect.QuerySingleOrDefault<int>("SELECT MAX(id) FROM translation_link");
        }

        /// <summary>
        /// Adds a language
        /// </summary>
        /// <param name="language">Language information</param>
        /// <returns>Success </returdsdns>
        public static bool AddLanguage(Languages language)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var languageResult = connect.Execute(
                    "INSERT INTO language (Language, Is_Default) VALUES (@Language, @Is_Default)", new
                    {
                        Language = language.Language,
                        Is_Default = language.Is_Default
                    });

                return languageResult == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds translation and sets the correct ID's in the translation_link table
        /// </summary>
        /// <param name="translations">List with all translations</param>
        /// <returns>Success boolean</returns>
        public static bool AddTranslation(List<Translation> translations)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var translationLinkId = GetHighestTranslationLinkId();
                foreach (var translation in translations)
                {
                    //Inserts the translation
                    var translationResult = connect.ExecuteScalar(
                        "INSERT INTO translation (Language_Id, Title, Description) VALUES (@Language_Id, @Title, @Description)",
                        new
                        {
                            translation.Language_Id,
                            translation.Title,
                            translation.Description
                        });

                    if (translationResult != null)
                    {
                        //Insert the correct ID's, all translations for a subject are the same ID, that correspond with the correct translation_id from the statement above
                        var translationLinkResult = connect.Execute(
                            "INSERT INTO translation_link (Id, Translation_Id) VALUES (@Id, @Translation_Id)", new
                            {
                                Id = translationLinkId,
                                Translation_Id = translationResult
                            });
                        return translationLinkResult == 1;
                    }
                }

                return false;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }
        /// <summary>
        /// Returns a single translation
        /// </summary>
        /// <param name="linkId">Translation Link ID</param>
        /// <returns>A translation</returns>
        public static Translation GetTranslation(int linkId)
        {
            using var connect = DbUtils.GetDbConnection();
            return connect.QuerySingleOrDefault<Translation>("SELECT * FROM translation JOIN translation_link ON translation_link.translation_id = translation.id translation_link.id = @LinkId",
                new
                {
                    LinkId = linkId
                });
        }

        public static int GetTranslationId(int linkId)
        {
            using var connect = DbUtils.GetDbConnection();
            return connect.QuerySingleOrDefault<int>("SELECT translation_id FROM translation_link WHERE id = @LinkId",
                new
                {
                    LinkId = linkId
                });
        }

    }
}
