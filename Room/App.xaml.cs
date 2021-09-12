using System;

namespace Room
{
    public partial class App
    {
        public static string GetResourceFullFileName(string fileName)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", fileName);
        }
    }
}
