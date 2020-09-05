using System;
using System.IO;

namespace TurtBOT
{
    public static class DataDirectory
    {
        public static string DirPath()
        { 
            var dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create);

            var dir = Path.Combine(dataFolder, typeof(DataDirectory).Assembly.GetName().Name ?? "TurtBOT");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }

        public static bool Exists(string filename) => File.Exists(Combine(filename));

        public static string ReadString(string filename) => File.ReadAllText(Combine(filename));

        public static void WriteString(string filename, string contents) => File.WriteAllText(Combine(filename), contents);

        public static string Combine(string filename) => Path.Combine(DirPath(), filename);
    }
}
