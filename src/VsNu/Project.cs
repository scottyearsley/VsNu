using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace VsNu
{
    /// <summary>
    /// 
    /// </summary>
    public class Project
    {
        public string Name { get; set; }

        public List<ProjectRef> NugetProjectRefs { get; set; } = new List<ProjectRef>();

        public List<Package> Packages { get; set; } = new List<Package>();

        // public bool ReferencesMatch
        // {
        //     get { return true; }
        // }

        public static Project Create(string path)
        {
            var project = new Project();

            LoadProjectFile(path, project);

            LoadPackagesFile(path, project);

            return project;
        }

        public List<NugetIssue> GetNugetIssues()
        {
            var issues = new List<NugetIssue>();

            foreach (var nugetProjectRef in NugetProjectRefs)
            {
                // Find matching package   
                var package = Packages.SingleOrDefault(p => p.Id == nugetProjectRef.Assembly.Name);

                if (package == null)
                {
                    issues.Add(new NugetIssue(NugetIssueType.MissingPackageEntry, nugetProjectRef.Assembly, null));
                }
                else
                {
                    if (package.Version != nugetProjectRef.Assembly.Version)
                    {
                        if (nugetProjectRef.Assembly.Version != null)
                        {
                            if (!nugetProjectRef.Assembly.Version.StartsWith(package.Version))
                            {
                                issues.Add(new NugetIssue(NugetIssueType.VersionMismatch, nugetProjectRef.Assembly,
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

            project.Packages = packageList;
        }

        private static void LoadProjectFile(string path, Project project)
        {
            try
            {
                var doc = XDocument.Load(path);
                var ns = doc.Root.Name.Namespace;

                project.Name = doc.Root?.Element(ns + "PropertyGroup")?.Element(ns + "AssemblyName")?.Value;

                AddNugetProjectRefs(project, doc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddNugetProjectRefs(Project project, XDocument doc)
        {
            var refs = new List<ProjectRef>();
            var ns = doc.Root.Name.Namespace;

            var refIndicator = doc.Descendants(ns + "Private");

            foreach (var element in refIndicator)
            {
                if (element.Value == "True")
                {
                    var reference = element.Parent;
                    var nugetRef = ProjectRef.Create(ns, reference, project);
                    refs.Add(nugetRef);
                }
            }

            project.NugetProjectRefs = refs;
        }
    }
}
