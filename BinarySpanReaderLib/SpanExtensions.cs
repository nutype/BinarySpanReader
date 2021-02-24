using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BinarySpanReaderLib
{
    /// <summary>
    /// Extension methods for <see cref="ReadOnlySpan{T}"/> and <see cref="ReadOnlyMemory{T}"/> which
    /// read various common data types (e.g., <see cref="int"/>) or structs.
    /// </summary>
    public static class SpanExtensions
    {
        #region UInt32
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
        #endregion

        #region Int32
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
        /// Read a signed, 32-bit integer using Big Endian (BE) from a <see cref="ReadOnlyMemory{T}"/>
        /// at a specific <paramref name="position"/>.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="position">The byte position to read the <see cref="int"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        public static int ReadInt32BigEndian(this ReadOnlyMemory<byte> memory, int position) =>
            ReadInt32BigEndian(memory.Span, position);

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
        /// Reads a signed, 32-bit integer using Big Endian (BE) from a <see cref="ReadOnlyMemory{T}"/>
        /// at a specific <paramref name="position"/> and casts it to a regular <see cref="int"/>
        /// <typeparamref name="TEnum"/> enum.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="int"/> enum type.</typeparam>
        /// <param name="memory"></param>
        /// <param name="position">The byte position to read the <see cref="int"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        /// <exception cref="InvalidCastException"><typeparamref name="TEnum"/> is not
        /// a regular <see cref="int"/> enum; it's probably a <see cref="uint"/> enum.</exception>
        public static TEnum ReadInt32BigEndianEnum<TEnum>(this ReadOnlyMemory<byte> memory, int position)
            where TEnum : Enum =>
            ReadInt32BigEndianEnum<TEnum>(memory.Span, position);

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
        /// Read a signed, 32-bit integer using Little Endian (LE) from a <see cref="ReadOnlyMemory{T}"/>
        /// at a specific <paramref name="position"/>.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="position">The byte position to read the <see cref="int"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        public static int ReadInt32LittleEndian(this ReadOnlyMemory<byte> memory, int position) =>
            ReadInt32LittleEndian(memory.Span, position);

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

        /// <summary>
        /// Reads a signed, 32-bit integer using Little Endian (LE) from a <see cref="ReadOnlyMemory{T}"/>
        /// at a specific <paramref name="position"/> and casts it to a regular <see cref="int"/>
        /// <typeparamref name="TEnum"/> enum.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="int"/> enum type.</typeparam>
        /// <param name="memory"></param>
        /// <param name="position">The byte position to read the <see cref="int"/> from.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="position"/> is outside the range
        /// of the <see cref="ReadOnlySpan{T}"/>.</exception>
        /// <exception cref="InvalidCastException"><typeparamref name="TEnum"/> is not
        /// a regular <see cref="int"/> enum; it's probably a <see cref="uint"/> enum.</exception>
        public static TEnum ReadInt32LittleEndianEnum<TEnum>(this ReadOnlyMemory<byte> memory, int position)
            where TEnum : Enum =>
            ReadInt32LittleEndianEnum<TEnum>(memory.Span, position);
        #endregion

        #region CreateStruct
        /// <summary>
        /// Creates a readonly reference to a <typeparamref name="TStruct"/> created using the bytes
        /// starting at the provided <paramref name="position"/>.  The endianess of the various data
        /// types defined within the <typeparamref name="TStruct"/> is determiend based on the endianess
        /// of this machine.
        /// </summary>
        /// <typeparam name="TStruct">The type of struct to create; this should be
        /// a readonly struct.</typeparam>
        /// <param name="span"></param>
        /// <param name="position">The byte position to create the <typeparamref name="TStruct"/> from.</param>
        /// <returns></returns>
        /// <seealso cref="BitConverter.IsLittleEndian"/>
        public static ref readonly TStruct CreateStruct<TStruct>(this ReadOnlySpan<byte> span, int position)
            where TStruct : struct =>
            ref MemoryMarshal.AsRef<TStruct>(span.Slice(position, Marshal.SizeOf<TStruct>()));

        /// <summary>
        /// Creates a readonly reference to a <typeparamref name="TStruct"/> created using the bytes
        /// starting at the provided <paramref name="position"/>.  The endianess of the various data
        /// types defined within the <typeparamref name="TStruct"/> is determiend based on the endianess
        /// of this machine.
        /// </summary>
        /// <typeparam name="TStruct">The type of struct to create; this should be
        /// a readonly struct.</typeparam>
        /// <param name="memory"></param>
        /// <param name="position">The byte position to create the <typeparamref name="TStruct"/> from.</param>
        /// <returns></returns>
        /// <seealso cref="BitConverter.IsLittleEndian"/>
        public static ref readonly TStruct CreateStruct<TStruct>(this ReadOnlyMemory<byte> memory, int position)
            where TStruct : struct => ref CreateStruct<TStruct>(memory.Span, position);
        #endregion

        #region WriteToMemory
        /// <summary>
        /// Writes a <typeparamref name="TStruct"/> to memory inside a
        /// <see cref="ReadOnlySpan{T}"/>.  The endianess of the data types in <typeparamref name="TStruct"/>
        /// is based on the machine's endianess.
        /// </summary>
        /// <typeparam name="TStruct"></typeparam>
        /// <param name="struct">The <typeparamref name="TStruct"/> to write to memory.</param>
        /// <returns></returns>
        public static ReadOnlySpan<byte> WriteToMemory<TStruct>(ref TStruct @struct) where TStruct : struct
        {
            var span = new Span<byte>(new byte[Marshal.SizeOf<TStruct>()]);
            MemoryMarshal.Write(span, ref @struct);
            return span;
        }
        #endregion
    }
}
