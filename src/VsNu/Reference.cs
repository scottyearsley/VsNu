using System.IO;
using System.Xml.Linq;
using VsNu.Helpers;

namespace VsNu
{
    public class Reference
    {
        private AssemblyRef _fileAssemblyRef;

        /// <summary>
        /// Contains the details about the actual assembly being referenced.
        /// </summary>
        public AssemblyRef ProjectAssemblyRef { get; private set; }

        /// <summary>
        /// The hint path used by the project file to locate the assembly.
        /// </summary>
        public string HintPath { get; private set; }

        /// <summary>
        /// The parent project
        /// </summary>
        public Project Project { get; private set; }

        /// <summary>
        /// Determines if the reference is a NuGet reference by determining if the assembly is in a packages folder.
        /// </summary>
        public bool IsNuGetPackageRef => HintPath.Contains("packages");

        /// <summary>
        /// Determines that the project reference and the assembly match.
        /// </summary>
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

            returnValue.ProjectAssemblyRef = AssemblyRef.CreateFromAssemblyFullname(include.Value);
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

                    _fileAssemblyRef = new AssemblyRef(name, version, actualPath);
                }

                return _fileAssemblyRef;
            }
        }
    }
}
