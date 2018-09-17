using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Build.Buildary
{
    public class Runner
    {
        public static T ParseOptions<T>(IEnumerable<string> args) where T: RunnerOptions
        {
            T options = null;

            try
            {
                options = PowerArgs.Args.Parse<T>(args.ToArray());
            }
            catch (PowerArgs.ArgException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(PowerArgs.ArgUsage.GenerateUsageFromTemplate<T>());
                Environment.Exit(1);
            }

            if(options == null)
            {
                // It was a help command (-help);
                Environment.Exit(0);
            }

            return options;
        }

        public static RunnerOptions ParseOptions(IEnumerable<string> args)
        {
            return ParseOptions<RunnerOptions>(args);
        }

        public static Task Run(RunnerOptions options)
        {
            return Bullseye.Targets.RunTargetsAsync(new[] {options.Target});
        }
        
        public class RunnerOptions
        {
            [PowerArgs.HelpHook, PowerArgs.ArgShortcut("-?")]
            public bool Help { get; set; }

            [PowerArgs.ArgDefaultValue("default"), PowerArgs.ArgPosition(0)]
            public string Target { get; set; }
        }
    }
}