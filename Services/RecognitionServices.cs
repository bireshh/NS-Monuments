using PrvaApp.Data;
using PrvaApp.Data.Model;
using PrvaApp.ML;

namespace PrvaApp
{
    public class RecognitionResult
    {
        public string MonumentName { get; set; }
        public string Description { get; set; }
        public float Score { get; set; }
    }

    internal class CachedVector
    {
        public string MonumentName { get; set; }
        public float[] Embedding { get; set; }
    }

    public class RecognitionService
    {
        private List<CachedVector> _vectorCache = new();

        // ── Init ──────────────────────────────────────────────────
        public async Task InitAsync()
        {
            // AppInitService copies test_database.db
            await AppInitService.InitializeAsync();

            // DatabaseService opens the connection
            await DatabaseService.InitAsync();

            // EmbeddingService copies and loads inference_model.onnx
            await EmbeddingService.InitAsync();

            // Cache all vectors from DB into memory
            await WarmUpAsync();
        }

        private async Task WarmUpAsync()
        {
            // GetAllWithEmbeddingsAsync loads monuments + attaches float[] embeddings
            var monuments = await DatabaseService.GetAllWithEmbeddingsAsync();

            _vectorCache = monuments
                .Where(m => m.Embeddings != null && m.Embeddings.Count > 0)
                .SelectMany(m => m.Embeddings.Select(emb => new CachedVector
                {
                    MonumentName = m.MonumentName,
                    Embedding = emb
                }))
                .ToList();
        }

        // ── Recognition ───────────────────────────────────────────
        public async Task<List<RecognitionResult>> RecognizeAsync(byte[] imageBytes, int topN = 3)
        {
            // Run inference on background thread — keeps UI responsive
            float[] queryEmbedding = await Task.Run(() => EmbeddingService.GetEmbedding(imageBytes));

            // Dot product == cosine similarity since embeddings are L2-normalized
            var results = _vectorCache
                .Select(v => (
                    name: v.MonumentName,
                    score: DotProduct(queryEmbedding, v.Embedding)
                ))
                .GroupBy(x => x.name)
                .Select(g => new RecognitionResult
                {
                    MonumentName = g.Key,
                    Score = g.Max(x => x.score) // best single vector per monument
                })
                .OrderByDescending(x => x.Score)
                .Take(topN)
                .ToList();

            // Enrich with descriptions from monument list
            var monuments = await DatabaseService.GetAllWithEmbeddingsAsync();
            foreach (var result in results)
            {
                var monument = monuments.FirstOrDefault(m => m.MonumentName == result.MonumentName);
                if (monument != null)
                    result.Description = monument.MonumentDescription;
            }

            return results;
        }

        // ── Math ──────────────────────────────────────────────────
        private static float DotProduct(float[] a, float[] b)
        {
            float dot = 0f;
            for (int i = 0; i < a.Length; i++)
                dot += a[i] * b[i];
            return dot;
        }
    }
}