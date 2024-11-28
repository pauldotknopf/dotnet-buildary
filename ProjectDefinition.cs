using static Build.Buildary.Log;
using static Build.Buildary.GitVersion;
using static Build.Buildary.Path;
using static Bullseye.Targets;
using static Build.Buildary.Directory;
using static Build.Buildary.Shell;
using static Build.Buildary.File;

namespace Build.Buildary
{
    public class ProjectDefinition
    {
        public string SolutionPath { get; set; }
        
        public static void Register(Runner.RunnerOptions options, ProjectDefinition definition)
        {
            Info($"Configuration: {options.Config}");
            
            var gitVersion = GetGitVersion(ExpandPath("./"));
            Info($"Version: {gitVersion.FullVersion}");

            var commandBuildArgs = $"--configuration {options.Config}";
            var commandBuildArgsWithVersion = commandBuildArgs;
            
            Target("clean", () =>
            {
                CleanDirectory(ExpandPath("./output"));
            });
            
            Target("update-version", () =>
            {
                if (FileExists("./build/version.props"))
                {
                    DeleteFile("./build/version.props");
                }
                
                WriteFile("./build/version.props",
                    $@"<Project>
    <PropertyGroup>
        <VersionPrefix>{gitVersion.Version}</VersionPrefix>
    </PropertyGroup>
</Project>");
            });

            Target("build", () =>
            {
                RunShell($"dotnet build {commandBuildArgs} {ExpandPath(definition.SolutionPath)}");
            });

            Target("test", () =>
            {
                RunShell($"dotnet test {commandBuildArgs} {ExpandPath(definition.SolutionPath)}");
            });

            Target("deploy", () =>
            {
                RunShell($"dotnet pack --output {ExpandPath("./output")} {commandBuildArgsWithVersion} {ExpandPath(definition.SolutionPath)}");
            });
            
            Target("ci", DependsOn("clean", "update-version", "test", "deploy"));
            
            Target("default", DependsOn("build"));
        }
    }
}