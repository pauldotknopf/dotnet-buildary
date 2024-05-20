using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace Build.Buildary
{
    public class Runner
    {
        public static void Execute(RunnerOptions options)
        {
            var newArgs = new List<string>();
            newArgs.Add(options.Target);
            try
            {
                Bullseye.Targets.RunTargetsAndExitAsync(newArgs).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
                throw;
            }
        }

        public static T ParseOptions<T>(string[] args) where T: RunnerOptions
        {
            T result = null;
            var parsed = Parser.Default.ParseArguments<T>(args)
                .WithParsed(o => { result = o; })
                .WithNotParsed(errors => Environment.Exit(1));
            return result;
        }
        
        public class RunnerOptions
        {
            [Value(0, Default = "default", HelpText = "The target to run.")]
            public string Target { get; set; }
            
            [Option('c', "config", HelpText = "The configuration.", Default = "Release")]
            public string Config { get; set; }
        }
    }
}