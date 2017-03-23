using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace VsNu
{
    public class Project
    {
        private readonly IPackageFactory _packageFactory;
        private readonly IReferenceFactory _referenceFactory;

        public Project(string path, IPackageFactory packageFactory, IReferenceFactory referenceFactory)
        {
            _packageFactory = packageFactory;
            _referenceFactory = referenceFactory;

            ProjectPath = path;

            LoadProjectFile(path);

            LoadPackagesFile(path);
        }

        public string Name { get; private set; }

        public string ProjectPath { get; private set; }

        public List<Reference> References { get; } = new List<Reference>();

        public List<Package> Packages { get; } = new List<Package>();

        public Reference GetReference(string assembly)
        {
            // TODO: If there is more than one then what to do?
            return References.FirstOrDefault(r => r.ProjectAssemblyRef.Name == assembly);
        }

        public List<NugetIssue> GetNugetIssues()
        {
            var issues = new List<NugetIssue>();

            foreach (var nugetProjectRef in References)
            {
                // Find matching package   
                var package = Packages.FirstOrDefault(p => p.Id == nugetProjectRef.ProjectAssemblyRef.Name);

                if (package == null)
                {
                    issues.Add(new NugetIssue(NugetIssueType.MissingPackageEntry, nugetProjectRef.ProjectAssemblyRef, null));
                }
                else
                {
                    if (package.Version != nugetProjectRef.ProjectAssemblyRef.Version)
                    {
                        if (nugetProjectRef.ProjectAssemblyRef.Version != null)
                        {
                            if (!nugetProjectRef.ProjectAssemblyRef.Version.StartsWith(package.Version))
                            {
                                issues.Add(new NugetIssue(NugetIssueType.VersionMismatch, nugetProjectRef.ProjectAssemblyRef,
                                    package));
                            }
                        }
                    }
                }
            }

            return issues;
        }

        private void LoadPackagesFile(string path)
        {
            var directoryPath = Path.GetDirectoryName(path);
            var packagesPath = Path.Combine(directoryPath, "packages.config");

            if (File.Exists(packagesPath))
            {
                var doc = XDocument.Load(packagesPath);
                AddPackageItems(doc);
            }
        }

        private void AddPackageItems(XDocument doc)
        {
            var ns = doc.Root.Name.Namespace;
            var packageElements = doc.Root.Elements(ns + "package");

            var packageList = new List<Package>();

            foreach (var packageElement in packageElements)
            {
                packageList.Add(_packageFactory.Create(packageElement));
            }

            Packages.AddRange(packageList);
        }

        private void LoadProjectFile(string path)
        {
            try
            {
                var doc = XDocument.Load(path);
                var ns = doc.Root.Name.Namespace;

                Name = doc.Root?.Element(ns + "PropertyGroup")?.Element(ns + "AssemblyName")?.Value;

                References.AddRange(_referenceFactory.CreateReferences(doc, this));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
