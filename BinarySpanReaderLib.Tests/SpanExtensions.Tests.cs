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
    }
}