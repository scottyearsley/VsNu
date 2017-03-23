using System.Collections.Generic;
using System.Linq;

namespace VsNu
{
    /// <summary>
    /// Contains the results of a analysis run.
    /// </summary>
    public class NugetAnalysisResult
    {
        public NugetAnalysisResult(IList<Project> projects)
        {
            Projects = projects;
        }

        public IList<Project> Projects { get; }

        /// <summary>
        /// Returns the unique assembly reference names.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetUniqueAssemblyRefNames()
        {
            return Projects
                .SelectMany(p => p.References.Select(r => r.ProjectAssemblyRef.Name))
                .Distinct()
                .OrderBy(n => n)
                .ToList();
        }
        
        /// <summary>
        /// Returns the projects that have a specific package reference.
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public IList<Project> GetProjectsForPackage(string packageName)
        {
            return Projects
                .SelectMany(p => p.References.Where(r => r.ProjectAssemblyRef.Name == packageName))
                .Select(r => r.Project)
                .OrderBy(p => p.Name)
                .ToList();
        }

        /// <summary>
        /// Returns all the issues identified across all projects.
        /// </summary>
        /// <returns></returns>
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
