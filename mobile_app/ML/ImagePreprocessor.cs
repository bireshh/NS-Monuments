using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace PrvaApp.ML
{
    public class ImagePreprocessor
    {
        private const int InputSize = 384;
        private const int ResizeSize = (int)(InputSize * 1.1); 

        // ImageNet normalization constants
        private static readonly float[] Mean = { 0.485f, 0.456f, 0.406f };
        private static readonly float[] Std = { 0.229f, 0.224f, 0.225f };

        public static DenseTensor<float> Preprocess(byte[] imageBytes)
        {
            using var bitmap = DecodeAndResize(imageBytes);
            using var cropped = CenterCrop(bitmap);
            return ToNormalizedTensor(cropped);
        }

        private static SKBitmap DecodeAndResize(byte[] imageBytes)
        {
            using var original = SKBitmap.Decode(imageBytes);

            float scale = (float)ResizeSize / Math.Min(original.Width, original.Height);
            int newW = (int)Math.Round(original.Width * scale);
            int newH = (int)Math.Round(original.Height * scale);

            return original.Resize(new SKImageInfo(newW, newH), SKFilterQuality.High);
        }

        private static SKBitmap CenterCrop(SKBitmap bitmap)
        {
            int x = (bitmap.Width - InputSize) / 2;
            int y = (bitmap.Height - InputSize) / 2;

            var cropped = new SKBitmap(InputSize, InputSize);
            using var canvas = new SKCanvas(cropped);
            canvas.DrawBitmap(bitmap,
                new SKRect(x, y, x + InputSize, y + InputSize), // src
                new SKRect(0, 0, InputSize, InputSize));          // dst
            return cropped;
        }

        private static DenseTensor<float> ToNormalizedTensor(SKBitmap bitmap)
        {
            var tensor = new DenseTensor<float>(new[] { 1, 3, InputSize, InputSize });

            for (int y = 0; y < InputSize; y++)
            {
                for (int x = 0; x < InputSize; x++)
                {
                    SKColor pixel = bitmap.GetPixel(x, y);

                    float r = pixel.Red / 255f;
                    float g = pixel.Green / 255f;
                    float b = pixel.Blue / 255f;

                    // Normalize: (value - mean) / std
                    tensor[0, 0, y, x] = (r - Mean[0]) / Std[0];
                    tensor[0, 1, y, x] = (g - Mean[1]) / Std[1];
                    tensor[0, 2, y, x] = (b - Mean[2]) / Std[2];
                }
            }

            return tensor;
        }
    }
}
