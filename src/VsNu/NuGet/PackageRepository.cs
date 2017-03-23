using NuGet;

namespace VsNu.NuGet
{
    public class PackageRepository : IPackageRepository
    {
        static IPackageCache _cache = new FilePackageCache();

        public NuGetPackage GetPackage(string packageId, string version)
        {
            var cached = _cache.Get(packageId, version);
            if (cached != null)
            {
                return cached;
            }

            var repository = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            var packageDefinition = repository.FindPackage(packageId, new SemanticVersion(version));

            if (packageDefinition != null)
            {
                var package = new NuGetPackage(packageDefinition);
                _cache.Insert(package);
                return package;
            }

            return null;
        }
    }
}
