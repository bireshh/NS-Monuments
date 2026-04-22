using System;
using System.Collections.Generic;
using System.Text;
using PrvaApp.Data.Model;
using SQLite;

namespace PrvaApp.Data
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _db;

        public static async Task InitAsync()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test_database.db");
            _db = new SQLiteAsyncConnection(path);
        }

        public static async Task<List<Monument>> GetAllWithEmbeddingsAsync()
        {
            var monuments = await _db.Table<Monument>().ToListAsync();
            var vectors = await _db.Table<MonumentVector>().ToListAsync();

            var grouped = vectors.GroupBy(v => v.MonumentName);
            foreach (var group in grouped)
            {
                var monument = monuments.FirstOrDefault(m => m.MonumentName == group.Key);
                if (monument != null)
                    monument.Embeddings = group.Select(v => v.Embedding).ToList();
            }

            return monuments;
        }
    }
}
