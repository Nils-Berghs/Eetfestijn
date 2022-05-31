using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Helpers
{
    internal static class FileSystemHelper
    {
        /// <summary>
        /// Gets a path to a file or directory with the given name in a temp directory
        /// This is AppData/Local/Eetfestijn
        /// </summary>
        /// <param name="name">The name for the file or directory</param>
        /// <returns></returns>
        internal static string GetTempPath(string name)
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(path, "EetFestijn", name);
        }

        internal static string GetSessionPath(Session session, string fileName)
        {
            
            return Path.Combine(GetSessionDirectory(session), fileName);
        }

        internal static void CreateSessionPath(Session session)
        {
            Directory.CreateDirectory(GetSessionDirectory(session));

        }

        private static string GetSessionDirectory(Session session)
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(path, "EetFestijn", session.CreatedDateTime.ToString("yyyyMMdd_HHmm"));
        }
    }
}
