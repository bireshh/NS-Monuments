using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrvaApp.ML
{
    public class EmbeddingService
    {
        private static InferenceSession _session;

        public static async Task InitAsync()
        {
            string modelPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "inference_model.onnx"
            );

            // Copy model from assets on first run (same pattern as DB)
            if (!File.Exists(modelPath))
            {
                using var src = await FileSystem.OpenAppPackageFileAsync("inference_model.onnx");
                using var dst = File.Create(modelPath);
                await src.CopyToAsync(dst);
            }

            _session = new InferenceSession(modelPath);
        }

        public static float[] GetEmbedding(byte[] imageBytes)
        {
            var preprocessor = new ImagePreprocessor();
            var tensor = ImagePreprocessor.Preprocess(imageBytes);

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input", tensor) // match your ONNX input name
            };

            using var results = _session.Run(inputs);
            var embedding = results.First().AsEnumerable<float>().ToArray();

            // L2 normalize (model does this too, but safe to repeat)
            float norm = MathF.Sqrt(embedding.Sum(x => x * x));
            return embedding.Select(x => x / norm).ToArray();
        }

    }
}
