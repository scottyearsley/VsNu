using NuGet;

namespace VsNu.NuGet
{
    public interface IPackageRepository
    {
        NuGetPackage GetPackage(string packageId, string version);
    }
}