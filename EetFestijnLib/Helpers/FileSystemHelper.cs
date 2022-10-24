using be.berghs.nils.EetFestijnLib.Models;
using be.berghs.nils.EetFestijnLib.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace be.berghs.nils.EetFestijnLib.Helpers
{
    internal static class FileSystemHelper
    {
        private const string SESSION_FILE_NAME = "Session.json";
        private const string SESSION_DIR_FORMAT = "yyyyMMdd_HHmm";
        private const string ORDER_FILE_START = "Order-";

        /// <summary>
        /// Gets the root of the temp path
        /// </summary>
        /// <returns></returns>
        private static string GetTempPath()
        {
            string path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EetFestijn");
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Gets a path to a file or directory with the given name in a temp directory
        /// This is AppData/Local/Eetfestijn
        /// </summary>
        /// <param name="name">The name for the file or directory</param>
        /// <returns></returns>
        private static string GetTempPath(string name)
        {
            return Path.Combine(GetTempPath(), name);
        }
                
        private static string GetSessionPath(Session session, string fileName)
        {
            return Path.Combine(GetSessionDirectory(session), fileName);
        }

        private static void CreateSessionPath(Session session)
        {
            Directory.CreateDirectory(GetSessionDirectory(session));
        }

        private static string GetSessionDirectory(Session session)
        {
            return Path.Combine(GetTempPath(), session.CreatedDateTime.ToString(SESSION_DIR_FORMAT));
        }

        /// <summary>
        /// Saves general info about a session to its own folder
        /// </summary>
        /// <param name="session"></param>
        internal async static Task SaveSession(Session session)
        {
            CreateSessionPath(session);
            string sessionPath = GetSessionPath(session, SESSION_FILE_NAME);
            using (var sw = new StreamWriter(sessionPath))
            {
                await sw.WriteAsync(JsonConvert.SerializeObject(session, Formatting.Indented));
            }
        }

        /// <summary>
        /// Saves general info about a session to the global session file
        /// </summary>
        /// <param name="session"></param>
        internal static void SaveGlobalSession(Session session)
        {
            string tempPath = FileSystemHelper.GetTempPath(SESSION_FILE_NAME);
            FileInfo fileInfo = new FileInfo(tempPath);
            Directory.CreateDirectory(fileInfo.DirectoryName);
            File.WriteAllText(tempPath, JsonConvert.SerializeObject(session, Formatting.Indented));
        }

        /// <summary>
        /// Saves an order to the given sessions, temp folder
        /// </summary>
        /// <param name="session"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        internal static async Task SaveSessionAndOrder(Session session, Order order)
        {
            await SaveSession(session);

            string orderPath = GetSessionPath(session, ORDER_FILE_START+order.OrderId+ ".json");
            using (var sw = new StreamWriter(orderPath))
            {
                await sw.WriteAsync(JsonConvert.SerializeObject(order, Formatting.Indented));
            }
        }
        
        /// <summary>
        /// This function reads the global session information
        /// </summary>
        internal static Session ReadGlobalSession()
        {
            try
            {
                return ReadSession(GetTempPath(SESSION_FILE_NAME));
            }
            catch
            {
            }
            //fall back to a new product list
            return new Session();

        }

        /// <summary>
        /// Reads basic info for a session from a given file.
        /// Note that this method does not handle errors
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static Session ReadSession(string fileName)
        {
            return JsonConvert.DeserializeObject<Session>(File.ReadAllText(fileName));
        }

        internal static IEnumerable<Session> ReadAvailableSessions()
        {
            var sessions = new List<Session>();
            var dirInfo = new DirectoryInfo(GetTempPath());
            foreach(var dir in dirInfo.GetDirectories())
            {
                var sessionFiles = dir.GetFiles(SESSION_FILE_NAME);
                if (sessionFiles.Length == 1)
                {
                    try
                    {
                        sessions.Add(ReadSession(sessionFiles[0].FullName));
                    }
                    catch { }
                }
            }
            return sessions;
        }

        /// <summary>
        /// Reads the full session information into the given stession
        /// </summary>
        /// <param name="session"></param>
        internal static void ReadFullSession(Session session)
        {
            DirectoryInfo info = new DirectoryInfo(GetSessionDirectory(session));
            List<Order> orders = new List<Order>();
            foreach(var f in info.GetFiles(ORDER_FILE_START +"*"))
                orders.Add(JsonConvert.DeserializeObject<Order>(File.ReadAllText(f.FullName)));
            session.OrderList.AddOrders(orders);
        }

        internal static void ExportMenu(Session session, string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            Directory.CreateDirectory(fileInfo.DirectoryName);
            File.WriteAllText(fileName, JsonConvert.SerializeObject(session.ProductList, Formatting.Indented));
        }

        internal static void Export(Session session, string fileName)
        {
            string path = GetSessionDirectory(session);
            ZipFile.CreateFromDirectory(path, fileName);
        }

        internal static Session Import(string fileName)
        {
            string tempPath = GetTempPath("import");
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);
            ZipFile.ExtractToDirectory(fileName, tempPath);
            string sessionPath = Path.Combine(tempPath, SESSION_FILE_NAME);
            Session session = ReadSession(sessionPath);
            ReadFullSession(session);

            Directory.Delete(GetTempPath("import"), true);
            return session;
            
        }
    }
}
