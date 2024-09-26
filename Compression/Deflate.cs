using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Compression;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

namespace TLSecure.Compression
{
    public class Deflate : BaseCompressor
    {
        public override string Header => "DEFLATE";

        public override async Task<byte[]> Compress(byte[] data)
        {
            using (MemoryStream ms = new())
            {
                await ms.WriteAsync(Encoding.UTF8.GetBytes(Header));
                using (DeflateStream ds = new(ms, CompressionMode.Compress))
                {
                    await ds.WriteAsync(data);
                }
                return ms.ToArray();
            }
        }

        public override async Task<byte[]> Decompress(byte[] data)
        {
            using (MemoryStream ms = new(data))
            {
                ms.Seek(0, SeekOrigin.Begin);
                byte[] header = new byte[Header.Length];
                await ms.ReadAsync(header, 0, header.Length);
                if (!header.SequenceEqual(Encoding.UTF8.GetBytes(Header)))
                {
                    throw new InvalidDataException("Invalid header");
                }
                using (DeflateStream ds = new(ms, CompressionMode.Decompress))
                {
                    using (MemoryStream decompressed = new())
                    {
                        await ds.CopyToAsync(decompressed);
                        return decompressed.ToArray();
                    }
                }
            }
        }
    }
}