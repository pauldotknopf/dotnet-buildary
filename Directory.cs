using System.Collections.Generic;
using System.Linq;

namespace Build.Buildary
{
    public static class Directory
    {
        public static void CleanDirectory(string directory)
        {
            var di = new System.IO.DirectoryInfo(directory);

            if(!di.Exists)
            {
                return;
            }

            foreach (var file in di.GetFiles())
            {
                file.Delete(); 
            }

            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true); 
            }
        }

        public static void DeleteDirectory(string directory, bool recursive = true)
        {
            System.IO.Directory.Delete(directory, recursive);
        }

        public static bool DirectoryExists(string directory)
        {
            return System.IO.Directory.Exists(directory);
        }

        public static string CurrentDirectory()
        {
            return System.IO.Directory.GetCurrentDirectory();
        }

        public static List<string> GetDirecories(string directory, string pattern = null, bool recursive = false)
        {
            var searchOptions = recursive ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly;
            return !string.IsNullOrEmpty(pattern)
                ? System.IO.Directory.GetDirectories(directory, pattern, searchOptions).ToList()
                : System.IO.Directory.GetDirectories(directory, "*", searchOptions).ToList();
        }
    }
}