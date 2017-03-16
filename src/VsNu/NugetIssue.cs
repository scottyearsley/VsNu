namespace VsNu
{
    public class NugetIssue
    {
        public NugetIssue(NugetIssueType type, AssemblyRef assembly, Package package)
        {
            IssueType = type;
            Assembly = assembly;
            Package = package;
        }

        public AssemblyRef Assembly { get; }

        public Package Package { get; }

        public NugetIssueType IssueType { get; }
    }

    public enum NugetIssueType
    {
        MissingPackageEntry,
        VersionMismatch
    }
}
