using System.IO;
using System.Reflection;

namespace VsNu.Resources
{
    internal class EmbeddedResourceManager
    {
        public string GetTemplate(string resourceName)
        {
            var assembly = Assembly.GetEntryAssembly();

            string result;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }
    }
}
