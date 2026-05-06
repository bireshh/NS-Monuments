using PrvaApp.Data;
using PrvaApp.Data.Model;
using PrvaApp.ML;

namespace PrvaApp
{
    public class RecognitionResult
    {
        public string MonumentName { get; set; }
        public string MonumentNameSerbian {  get; set; }
        public string Description { get; set; }
        public string DescriptionSerbian { get; set; }
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

        public async Task InitAsync()
        {
            await AppInitService.InitializeAsync();

            await DatabaseService.InitAsync();

            await EmbeddingService.InitAsync();

            await WarmUpAsync();
        }

        private async Task WarmUpAsync()
        {
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

        public async Task<List<RecognitionResult>> RecognizeAsync(byte[] imageBytes, int topN = 3)
        {
            float[] queryEmbedding = await Task.Run(() => EmbeddingService.GetEmbedding(imageBytes));

            var results = _vectorCache
                .Select(v => (
                    name: v.MonumentName,
                    score: DotProduct(queryEmbedding, v.Embedding)
                ))
                .GroupBy(x => x.name)
                .Select(g => new RecognitionResult
                {
                    MonumentName = g.Key,
                    Score = g.Max(x => x.score) 
                })
                .OrderByDescending(x => x.Score)
                .Take(topN)
                .ToList();

            var monuments = await DatabaseService.GetAllWithEmbeddingsAsync();
            foreach (var result in results)
            {
                var monument = monuments.FirstOrDefault(m => m.MonumentName == result.MonumentName);
                if (monument != null)
                {
                    result.Description = monument.MonumentDescription;
                    result.MonumentNameSerbian = monument.MonumentNameSerbian;
                    result.DescriptionSerbian = monument.MonumentDescriptionSerbian;
                }
                
            }

            return results;
        }

        private static float DotProduct(float[] a, float[] b)
        {
            float dot = 0f;
            for (int i = 0; i < a.Length; i++)
                dot += a[i] * b[i];
            return dot;
        }
    }
}