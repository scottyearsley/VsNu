using System.IO;
using VsNu.Reports;

namespace VsNu
{
    class Program
    {
        static void Main(string[] args)
        {
            var analyzer = new NugetAnalyzer();
            var result = analyzer.Analyze(args[0]);

            var issues = result.GetIssues();

            var report = new ReportService().CreateReport(result);

            //var packages = result.GetUniqueAssemblyRefNames();

            File.WriteAllText("C:\\temp\\report.html", report);
        }
    }
}
