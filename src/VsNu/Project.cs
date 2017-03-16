using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace VsNu
{
    public class Project
    {
        public string Name { get; private set; }

        public string ProjectPath { get; private set; }

        public List<Reference> References { get; } = new List<Reference>();

        public List<Package> Packages { get; } = new List<Package>();

        //public bool ReferencesMatch
        //{
        //    get { return true; }
        //}

        public static Project Create(string path)
        {
            var project = new Project
            {
                ProjectPath = path
            };

            LoadProjectFile(path, project);

            LoadPackagesFile(path, project);

            return project;
        }

        public List<NugetIssue> GetNugetIssues()
        {
            var issues = new List<NugetIssue>();

            foreach (var nugetProjectRef in References)
            {
                // Find matching package   
                var package = Packages.SingleOrDefault(p => p.Id == nugetProjectRef.ProjectAssemblyRef.Name);

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

        private static void LoadPackagesFile(string path, Project project)
        {
            var directoryPath = Path.GetDirectoryName(path);
            var packagesPath = Path.Combine(directoryPath, "packages.config");

            if (File.Exists(packagesPath))
            {
                var doc = XDocument.Load(packagesPath);
                AddPackageItems(doc, project);
            }
        }

        private static void AddPackageItems(XDocument doc, Project project)
        {
            var ns = doc.Root.Name.Namespace;
            var packageElements = doc.Root.Elements(ns + "package");

            var packageList = new List<Package>();

            foreach (var packageElement in packageElements)
            {
                packageList.Add(new Package(packageElement.Attribute("id")?.Value,
                    packageElement.Attribute("version")?.Value));
            }

            project.Packages.AddRange(packageList);
        }

        private static void LoadProjectFile(string path, Project project)
        {
            try
            {
                var doc = XDocument.Load(path);
                var ns = doc.Root.Name.Namespace;

                project.Name = doc.Root?.Element(ns + "PropertyGroup")?.Element(ns + "AssemblyName")?.Value;

                AddReferences(project, doc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void AddReferences(Project project, XDocument doc)
        {
            var refs = new List<Reference>();
            var ns = doc.Root.Name.Namespace;

            var refIndicator = doc.Descendants(ns + "HintPath");

            foreach (var element in refIndicator)
            {
                var reference = element.Parent;
                var nugetRef = Reference.Create(ns, reference, project);
                refs.Add(nugetRef);
            }

            project.References.AddRange(refs);
        }
    }
}
