using System;
using System.Linq;
using System.Collections.Generic;
using VsNu.Reports;
using VsNu.Helpers;

namespace VsNu
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("args", "Please provide a root directory and output file path");
            }

            var exclusions = new List<string>();

            if (args.Length == 3 && !string.IsNullOrWhiteSpace(args[2]))
            {
                exclusions.AddRange(args[2].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(e => RegexHelper.WildCardToRegEx(e.Trim())));
            }

            var engine = new VsNuEngine(
                new RazorEngineReportService(),
                new PackageFactory(
                    new NuGet.PackageRepository()
                ),
                new ReferenceFactory(exclusions)
            );

            engine.Execute(args[0], args[1]);
        }
    }
}
