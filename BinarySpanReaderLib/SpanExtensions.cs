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
        public static uint ReadUInt32BigEndian(this ReadOnlySpan<byte> span, int position) =>
            unchecked(
                ((uint)span[position] << 24) |
                ((uint)span[position + 1] << 16) |
                ((uint)span[position + 2] << 8) |
                span[position + 3]);

        /// <summary>
        /// Reads an unsigned, 32-bit integer using Big Endian (BE) from a <see cref="ReadOnlySpan{T}"/>
        /// at a specific <paramref name="position"/> and casts it to a specific <see cref="uint"/>
        /// <typeparamref name="TEnum"/> enum.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="uint"/> enum type.</typeparam>
        /// <param name="span"></param>
        /// <param name="position">The byte position to read the <see cref="uint"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        /// <exception cref="InvalidCastException"><typeparamref name="TEnum"/> is not
        /// a <see cref="uint"/> enum; it's probably a regular <see cref="int"/> enum.</exception>
        public static TEnum ReadUInt32BigEndianEnum<TEnum>(this ReadOnlySpan<byte> span, int position)
            where TEnum : Enum =>
            (TEnum)(object)ReadUInt32BigEndian(span, position);

        /// <summary>
        /// Read an unsigned, 32-bit integer using Little Endian (LE) from a <see cref="ReadOnlySpan{T}"/>
        /// at a specific <paramref name="position"/>.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="position">The byte position to read the <see cref="uint"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        public static uint ReadUInt32LittleEndian(this ReadOnlySpan<byte> span, int position) =>
            unchecked(
                span[position] |
                ((uint)span[position + 1] << 8) |
                ((uint)span[position + 2] << 16) |
                ((uint)span[position + 3] << 24));

        /// <summary>
        /// Reads an unsigned, 32-bit integer using Little Endian (LE) from a <see cref="ReadOnlySpan{T}"/>
        /// at a specific <paramref name="position"/> and casts it to a specific <see cref="uint"/>
        /// <typeparamref name="TEnum"/> enum.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="uint"/> enum type.</typeparam>
        /// <param name="span"></param>
        /// <param name="position">The byte position to read the <see cref="uint"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        /// <exception cref="InvalidCastException"><typeparamref name="TEnum"/> is not
        /// a <see cref="uint"/> enum; it's probably a regular <see cref="int"/> enum.</exception>
        public static TEnum ReadUInt32LittleEndianEnum<TEnum>(this ReadOnlySpan<byte> span, int position)
            where TEnum : Enum =>
            (TEnum)(object)ReadUInt32LittleEndian(span, position);

        /// <summary>
        /// Read a signed, 32-bit integer using Big Endian (BE) from a <see cref="ReadOnlySpan{T}"/>
        /// at a specific <paramref name="position"/>.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="position">The byte position to read the <see cref="int"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        public static int ReadInt32BigEndian(this ReadOnlySpan<byte> span, int position) =>
            unchecked(
                (span[position] << 24) |
                (span[position + 1] << 16) |
                (span[position + 2] << 8) |
                span[position + 3]);

        /// <summary>
        /// Reads a signed, 32-bit integer using Big Endian (BE) from a <see cref="ReadOnlySpan{T}"/>
        /// at a specific <paramref name="position"/> and casts it to a regular <see cref="int"/>
        /// <typeparamref name="TEnum"/> enum.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="int"/> enum type.</typeparam>
        /// <param name="span"></param>
        /// <param name="position">The byte position to read the <see cref="int"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        /// <exception cref="InvalidCastException"><typeparamref name="TEnum"/> is not
        /// a regular <see cref="int"/> enum; it's probably a <see cref="uint"/> enum.</exception>
        public static TEnum ReadInt32BigEndianEnum<TEnum>(this ReadOnlySpan<byte> span, int position)
            where TEnum : Enum =>
            (TEnum)(object)ReadInt32BigEndian(span, position);

        /// <summary>
        /// Read a signed, 32-bit integer using Little Endian (LE) from a <see cref="ReadOnlySpan{T}"/>
        /// at a specific <paramref name="position"/>.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="position">The byte position to read the <see cref="int"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        public static int ReadInt32LittleEndian(this ReadOnlySpan<byte> span, int position) =>
            unchecked(
                span[position] |
                (span[position + 1] << 8) |
                (span[position + 2] << 16) |
                (span[position + 3] << 24));

        /// <summary>
        /// Reads a signed, 32-bit integer using Little Endian (LE) from a <see cref="ReadOnlySpan{T}"/>
        /// at a specific <paramref name="position"/> and casts it to a regular <see cref="int"/>
        /// <typeparamref name="TEnum"/> enum.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="int"/> enum type.</typeparam>
        /// <param name="span"></param>
        /// <param name="position">The byte position to read the <see cref="int"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        /// <exception cref="InvalidCastException"><typeparamref name="TEnum"/> is not
        /// a regular <see cref="int"/> enum; it's probably a <see cref="uint"/> enum.</exception>
        public static TEnum ReadInt32LittleEndianEnum<TEnum>(this ReadOnlySpan<byte> span, int position)
            where TEnum : Enum =>
            (TEnum)(object)ReadInt32LittleEndian(span, position);
    }
}
