namespace Build.Buildary
{
    public static class Path
    {
        public static string ExpandPath(string path)
        {
            return System.IO.Path.GetFullPath(path);
        }

        public static string CombinePath(string left, string right)
        {
            return System.IO.Path.Combine(left, right);
        }
    }
}