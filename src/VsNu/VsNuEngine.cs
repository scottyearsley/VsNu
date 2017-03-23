using System.IO;
using VsNu.Reports;

namespace VsNu
{
    public class VsNuEngine
    {
        private readonly IReportService _reportService;
        private readonly IPackageFactory _packageFactory;
        private readonly IReferenceFactory _referenceFactory;


        public VsNuEngine(IReportService reportService, IPackageFactory packageFactory, IReferenceFactory referenceFactory)
        {
            _reportService = reportService;
            _packageFactory = packageFactory;
            _referenceFactory = referenceFactory;
        }

        public void Execute(string rootDir, string outputPath)
        { 
            var analyzer = new NugetAnalyzer(_packageFactory, _referenceFactory);
            var result = analyzer.Analyze(rootDir);
            var report = _reportService.Create(result);

            var directory = Path.GetDirectoryName(outputPath);
            if (Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(outputPath, report);
        }
    }
}
