using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Build.Buildary
{
    public static class Runtime
    {
        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
    }
}
