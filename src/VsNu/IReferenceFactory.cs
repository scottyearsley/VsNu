using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VsNu
{
    public interface IReferenceFactory
    {
        IEnumerable<Reference> CreateReferences(XDocument doc, Project project);
    }

    public class ReferenceFactory : IReferenceFactory
    {
        private readonly IEnumerable<string> _exclusionRegEx;

        public ReferenceFactory(IEnumerable<string> exclusionRegEx)
        {
            _exclusionRegEx = exclusionRegEx;
        }

        public IEnumerable<Reference> CreateReferences(XDocument doc, Project project)
        {
            var references = new List<Reference>();
            var ns = doc.Root.Name.Namespace;

            var refIndicator = doc.Descendants(ns + "HintPath");

            foreach (var element in refIndicator)
            {
                var referenceElement = element.Parent;
                var include = referenceElement.Attribute("Include");

                if (include == null || IsExcluded(include.Value))
                {
                    continue;
                }

                var reference = Reference.Create(ns, referenceElement, project);
                references.Add(reference);
            }

            references.AddRange(references);

            return references;
        }

        private bool IsExcluded(string path)
        {
            var excluded = false;
            foreach (var exclusionPattern in _exclusionRegEx)
            {
                if (Regex.IsMatch(path, exclusionPattern, RegexOptions.IgnoreCase))
                {
                    excluded = true;
                    break;
                }
            }

            return excluded;
        }
    }
}
