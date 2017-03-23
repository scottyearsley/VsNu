using System;
using VsNu.Reports;

namespace VsNu
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("args", "Please provide a root directory and output file path");
            }

            var engine = new VsNuEngine(
                new RazorEngineReportService(), 
                new PackageFactory(
                    new NuGet.PackageRepository()
                )
            );

            engine.Execute(args[0], args[1]);
        }
    }
}
