using NuGet;

namespace VsNu.NuGet
{
    public class PackageRepository : IPackageRepository
    {
        static IPackageCache _cache = new FilePackageCache();

        public IPackage GetPackage(string packageId, string version)
        {
            var cached = _cache.Get(packageId, version);
            if (cached != null)
            {
                return cached;
            }

            var repository = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            var package = repository.FindPackage(packageId, new SemanticVersion(version));

            if (package != null)
            {
                _cache.Insert(package);
            }

            return package;
        }
    }
}
