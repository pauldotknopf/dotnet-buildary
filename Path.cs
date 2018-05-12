namespace Build.Buildary
{
    public static class Path
    {
        public static string ExpandPath(string path)
        {
            return System.IO.Path.GetFullPath(path);
        }
    }
}