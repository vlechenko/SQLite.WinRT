namespace SQLite.WinRT
{
    public class PlatformStorage : IPlatformStorage
    {
        private static IPlatformStorage instance;

        protected PlatformStorage()
        {
        }

        public static IPlatformStorage Instance
        {
            get { return instance ?? (instance = new PlatformStorage()); }
        }

        public void SetTempDirectory()
        {
            SQLite3.SetDirectory(/*temp directory type*/2, Windows.Storage.ApplicationData.Current.TemporaryFolder.Path);
        }

        public string GetDatabasePath(string databasePath)
        {
            return System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, databasePath);
        }
    }
}