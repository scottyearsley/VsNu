namespace VsNu
{
    public class Package
    {
        public Package(string id, string version)
        {
            Id = id;
            Version = version;
        }

        public string Id { get;  }
        public string Version { get;  }
    }
}
