namespace VsNu
{
    /// <summary>
    /// A reference within a project to an Assembly.
    /// </summary>
    public class AssemblyRef
    {
        private AssemblyRef()
        { }

        public AssemblyRef(string name, string version, string path)
        {
            Name = name;
            Version = version;
            Path = path;
        }

        /// <summary>
        /// The name of the assembly.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The assembly version
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// The path to the assembly
        /// </summary>
        public string Path { get; private set; }

        public static AssemblyRef CreateFromAssemblyFullname(string name)
        {
            var assembyRef = new AssemblyRef();
            var split = name.Split(',');

            for (int index = 0; index < split.Length; index++)
            {
                var item = split[index];

                switch (index)
                {
                    case 0:
                        assembyRef.Name = item;
                        break;
                    case 1:
                        assembyRef.Version = item.Split('=')[1];
                        break;
                    default:
                        break;
                }
            }

            return assembyRef;
        }
    }
}
