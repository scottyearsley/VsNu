using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NuGet;

namespace VsNu
{
    public class Package
    {
        private static IDictionary<string, IPackage> _packages = new Dictionary<string, IPackage>();

        public Package(string id, string version)
        {
            Id = id;
            Version = version;
        }

        public string Id { get; }

        public string Version { get; }

        public bool Contains(string assembly)
        {
            var contains = GetPackageInfo().AssemblyReferences.Any(p => p.Name.Replace(".dll", "") == assembly);
            return contains;
        }

        public IPackageAssemblyReference GetPackageAssemblyReference(string assembly)
        {
            return GetPackageInfo().AssemblyReferences.SingleOrDefault(p => p.Name.Replace(".dll", "") == assembly);
        }

        public IPackage GetPackageInfo()
        {
            if (_packages.ContainsKey(Id))
            {
                return _packages[Id];
            }

            var package = new PackageRepository().GetPackage(Id);
            _packages.Add(Id, package);

            return package;
        }
    }
}
