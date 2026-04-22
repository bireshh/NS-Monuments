using System;
using System.Collections.Generic;
using System.Text;

namespace PrvaApp.Data
{
    internal class AppInitService
    {
        private static readonly string BaseDir =
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static async Task InitializeAsync()
        {
            await CopyDatabaseIfNeeded();
        }

        private static async Task CopyDatabaseIfNeeded()
        {
            string dbPath = Path.Combine(BaseDir, "test_database.db");
            if (File.Exists(dbPath)) return;

            using var src = await FileSystem.OpenAppPackageFileAsync("test_database.db");
            using var dst = File.Create(dbPath);
            await src.CopyToAsync(dst);
        }
    }
}
