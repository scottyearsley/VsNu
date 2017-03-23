using System.IO;
using VsNu.Reports;

namespace VsNu
{
    public class VsNuEngine
    {
        private readonly IReportService _reportService;
        private readonly IPackageFactory _packageFactory;

        public VsNuEngine(IReportService reportService, IPackageFactory packageFactory)
        {
            _reportService = reportService;
            _packageFactory = packageFactory;
        }

        public void Execute(string rootDir, string outputPath)
        { 
            var analyzer = new NugetAnalyzer(_packageFactory);
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
