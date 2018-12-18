using System.Runtime.InteropServices;
using static SimpleExec.Command;

namespace Build.Buildary
{
    public static class Shell
    {
        public static void RunShell(string shell)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Run("cmd.exe", $"/S /C \"{shell}\"", Directory.CurrentDirectory(), true);
            }
            else
            {
                var escapedArgs = shell.Replace("\"", "\\\"");
                Run("/usr/bin/env", $"bash -c \"{escapedArgs}\"", Directory.CurrentDirectory(), true);
            }
        }
        
        public static string ReadShell(string shell)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Read("cmd.exe", $"/S /C \"{shell}\"", Directory.CurrentDirectory(), true);
            }

            var escapedArgs = shell.Replace("\"", "\\\"");
            return Read("/usr/bin/env", $"bash -c \"{escapedArgs}\"", Directory.CurrentDirectory(), true);
        }
    }
}