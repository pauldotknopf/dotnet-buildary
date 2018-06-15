using System;

namespace Build.Buildary
{
    public static class Shell
    {
        public static void RunShell(string shell)
        {
            // TODO: Support Windows
            
            var escapedArgs = shell.Replace("\"", "\\\"");
        
            SimpleExec.Command.Run("/usr/bin/env", $"bash -c \"{escapedArgs}\"");
        }
        
        public static string ReadShell(string command)
        {
            // TODO: Support Windows
            
            var escapedArgs = command.Replace("\"", "\\\"");
        
            return SimpleExec.Command.Read("/usr/bin/env", $"bash -c \"{escapedArgs}\"");
        }
    }
}