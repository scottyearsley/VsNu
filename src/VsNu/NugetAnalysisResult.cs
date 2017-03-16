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

        public string[] GetUniqueAssemblyRefNames()
        {
            return Projects.SelectMany(p => p.NugetProjectRefs.Select(r => r.Assembly.Name)).Distinct().OrderBy(n => n).ToArray();
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
