using NUnit.Framework;
using System;
using System.Buffers.Binary;
using System.Linq;

namespace BinarySpanReaderLib.Tests
{
    public class SpanExtensionsTests
    {
        #region ReadUInt32BigEndian
        private static ReadOnlyMemory<byte> Uint32BETestData = new byte[]
        {
            // 0
            0, 0, 0, 0,
            // 1
            0, 0, 0, 1,
            // 2
            0, 0, 0, 2,
            // 16
            0, 0, 0, 16,
            // 32
            0, 0, 0, 32,
            // 64
            0, 0, 0, 64,
            // 255
            0, 0, 0, 255,
            // 256
            0, 0, 1, 0,
            // 1023
            0, 0, 3, 255,
            // 1024
            0, 0, 4, 0,
            // 65_535
            0, 0, 255, 255,
            // 65_536
            0, 1, 0, 0,
            // uint.MaxValue
            255, 255, 255, 255
        };
        private static object[] Uint32BETestCases = new object[]
        {
            new object[] { Uint32BETestData, 0, (uint)0 },
            new object[] { Uint32BETestData, 1 * sizeof(uint), (uint)1 },
            new object[] { Uint32BETestData, 2 * sizeof(uint), (uint)2 },
            new object[] { Uint32BETestData, 3 * sizeof(uint), (uint)16 },
            new object[] { Uint32BETestData, 4 * sizeof(uint), (uint)32 },
            new object[] { Uint32BETestData, 5 * sizeof(uint), (uint)64 },
            new object[] { Uint32BETestData, 6 * sizeof(uint), (uint)255 },
            new object[] { Uint32BETestData, 7 * sizeof(uint), (uint)256 },
            new object[] { Uint32BETestData, 8 * sizeof(uint), (uint)1023 },
            new object[] { Uint32BETestData, 9 * sizeof(uint), (uint)1024 },
            new object[] { Uint32BETestData, 10 * sizeof(uint), (uint)65_535 },
            new object[] { Uint32BETestData, 11 * sizeof(uint), (uint)65_536 },
            new object[] { Uint32BETestData, 12 * sizeof(uint), uint.MaxValue }
        };

        [TestCaseSource(nameof(Uint32BETestCases))]
        public void ReadUInt32BigEndian_IsValid(ReadOnlyMemory<byte> mem, int position, uint expectedValue) =>
            Assert.That(mem.Span.ReadUInt32BigEndian(position), Is.EqualTo(expectedValue));

        [TestCase(-1)]
        [TestCase(500)]
        public void ReadUint32BigEndian_Throws_OnInvalidPosition(int position) =>
            Assert.That(() => Uint32BETestData.Span.ReadUInt32BigEndian(position),
                Throws.TypeOf<IndexOutOfRangeException>());
        #endregion

        #region ReadUInt32LittleEndian
        private static ReadOnlyMemory<byte> Uint32LETestData = new byte[]
        {
            // 0
            0, 0, 0, 0,
            // 1
            1, 0, 0, 0,
            // 2
            2, 0, 0, 0,
            // 16
            16, 0, 0, 0,
            // 32
            32, 0, 0, 0,
            // 64
            64, 0, 0, 0,
            // 255
            255, 0, 0, 0,
            // 256
            0, 1, 0, 0,
            // 1023
            255, 3, 0, 0,
            // 1024
            0, 4, 0, 0,
            // 65_535
            255, 255, 0, 0,
            // 65_536
            0, 0, 1, 0,
            // uint.MaxValue
            255, 255, 255, 255
        };
        private static object[] Uint32LETestCases = new object[]
        {
            new object[] { Uint32LETestData, 0, (uint)0 },
            new object[] { Uint32LETestData, 1 * sizeof(uint), (uint)1 },
            new object[] { Uint32LETestData, 2 * sizeof(uint), (uint)2 },
            new object[] { Uint32LETestData, 3 * sizeof(uint), (uint)16 },
            new object[] { Uint32LETestData, 4 * sizeof(uint), (uint)32 },
            new object[] { Uint32LETestData, 5 * sizeof(uint), (uint)64 },
            new object[] { Uint32LETestData, 6 * sizeof(uint), (uint)255 },
            new object[] { Uint32LETestData, 7 * sizeof(uint), (uint)256 },
            new object[] { Uint32LETestData, 8 * sizeof(uint), (uint)1023 },
            new object[] { Uint32LETestData, 9 * sizeof(uint), (uint)1024 },
            new object[] { Uint32LETestData, 10 * sizeof(uint), (uint)65_535 },
            new object[] { Uint32LETestData, 11 * sizeof(uint), (uint)65_536 },
            new object[] { Uint32LETestData, 12 * sizeof(uint), uint.MaxValue }
        };

