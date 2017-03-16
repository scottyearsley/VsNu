using System;
using System.Reflection;
using System.Xml.Linq;

namespace VsNu
{
    public class ProjectRef
    {
        public AssemblyRef Assembly { get; set; }

        public string HintPath { get; set; }

        public Project Project { get; set; }

        public static ProjectRef Create(XNamespace ns, XElement element, Project project)
        {
            var returnValue = new ProjectRef();
            var include = element.Attribute("Include");
            var hintPath = element.Element(ns + "HintPath")?.Value;

            if (include == null)
            {
                return null;
            }

            returnValue.Assembly = AssemblyRef.Create(include.Value);
            returnValue.HintPath = hintPath;
            returnValue.Project = project;
            return returnValue;
        }
    }
}
