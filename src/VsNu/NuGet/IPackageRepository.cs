using NuGet;

namespace VsNu.NuGet
{
    public interface IPackageRepository
    {
        IPackage GetPackage(string packageId, string version);
    }
}