        [TestCaseSource(nameof(Uint32LETestCases))]
        public void ReadUInt32LittleEndian_IsValid(ReadOnlyMemory<byte> mem, int position, uint expectedValue) =>
            Assert.That(mem.Span.ReadUInt32LittleEndian(position), Is.EqualTo(expectedValue));

        [TestCase(-1)]
        [TestCase(500)]
        public void ReadUint32LittleEndian_Throws_OnInvalidPosition(int position) =>
            Assert.That(() => Uint32LETestData.Span.ReadUInt32LittleEndian(position),
                Throws.TypeOf<IndexOutOfRangeException>());
        #endregion

        #region ReadInt32BigEndian
        private static ReadOnlyMemory<byte> Int32BETestData = new byte[]
        {
            // 0
            0, 0, 0, 0,
            // 1
            0, 0, 0, 1,
            // 2
            0, 0, 0, 2,
            // 16
            0, 0, 0, 16,
            // 32
            0, 0, 0, 32,
            // 64
            0, 0, 0, 64,
            // 255
            0, 0, 0, 255,
            // 256
            0, 0, 1, 0,
            // 1023
            0, 0, 3, 255,
            // 1024
            0, 0, 4, 0,
            // 65_535
            0, 0, 255, 255,
            // 65_536
            0, 1, 0, 0,
            // Int.MaxValue
            127, 255, 255, 255,
            // -1
            255, 255, 255, 255,
            // -2
            255, 255, 255, 254,
            // -16
            255, 255, 255, 0xF0,
            // -32
            255, 255, 255, 0xE0,
            // -64
            255, 255, 255, 0xC0,
            // -255
            255, 255, 255, 0x01,
            // -256
            255, 255, 255, 0,
            // -1023
            255, 255, 0xFC, 0x01,
            // -1024
            255, 255, 0xFC, 0,
            // -65_535
            255, 255, 0, 1,
            // -65_536
            255, 255, 0, 0,
            // Int.MinValue
            128, 0, 0, 0
        };
        private static object[] Int32BETestCases = new object[]
        {
            new object[] { Int32BETestData, 0, 0 },
            new object[] { Int32BETestData, 1 * sizeof(int), 1 },
            new object[] { Int32BETestData, 2 * sizeof(int), 2 },
            new object[] { Int32BETestData, 3 * sizeof(int), 16 },
            new object[] { Int32BETestData, 4 * sizeof(int), 32 },
            new object[] { Int32BETestData, 5 * sizeof(int), 64 },
            new object[] { Int32BETestData, 6 * sizeof(int), 255 },
            new object[] { Int32BETestData, 7 * sizeof(int), 256 },
            new object[] { Int32BETestData, 8 * sizeof(int), 1023 },
            new object[] { Int32BETestData, 9 * sizeof(int), 1024 },
            new object[] { Int32BETestData, 10 * sizeof(int), 65_535 },
            new object[] { Int32BETestData, 11 * sizeof(int), 65_536 },
            new object[] { Int32BETestData, 12 * sizeof(int), int.MaxValue },

            new object[] { Int32BETestData, 13 * sizeof(int), -1 },
            new object[] { Int32BETestData, 14 * sizeof(int), -2 },
            new object[] { Int32BETestData, 15 * sizeof(int), -16 },
            new object[] { Int32BETestData, 16 * sizeof(int), -32 },
            new object[] { Int32BETestData, 17 * sizeof(int), -64 },
            new object[] { Int32BETestData, 18 * sizeof(int), -255 },
            new object[] { Int32BETestData, 19 * sizeof(int), -256 },
            new object[] { Int32BETestData, 20 * sizeof(int), -1023 },
            new object[] { Int32BETestData, 21 * sizeof(int), -1024 },
            new object[] { Int32BETestData, 22 * sizeof(int), -65_535 },
            new object[] { Int32BETestData, 23 * sizeof(int), -65_536 },
            new object[] { Int32BETestData, 24 * sizeof(int), int.MinValue }
        };

        [TestCaseSource(nameof(Int32BETestCases))]
        public void ReadInt32BigEndian_IsValid(ReadOnlyMemory<byte> mem, int position, int expectedValue) =>
            Assert.That(mem.Span.ReadInt32BigEndian(position), Is.EqualTo(expectedValue));

