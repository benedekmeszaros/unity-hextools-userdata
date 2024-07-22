namespace HexTools.Persitence
{
    public interface IData
    {
        public string Name { get; }
        public bool Exists { get; }
        public string Extension { get; }
        public string RelativePath { get; }
        public string AbsolutePath { get; }

        public void Save();
        public void Read();
        public void Unload();
        public bool Remove();
    }
}
