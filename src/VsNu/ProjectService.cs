using System.Collections.Generic;
using System.IO;

namespace VsNu
{
    class ProjectService
    {
        private readonly string _rootPath;

        public ProjectService(string rootPath)
        {
            _rootPath = rootPath;
        }

        public IList<Project> Find()
        {
            var projects = new List<Project>();

            var csharpProjectPaths = Directory.GetFiles(_rootPath, "*.csproj", SearchOption.AllDirectories);
            var vbProjectPaths = Directory.GetFiles(_rootPath, "*.vbproj", SearchOption.AllDirectories);

            var allPaths = new List<string>();
            allPaths.AddRange(csharpProjectPaths);
            allPaths.AddRange(vbProjectPaths);

            foreach (var projectPath in allPaths)
            {
                var project = Project.Create(projectPath);
                if (project.References.Count > 0)
                {
                    projects.Add(project);
                }
            }

            return projects;
        }
    }
}
