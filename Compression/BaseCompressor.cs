using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TLSecure.Compression
{
    public abstract class BaseCompressor
    {
        public abstract string Header { get; }

        public abstract Task<byte[]> Compress(byte[] data);
        public abstract Task<byte[]> Decompress(byte[] data); 
    }
}