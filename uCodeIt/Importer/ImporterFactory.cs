namespace uCodeIt.Importer
{
    internal static class ImporterFactory
    {
        private static IImporter _instance;

        public static IImporter Instance
        {
            get
            {
                return _instance == null ? _instance = new DefaultImporter() : _instance;
            }
            set
            {
                _instance = value;
            }
        }

    }
}
