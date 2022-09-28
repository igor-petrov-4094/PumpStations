using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Lib.Profiles
{
    public class Profile
    {
        public string appName { get; private set; } // имя приложения
        public string producerName { get; private set; } // производитель
        public string profileId { get; private set; }    // id профиля
        public string profileBaseDir { get; private set; } // базовый каталог профиля

        public Profile(string _producerName, string _appName, string? _profileId = null)
        {
            appName = _appName;
            producerName = _producerName;
            profileId = _profileId ?? getCurrentProfileId();
            profileBaseDir = getProfileBaseDir(producerName, appName, profileId);
        }

        /// <summary>
        /// Получает строковый ID профиля
        /// </summary>
        /// <returns></returns>
        public static string getCurrentProfileId()
        {
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                string paramName = "/PROFILE:";
                string found = args.Where(x => x.ToUpper().StartsWith(paramName)).FirstOrDefault();
                if (!string.IsNullOrEmpty(found))
                {
                    string clean = found.Substring(paramName.Length);
                    return clean;
                }
            }
            catch
            {
            }
            return "001";
        }

        /// <summary>
        /// Получает базовый каталог профиля
        /// </summary>
        /// <returns></returns>
        public static string getProfileBaseDir(string _producerName, string _appName, string _profileId)
        {
            string s = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\{_producerName}\{_appName}\Profiles\{_profileId}";
            s = s.Replace(@"\\", @"\");
            return s;
        }

        /// <summary>
        /// Получает каталог для временных файлов в профиле
        /// </summary>
        /// <returns></returns>
        public string getProfileTempDirectory()
        {
            return $"{profileBaseDir}\\Temp";
        }

        /// <summary>
        /// Создает временный каталог в профиле и получает имя временного файла
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string createProfileTempDirectoryAndGetTempFileName(string prefix = "")
        {
            DateTime now = DateTime.Now;
            Random random = new Random(now.Millisecond + now.Second + now.Minute);

            int a = (int)('A');
            string suffix = "";
            suffix += (char)(a + random.Next(26));
            suffix += (char)(a + random.Next(26));
            suffix += (char)(a + random.Next(26));
            suffix += (char)(a + random.Next(26));
            suffix += (char)(a + random.Next(26));
            suffix += (char)(a + random.Next(26));

            string shortFilename = $"{now.ToString("yyyyMMdd_HHmmss")}_{prefix}_{suffix}";

            string tempDir = getProfileTempDirectory();

            try
            {
                Directory.CreateDirectory(tempDir);
            }
            catch
            {
            }

            return $"{tempDir}\\{shortFilename}";
        }

        /// <summary>
        /// Получает полное имя файла в профиле по короткому имени
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        public string getFullnameByShortName(string shortName)
        {
            string s = @$"{profileBaseDir}\{shortName}";
            s = s.Replace(@"\\", @"\");
            return s;
        }

        /// <summary>
        /// Получает в профиле содержимое текстового файла по короткому имени
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="defaultContentIfFileNotFound"></param>
        /// <returns></returns>
        public string? getFileContentsByShortName(string shortName, string? defaultContentIfFileNotFound = null)
        {
            string filename = getFullnameByShortName(shortName);
            if (!File.Exists(filename))
                return defaultContentIfFileNotFound;
            else
                return System.IO.File.ReadAllText(filename);
        }

        /// <summary>
        /// Устанавливает в профиле содержимое текстового файла по короткому имени
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="contents"></param>
        public void setFileContentsByShortName(string shortName, string contents)
        {
            string filename = getFullnameByShortName(shortName);
            System.IO.File.WriteAllText(filename, contents);
        }

        /// <summary>
        /// Получает в профиле содержимое текстового файла по короткому имени в виде списка строк
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        public List<string> getFileContentsByShortName_StringList(string shortName)
        {
            string filename = getFullnameByShortName(shortName);
            return System.IO.File.ReadAllLines(filename).ToList();
        }
    }
}
