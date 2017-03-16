using NuGet;

namespace VsNu
{
    class PackageRepository
    {
        public IPackage GetPackage(string packageName)
        {
            // TODO: create cache on file system


            var repository = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            return repository.FindPackage(packageName);
        }
    }
}
