using System.Collections.Generic;
using System.Linq;

namespace Build.Buildary
{
    public static class File
    {
        public static bool FileExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public static void DeleteFile(string file)
        {
            System.IO.File.Delete(file);
        }

        public static void WriteFile(string file, string content)
        {
            System.IO.File.WriteAllText(file, content);
        }

        public static void ReplaceInFile(string file, string oldValue, string newValue)
        {
            string text = System.IO.File.ReadAllText(file);
            text = text.Replace(oldValue, newValue);
            System.IO.File.WriteAllText(file, text);
        }

        public static List<string> GetFiles(string directory, string pattern = null)
        {
            return !string.IsNullOrEmpty(pattern)
                ? System.IO.Directory.GetFiles(directory, pattern).ToList()
                : System.IO.Directory.GetFiles(directory).ToList();
        }

        public static void CopyFile(string source, string destination)
        {
            System.IO.File.Copy(source, destination);
        }
    }
}