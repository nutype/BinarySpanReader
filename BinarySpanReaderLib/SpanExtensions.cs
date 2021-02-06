using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySpanReaderLib
{
    public static class SpanExtensions
    {
        /// <summary>
        /// Read an unsigned, 32-bit integer using Big Endian (BE) from a <see cref="ReadOnlySpan{T}"/>
        /// at a specific <paramref name="position"/>.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="position">The byte position to read the <see cref="uint"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        public static uint ReadUInt32BigEndian(this ReadOnlySpan<byte> span, int position)
        {
            uint num = 0;

            for (int i = 0, j = 3; i < 4; i++, j--)
                num |= (uint)span[position + i] << (j * 8);

            return num;
        }

        /// <summary>
        /// Read an unsigned, 32-bit integer using Little Endian (LE) from a <see cref="ReadOnlySpan{T}"/>
        /// at a specific <paramref name="position"/>.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="position">The byte position to read the <see cref="uint"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        public static uint ReadUInt32LittleEndian(this ReadOnlySpan<byte> span, int position)
        {
            uint num = 0;

            for (int i = 0; i < 4; i++)
                num |= (uint)span[position + i] << (i * 8);

            return num;
        }
    }
}
