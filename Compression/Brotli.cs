using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLSecure.Compression
{
    public class Brotli : BaseCompressor
    {
        public override string Header => "BROTLI";

        public override async Task<byte[]> Compress(byte[] data)
        {
            using (MemoryStream ms = new())
            {
                await ms.WriteAsync(Encoding.UTF8.GetBytes(Header));
                using (MemoryStream compressed = new())
                {
                    using (BrotliStream bs = new(compressed, CompressionMode.Compress))
                    {
                        await bs.WriteAsync(data);
                    }
                    await ms.WriteAsync(compressed.ToArray());
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
                using (BrotliStream bs = new(ms, CompressionMode.Decompress))
                {
                    using (MemoryStream decompressed = new())
                    {
                        await bs.CopyToAsync(decompressed);
                        return decompressed.ToArray();
                    }
                }
            }
        }
    }
}