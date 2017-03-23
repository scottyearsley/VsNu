using System.Collections.Generic;
using System.IO;

using System;

namespace VsNu.Services
{
    class ProjectService
    {
        private readonly IPackageFactory _packageFactory;
        private readonly IReferenceFactory _referenceFactory;

        public ProjectService(IPackageFactory packageFactory, IReferenceFactory referenceFactory)
        {
            _packageFactory = packageFactory;
            _referenceFactory = referenceFactory;
        }

        /// <summary>
        /// Searches for all instances of VS projects within a directory tree.
        /// </summary>
        /// <returns></returns>
        public IList<Project> Find(string rootPath)
        {
            if(string.IsNullOrWhiteSpace(rootPath))
            {
                throw new ArgumentNullException(nameof(rootPath));
            }

            if (!Directory.Exists(rootPath))
            {
                throw new ArgumentException("Directory does not exist", nameof(rootPath));
            }

            var projects = new List<Project>();
            var csharpProjectPaths = Directory.GetFiles(rootPath, "*.csproj", SearchOption.AllDirectories);
            var vbProjectPaths = Directory.GetFiles(rootPath, "*.vbproj", SearchOption.AllDirectories);

            var allPaths = new List<string>();
            allPaths.AddRange(csharpProjectPaths);
            allPaths.AddRange(vbProjectPaths);

            foreach (var projectPath in allPaths)
            {
                var project = new Project(projectPath, _packageFactory, _referenceFactory);
                if (project.References.Count > 0)
                {
                    projects.Add(project);
                }
            }

            return projects;
        }
    }
}
