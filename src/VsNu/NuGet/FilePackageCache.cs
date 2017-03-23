using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NuGet;

namespace VsNu.NuGet
{
    /// <summary>
    /// File system package cache.
    /// </summary>
    public class FilePackageCache : IPackageCache
    {
        private static readonly string CacheFile;
        private List<DataServicePackage> _packages;

        private static JsonSerializerSettings _serializationSettings =
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

        static FilePackageCache()
        {
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var vsNuDir = Path.Combine(appDataDir, "VsNu");

            if (!Directory.Exists(vsNuDir))
            {
                Directory.CreateDirectory(vsNuDir);
            }

            CacheFile = Path.Combine(vsNuDir, "packages_cache.json");
        }


        /// <summary>
        /// Gets a package by id and version.
        /// </summary>
        /// <param name="packageId">The package name</param>
        /// <param name="version">The package version</param>
        /// <returns>IPackage instance</returns>
        public IPackage Get(string packageId, string version)
        {
            return Packages.FirstOrDefault(p => p.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase) && p.Version.ToString() == version);
        }

        /// <summary>
        /// Adds a package to the local cache.
        /// </summary>
        /// <param name="package">The package</param>
        public void Insert(IPackage package)
        {
            if (Get(package.Id, package.Version.ToString()) == null)
            {
                Packages.Add(package as DataServicePackage);
                File.WriteAllText(CacheFile, JsonConvert.SerializeObject(Packages, _serializationSettings));
            }
        }

        private List<DataServicePackage> Packages
        {
            get
            {
                if (_packages == null)
                {
                    if (File.Exists(CacheFile))
                    {
                        var fileContents = File.ReadAllText(CacheFile);
                        _packages = JsonConvert.DeserializeObject<List<DataServicePackage>>(fileContents, _serializationSettings);
                    }
                    else
                    {
                        _packages = new List<DataServicePackage>();
                    }
                }
                return _packages;
            }
        }
    }
}
