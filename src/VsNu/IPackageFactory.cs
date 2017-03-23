using NuGet;
using System.Xml.Linq;

namespace VsNu
{
    public interface IPackageFactory
    {
        Package Create(XElement element);
    }
}
