using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Build.Buildary
{
    public static class Shell
    {
        static Shell()
        {
            // Commands may contain sensitive information
            Config.PrintCommand = false;
        }
        
        public static void RunShell(string shell)
        {
            // TODO: Support Windows
            
            var escapedArgs = shell.Replace("\"", "\\\"");
        
            Command.Run("/usr/bin/env", $"bash -c \"{escapedArgs}\"");
        }
        
        public static string ReadShell(string command)
        {
            // TODO: Support Windows
            
            var escapedArgs = command.Replace("\"", "\\\"");
        
            return Command.Read("/usr/bin/env", $"bash -c \"{escapedArgs}\"");
        }
    }

    // Temp copy of SimpleExec, until new version is published: https://github.com/adamralph/simple-exec/pull/29
    
    internal static class Config
    {
        public static bool PrintCommand = true;
    }

    internal static class Command
    {
        public static void Run(string name, string args) => Run(name, args, "");

        public static Task RunAsync(string name, string args) => RunAsync(name, args, "");

        public static void Run(string name, string args, string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo = CreateProcessInfo(name, args, workingDirectory, false);
                process.Run();

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }
            }
        }

        public static async Task RunAsync(string name, string args, string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo = CreateProcessInfo(name, args, workingDirectory, false);
                await process.RunAsync();

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }
            }
        }

        public static string Read(string name, string args) => Read(name, args, "");

        public static Task<string> ReadAsync(string name, string args) => ReadAsync(name, args, "");

        public static string Read(string name, string args, string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo = CreateProcessInfo(name, args, workingDirectory, true);
                process.Run();

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }

                return process.StandardOutput.ReadToEnd();
            }
        }

        public static async Task<string> ReadAsync(string name, string args, string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo = CreateProcessInfo(name, args, workingDirectory, true);
                await process.RunAsync();

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }

                return await process.StandardOutput.ReadToEndAsync();
            }
        }

        private static ProcessStartInfo CreateProcessInfo(string name, string args, string workingDirectory, bool captureOutput) =>
            new ProcessStartInfo
            {
                FileName = name,
                Arguments = args,
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = captureOutput
            };

        private static void Run(this Process process)
        {
            process.EchoAndStart();
            process.WaitForExit();
        }

        private static async Task RunAsync(this Process process)
        {
            process.EnableRaisingEvents = true;
            var tcs = new TaskCompletionSource<object>();
            process.Exited += (s, e) => tcs.SetResult(null);
            process.EchoAndStart();
            await tcs.Task.ConfigureAwait(false);
        }

        private static void EchoAndStart(this Process process)
        {
            if (Config.PrintCommand)
            {
                var message = $"{(process.StartInfo.WorkingDirectory == "" ? "" : $"Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{process.StartInfo.FileName} {process.StartInfo.Arguments}";
                Console.Out.WriteLine(message);
            }
            process.Start();
        }

        private static void Throw(this Process process) =>
            process.Throw($"Error with exit code {process.ExitCode}");

        private static void Throw(this Process process, string stdErr) =>
            throw new Exception($"The process exited with code {process.ExitCode}: {stdErr.Trim()}");
    }
}