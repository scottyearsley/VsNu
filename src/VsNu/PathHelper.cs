using System.IO;

namespace VsNu
{
    public static class PathHelper
    {
        public static string GetAbsolutePath(string directoryPath, string relativePath)
        {
            // Remove file if included
            directoryPath = Path.GetDirectoryName(directoryPath);

            if (directoryPath != null)
            {
                var path = Path.Combine(directoryPath, relativePath);
                return Path.GetFullPath(path);
            }

            return null;
        }
    }
}
