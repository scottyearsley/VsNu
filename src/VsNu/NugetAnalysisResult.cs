using System.Collections.Generic;
using System.Linq;

namespace VsNu
{
    public class NugetAnalysisResult
    {
        public NugetAnalysisResult(IList<Project> projects)
        {
            Projects = projects;
        }

        public IList<Project> Projects { get; }

        public IList<string> GetUniqueAssemblyRefNames()
        {
            return Projects.SelectMany(p => p.References.Select(r => r.ProjectAssemblyRef.Name)).Distinct().OrderBy(n => n).ToList();
        }

        public IList<Project> GetProjectsForPackage(string packageName)
        {
            return
                Projects.SelectMany(p => p.References.Where(r => r.ProjectAssemblyRef.Name == packageName))
                    .Select(r => r.Project)
                    .OrderBy(p => p.Name)
                    .ToList();
        }

        public NugetIssue[] GetIssues()
        {
            var allIssues = new List<NugetIssue>();

            foreach (var project in Projects)
            {
                var issues = project.GetNugetIssues();
                allIssues.AddRange(issues);
            }

            return allIssues.ToArray();
        }
    }
}
