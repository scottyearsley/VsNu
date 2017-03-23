namespace VsNu
{
    public class NugetAnalyzer
    {
        private readonly IPackageFactory _packageFactory;

        public NugetAnalyzer(IPackageFactory packageFactory)
        {
            _packageFactory = packageFactory;
        }

        public NugetAnalysisResult Analyze(string rootPath)
        {
            var finder = new ProjectService(_packageFactory, rootPath);
            var projectFiles = finder.Find();

            return new NugetAnalysisResult(projectFiles);
        }
    }
}
