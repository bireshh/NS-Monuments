using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PrvaApp.Data.Model
{
    [Table("MONUMENT")]
    public class Monument
    {
        [PrimaryKey, Column("monument_name")]
        public string MonumentName { get; set; }

        [Column("monument_description")]
        public string MonumentDescription { get; set; }

        [Ignore]
        public List<float[]> Embeddings { get; set; } = new();
    }
    [Table("VECTOR")]
    public class MonumentVector
    {
        [PrimaryKey, AutoIncrement, Column("vector_id")]
        public int VectorId { get; set; }

        [Column("monument_name")]
        public string MonumentName { get; set; }

        [Column("vector_blob")]
        public byte[] VectorBlob { get; set; }

        [Ignore]
        public float[] Embedding
        {
            get
            {
                var floats = new float[VectorBlob.Length / sizeof(float)];
                Buffer.BlockCopy(VectorBlob, 0, floats, 0, VectorBlob.Length);
                return floats;
            }
        }
    }
}
