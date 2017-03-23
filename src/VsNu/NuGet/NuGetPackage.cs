using NuGet;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

namespace VsNu.NuGet
{
    public class NuGetPackage
    {
        public NuGetPackage()
        {
            // For JSON Serialization
        }

        public NuGetPackage(IPackage package)
        {
            Id = package.Id;
            Version = package.Version.ToString();
            AssemblyReferences = package.AssemblyReferences.Select(r => new PackageAssemblyReference(r)).ToList();
        }

        public string Id { get; set; }

        public string Version { get; set; }

        public List<PackageAssemblyReference> AssemblyReferences { get; set; }
    }

    public class PackageAssemblyReference
    {
        public PackageAssemblyReference()
        {
            // For JSON serialization
        }

        public PackageAssemblyReference(IPackageAssemblyReference reference)
        {
            Name = reference.Name;
            Path = reference.Path;
            TargetFramework = reference.TargetFramework != null ? new Framework(reference.TargetFramework) : null;
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public Framework TargetFramework { get; set; }
    }

    public class Framework
    {
        public Framework()
        {
            // For JSON serialization
        }

        public Framework(FrameworkName frameworkName)
        {
            FullName = frameworkName.FullName;
            Profile = frameworkName.Profile;
            Version = frameworkName.Version.ToString();
        }

        public string FullName { get; set; }

        public string Profile { get; set; }

        public string Version { get; set; }
    }
}
