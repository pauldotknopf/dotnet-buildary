using System;

namespace Build.Buildary
{
    public class Travis
    {
        public enum EventTypeEnum
        {
            Push,
            PullRequest,
            Api,
            Cron,
            Unknown
        }

        public static bool IsTravis => Environment.GetEnvironmentVariable("TRAVIS") == "true";

        public static EventTypeEnum EventType
        {
            get
            {
                switch(Environment.GetEnvironmentVariable("TRAVIS_EVENT_TYPE"))
                {
                    case "push":
                        return EventTypeEnum.Push;
                    case "pull_request":
                        return EventTypeEnum.PullRequest;
                    case "api":
                        return EventTypeEnum.Api;
                    case "cron":
                        return EventTypeEnum.Cron;
                    default:
                        return EventTypeEnum.Unknown;
                }
            }
        }

        public static bool IsTagBuild => !string.IsNullOrEmpty(Tag);

        public static string Tag => Environment.GetEnvironmentVariable("TRAVIS_TAG");

        public static string Branch => Environment.GetEnvironmentVariable("TRAVIS_BRANCH");
    }
}