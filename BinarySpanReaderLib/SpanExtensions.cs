using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySpanReaderLib
{
    public static class SpanExtensions
    {
        public static uint ReadUInt32BigEndian(this ReadOnlySpan<byte> span, int position)
        {
            uint num = 0;

            for (int i = 0, j = 3; i < 4; i++, j--)
                num |= (uint)span[position + i] << (j * 8);

            return num;
        }

        public static uint ReadUInt32LittleEndian(this ReadOnlySpan<byte> span, int position)
        {
            uint num = 0;

            for (int i = 0; i < 4; i++)
                num |= (uint)span[position + i] << (i * 8);

            return num;
        }
    }
}
