using System.Xml.Linq;
using VsNu.NuGet;

namespace VsNu
{
    public class PackageFactory : IPackageFactory
    {
        private readonly IPackageRepository _packageRepository;

        public PackageFactory(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public Package Create(XElement element)
        {
            var package = new Package(_packageRepository, 
                element.Attribute("id")?.Value,
                element.Attribute("version")?.Value);

            return package;
        }
    }
}
