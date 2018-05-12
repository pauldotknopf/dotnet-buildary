using System;

namespace Build.Buildary
{
    public static class Shell
    {
        public static void RunShell(string shell)
        {
            // TODO: Support Windows
            
            var escapedArgs = shell.Replace("\"", "\\\"");
        
            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "/usr/bin/env",
                Arguments = $"bash -c \"{escapedArgs}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = System.Diagnostics.Process.Start(processStartInfo))
            {
                // ReSharper disable PossibleNullReferenceException
                process.WaitForExit();
                // ReSharper restore PossibleNullReferenceException

                if (process.ExitCode != 0)
                    throw new Exception(string.Format("Exit code {0} from {1}", process.ExitCode, shell));
            }
        }
        
        public static string CaptureCapture(string command)
        {
            // TODO: Support Windows
            
            var escapedArgs = command.Replace("\"", "\\\"");
        
            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "/usr/bin/env",
                Arguments = $"bash -c \"{escapedArgs}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };

            using (var process = System.Diagnostics.Process.Start(processStartInfo))
            {
                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    Log.Failure($"Output: {output}");
                    throw new Exception(string.Format("Exit code {0} from {1}", process.ExitCode, command));
                }
            
                return output;
            }
        }
    }
}