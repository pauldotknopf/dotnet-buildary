namespace Build.Buildary
{
    public static class Directory
    {
        public static void CleanDirectory(string directory)
        {
            var di = new System.IO.DirectoryInfo(directory);

            if(!di.Exists)
            {
                return;
            }

            foreach (var file in di.GetFiles())
            {
                file.Delete(); 
            }

            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true); 
            }
        }
    }
}