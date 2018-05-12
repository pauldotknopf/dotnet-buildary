using Newtonsoft.Json;

namespace Build.Buildary
{
    public class GitVersion
    {
        public static GitVersionResult GetGitVersion(string directory)
        {
            // TODO: Switch to using GitVersion directly (.NET Standard) when it is supported: https://github.com/GitTools/GitVersion/pull/1269
            var output = Shell.CaptureCapture($"docker run --rm -v {Path.ExpandPath(directory)}:/repo gittools/gitversion /overrideconfig tag-prefix=v");
            
            dynamic json = JsonConvert.DeserializeObject(output);
            var result = new GitVersionResult
            {
                Version = json.MajorMinorPatch,
                PreReleaseLabel = json.PreReleaseLabel,
                PreReleaseTag = json.PreReleaseTag
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

            public string PreReleaseLabel { get; set; }

            public string PreReleaseTag { get; set; }

            public string FullVersion { get; set; }
        }
    }
}