using System;
using System.Runtime.InteropServices;
using static SimpleExec.Command;

namespace Build.Buildary
{
    public static class Shell
    {
        public static bool NoEcho { get; set; } = true;
        
        public static void RunShell(string shell)
        {
            if (!NoEcho)
            {
                Console.WriteLine($"{Log.Message(Log.MessageType.Info, "Running:")} {shell}");
            }
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Run("cmd.exe", $"/S /C \"{shell}\"", Directory.CurrentDirectory(), true);
            }
            else
            {
                Console.WriteLine("runnin!");
                var escapedArgs = shell.Replace("\"", "\\\"");
                Run("/usr/bin/env", $"bash -c \"{escapedArgs}\"", Directory.CurrentDirectory(), false);
                Console.WriteLine("done");
            }
        }
        
        public static void RunCommand(string command, string args)
        {
            if (!NoEcho)
            {
                Console.WriteLine($"{Log.Message(Log.MessageType.Info, "Running:")} {command}{(string.IsNullOrEmpty(args) ? "" : $" {args}")}");
            }
            
            Run(command, args, Directory.CurrentDirectory(), true);
        }
        
        public static string ReadShell(string shell)
        {
            if (!NoEcho)
            {
                Console.WriteLine($"{Log.Message(Log.MessageType.Info, "Running:")} {shell}");
            }
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return ReadAsync("cmd.exe", $"/S /C \"{shell}\"", Directory.CurrentDirectory()).GetAwaiter().GetResult()
                    .StandardOutput;
            }

            var escapedArgs = shell.Replace("\"", "\\\"");
            return ReadAsync("/usr/bin/env", $"bash -c \"{escapedArgs}\"", Directory.CurrentDirectory()).GetAwaiter().GetResult().StandardOutput;
        }
        
        public static string ReadCommand(string command, string args)
        {
            if (!NoEcho)
            {
                Console.WriteLine($"{Log.Message(Log.MessageType.Info, "Running:")} {command}{(string.IsNullOrEmpty(args) ? "" : $" {args}")}");
            }
            
            return ReadAsync(command, args, Directory.CurrentDirectory()).GetAwaiter().GetResult().StandardOutput;
        }
    }
}