using VsNu.Services;

namespace VsNu
{
    public class NugetAnalyzer
    {
        private readonly IPackageFactory _packageFactory;
        private readonly IReferenceFactory _referenceFactory;

        public NugetAnalyzer(IPackageFactory packageFactory, IReferenceFactory referenceFactory)
        {
            _packageFactory = packageFactory;
            _referenceFactory = referenceFactory;
        }

        public NugetAnalysisResult Analyze(string rootPath)
        {
            var finder = new ProjectService(_packageFactory, _referenceFactory);
            var projectFiles = finder.Find(rootPath);

            return new NugetAnalysisResult(projectFiles);
        }
    }
}
