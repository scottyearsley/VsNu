using RazorEngine;
using RazorEngine.Templating;
using VsNu.Resources;

namespace VsNu.Reports
{
    internal class RazorEngineReportService: IReportService
    {
        public string Create(NugetAnalysisResult analysisDataResult)
        {
            try
            {
                var template = new EmbeddedResourceManager().GetTemplate("VsNu.Resources.Report.cshtml");

                return Engine.Razor.RunCompile(template, "report", typeof(NugetAnalysisResult), analysisDataResult);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
