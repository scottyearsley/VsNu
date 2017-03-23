namespace VsNu
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyRef
    {
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

        public static AssemblyRef Create(string name, string version, string path)
        {
            return new AssemblyRef
            {
                Name = name,
                Version = version,
                Path = path
            };
        }

        public static AssemblyRef Create(string name)
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
