using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;
using static Build.Buildary.File;
using static Build.Buildary.Path;

namespace Build.Buildary
{
    public class GitVersion
    {
        public static GitVersionResult GetGitVersion(string directory)
        {
            string output;

            // If there exists a version.json in the directory, then gitversion was run by some other means.
            output = FileExists(CombinePath(directory, "version.json"))
                ? ReadFile(CombinePath(directory, "version.json"))
                : Shell.ReadShell($"cd {directory} && dotnet gitversion");

            dynamic json = JsonConvert.DeserializeObject(output);
            var result = new GitVersionResult
            {
                Version = json.MajorMinorPatch,
                VersionMajor = json.Major,
                VersionMinor = json.Minor,
                VersionPatch = json.Patch,
                PreReleaseLabel = json.PreReleaseLabel,
                PreReleaseTag = json.PreReleaseTag,
                PreReleaseNumber = json.PreReleaseNumber
            };
            
            if(!string.IsNullOrEmpty(result.PreReleaseTag))
            {
                result.FullVersion = $"{result.Version}-{result.PreReleaseTag}";
            }
            else
            {
                result.FullVersion = result.Version;
            }
            
            return result;
        }

        public class GitVersionResult
        {
            public string Version { get; set; }
            
            public int VersionMajor { get; set; }
            
            public int VersionMinor { get; set; }
            
            public int VersionPatch { get; set; }

            public string PreReleaseLabel { get; set; }

            public string PreReleaseTag { get; set; }
            
            public string PreReleaseNumber { get; set; }

            public string FullVersion { get; set; }
        }
    }
}