using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TLSecure.Compression
{
    public static class CompressHelper
    {
        static Dictionary<byte[], BaseCompressor> compressors = new Dictionary<byte[], BaseCompressor>
        {
            { Encoding.UTF8.GetBytes("DEFLATE"), new Deflate() },
            { Encoding.UTF8.GetBytes("BROTLI"), new Brotli() }
        };

        public static async Task<byte[]> Decompress(byte[] data)
        {
            using (MemoryStream ms = new(data))
            {
                foreach (var compressor in compressors)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] key = compressor.Key;
                    byte[] header = new byte[key.Length];
                    await ms.ReadAsync(header, 0, header.Length);
                    if (header.SequenceEqual(key))
                    {
                        return await compressor.Value.Decompress(ms.ToArray());
                    }
                }
                return data;
            }
        }
    }
}