        [TestCase(-1)]
        [TestCase(500)]
        public void ReadInt32BigEndian_Throws_OnInvalidPosition(int position) =>
            Assert.That(() => Int32BETestData.Span.ReadInt32BigEndian(position),
                Throws.TypeOf<IndexOutOfRangeException>());
        #endregion

        #region ReadInt32LittleEndian
        private static ReadOnlyMemory<byte> Int32LETestData = new byte[]
        {
            // 0
            0, 0, 0, 0,
            // 1
            1, 0, 0, 0,
            // 2
            2, 0, 0, 0,
            // 16
            16, 0, 0, 0,
            // 32
            32, 0, 0, 0,
            // 64
            64, 0, 0, 0,
            // 255
            255, 0, 0, 0,
            // 256
            0, 1, 0, 0,
            // 1023
            255, 3, 0, 0,
            // 1024
            0, 4, 0, 0,
            // 65_535
            255, 255, 0, 0,
            // 65_536
            0, 0, 1, 0,
            // Int.MaxValue
            255, 255, 255, 127,
            // -1
            255, 255, 255, 255,
            // -2
            254, 255, 255, 255,
            // -16
            0xF0, 255, 255, 255,
            // -32
            0xE0, 255, 255, 255,
            // -64
            0xC0, 255, 255, 255,
            // -255
            0x01, 255, 255, 255,
            // -256
            0, 255, 255, 255,
            // -1023
            0x01, 0xFC, 255, 255,
            // -1024
            0, 0xFC, 255, 255,
            // -65_535
            1, 0, 255, 255,
            // -65_536
            0, 0, 255, 255, 
            // Int.MinValue
            0, 0, 0, 128
        };
        private static object[] Int32LETestCases = new object[]
        {
            new object[] { Int32LETestData, 0, 0 },
            new object[] { Int32LETestData, 1 * sizeof(int), 1 },
            new object[] { Int32LETestData, 2 * sizeof(int), 2 },
            new object[] { Int32LETestData, 3 * sizeof(int), 16 },
            new object[] { Int32LETestData, 4 * sizeof(int), 32 },
            new object[] { Int32LETestData, 5 * sizeof(int), 64 },
            new object[] { Int32LETestData, 6 * sizeof(int), 255 },
            new object[] { Int32LETestData, 7 * sizeof(int), 256 },
            new object[] { Int32LETestData, 8 * sizeof(int), 1023 },
            new object[] { Int32LETestData, 9 * sizeof(int), 1024 },
            new object[] { Int32LETestData, 10 * sizeof(int), 65_535 },
            new object[] { Int32LETestData, 11 * sizeof(int), 65_536 },
            new object[] { Int32LETestData, 12 * sizeof(int), int.MaxValue },

            new object[] { Int32LETestData, 13 * sizeof(int), -1 },
            new object[] { Int32LETestData, 14 * sizeof(int), -2 },
            new object[] { Int32LETestData, 15 * sizeof(int), -16 },
            new object[] { Int32LETestData, 16 * sizeof(int), -32 },
            new object[] { Int32LETestData, 17 * sizeof(int), -64 },
            new object[] { Int32LETestData, 18 * sizeof(int), -255 },
            new object[] { Int32LETestData, 19 * sizeof(int), -256 },
            new object[] { Int32LETestData, 20 * sizeof(int), -1023 },
            new object[] { Int32LETestData, 21 * sizeof(int), -1024 },
            new object[] { Int32LETestData, 22 * sizeof(int), -65_535 },
            new object[] { Int32LETestData, 23 * sizeof(int), -65_536 },
            new object[] { Int32LETestData, 24 * sizeof(int), int.MinValue }
        };

        [TestCaseSource(nameof(Int32LETestCases))]
        public void ReadInt32LittleEndian_IsValid(ReadOnlyMemory<byte> mem, int position, int expectedValue) =>
            Assert.That(mem.Span.ReadInt32LittleEndian(position), Is.EqualTo(expectedValue));

        [TestCase(-1)]
        [TestCase(500)]
        public void ReadInt32LittleEndian_Throws_OnInvalidPosition(int position) =>
            Assert.That(() => Int32LETestData.Span.ReadInt32LittleEndian(position),
                Throws.TypeOf<IndexOutOfRangeException>());
        #endregion
    }
}