using System.Collections.Generic;
using System.Linq;
using NuGet;

namespace VsNu
{
    public class Package
    {
        private static IDictionary<string, IPackage> _packages = new Dictionary<string, IPackage>();
        private readonly VsNu.NuGet.IPackageRepository _packageRepository;

        public Package(VsNu.NuGet.IPackageRepository packageRepository, string id, string version)
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

        public IPackageAssemblyReference[] GetPackageAssemblyReference(string assembly)
        {
            var assemblyRefs = GetPackageInfo().AssemblyReferences;
            return assemblyRefs.Where(p => p.Name.Replace(".dll", "") == assembly).ToArray();
        }

        public IPackage GetPackageInfo()
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
