using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.IO;

namespace IndexingServices
{
    public class DeflateAdapter 
    {
        public byte[] Compress(byte[] input)
        {
            MemoryStream outputStream = new MemoryStream();
            DeflateStream compressor = new DeflateStream(outputStream, CompressionMode.Compress);
            compressor.Write(input, 0, input.Length);
            compressor.Close();
            return outputStream.ToArray();
        }

        public byte[] Uncompress(byte[] input)
        {
            MemoryStream outputStream = new MemoryStream();
            DeflateStream decompressor = new DeflateStream(outputStream, CompressionMode.Decompress);
            decompressor.Write(input, 0, input.Length);
            decompressor.Close();
            return outputStream.ToArray();
        }
    }
}
