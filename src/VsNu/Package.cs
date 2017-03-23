using System.Collections.Generic;
using System.Linq;
using VsNu.NuGet;

namespace VsNu
{
    public class Package
    {
        private static IDictionary<string, NuGetPackage> _packages = new Dictionary<string, NuGetPackage>();
        private readonly IPackageRepository _packageRepository;

        public Package(IPackageRepository packageRepository, string id, string version)
        {
            _packageRepository = packageRepository;
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

        public PackageAssemblyReference[] GetPackageAssemblyReference(string assembly)
        {
            var assemblyRefs = GetPackageInfo().AssemblyReferences;
            return assemblyRefs.Where(p => p.Name.Replace(".dll", "") == assembly).ToArray();
        }

        public NuGetPackage GetPackageInfo()
        {
            if (_packages.ContainsKey(Id))
            {
                return _packages[Id];
            }

            var package = _packageRepository.GetPackage(Id, Version);
            _packages.Add(Id, package);

            return package;
        }
    }
}
