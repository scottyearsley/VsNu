using System.IO;
using System.Xml.Linq;

namespace VsNu
{
    public class Reference
    {
        private AssemblyRef _fileAssemblyRef;

        public AssemblyRef ProjectAssemblyRef { get; private set; }

        public string HintPath { get; private set; }

        public Project Project { get; private set; }

        public bool IsNugetPackageRef => HintPath.Contains("Packages");

        public bool VersionMismatch => ProjectAssemblyRef.Version != AssemblyInfo.Version;

        public static Reference Create(XNamespace ns, XElement element, Project project)
        {
            var returnValue = new Reference();
            var include = element.Attribute("Include");
            var hintPath = element.Element(ns + "HintPath")?.Value;

            if (include == null)
            {
                return null;
            }

            returnValue.ProjectAssemblyRef = AssemblyRef.Create(include.Value);
            returnValue.HintPath = hintPath;
            returnValue.Project = project;
            return returnValue;
        }

        public AssemblyRef AssemblyInfo
        {
            get
            {
                if (_fileAssemblyRef == null)
                {
                    var version = "Unknown";
                    var name = "Unknown";

                    var actualPath = PathHelper.GetAbsolutePath(Project.ProjectPath, HintPath);

                    if (File.Exists(actualPath))
                    {
                        var assembly = System.Reflection.Assembly.LoadFile(actualPath);
                        version = assembly.GetName().Version.ToString();
                        name = assembly.GetName().Name;
                    }

                    _fileAssemblyRef = AssemblyRef.Create(name, version, actualPath);
                }

                return _fileAssemblyRef;
            }
        }
    }
}
