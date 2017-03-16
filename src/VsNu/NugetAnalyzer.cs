namespace VsNu
{
    public class NugetAnalyzer
    {
        public NugetAnalysisResult Analyze(string rootPath)
        {
            var finder = new ProjectService(rootPath);
            var projectFiles = finder.Find();

            return new NugetAnalysisResult(projectFiles);
        }

    }
}
