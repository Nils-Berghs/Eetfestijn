using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Helpers
{
    internal static class FileSystemHelper
    {
        /// <summary>
        /// Gets a path to a file with the given name in a temp directory
        /// This is AppData/Local/Eetfestijn
        /// </summary>
        /// <param name="fileName">The name for the file </param>
        /// <returns></returns>
        internal static string GetTempPath(string fileName)
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(path, "EetFestijn", fileName);
        }
    }
}
