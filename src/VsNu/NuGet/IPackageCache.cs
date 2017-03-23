using NuGet;

namespace VsNu.NuGet
{
    /// <summary>
    /// Package details cache.
    /// </summary>
    public interface IPackageCache
    {
        /// <summary>
        /// Gets a package by id and version.
        /// </summary>
        /// <param name="packageId">The package name</param>
        /// <param name="version">The package version</param>
        /// <returns>IPackage instance</returns>
        NuGetPackage Get(string packageId, string version);

        /// <summary>
        /// Adds a package to the local cache.
        /// </summary>
        /// <param name="package">The package</param>
        void Insert(NuGetPackage package);
    }
}
