using NUnit.Framework;
using System;
using System.Buffers.Binary;
using System.Linq;
using System.Runtime.InteropServices;

namespace BinarySpanReaderLib.Tests
{
    public class SpanExtensionsTests
    {
        #region Enums
        public enum RegularEnum
        {
            Foo,
            Bar,
            Baz
        }

        public enum UintEnum : uint
        {
            Foo,
            Bar,
            Baz
        }

        [Flags]
        public enum RegularFlagsEnum
        {
            None,
            Foo,
            Bar
        }

        [Flags]
        public enum UintFlagsEnum : uint
        {
            None,
            Foo,
            Bar
        }
        #endregion

        #region structs
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SimpleStruct
        {
            public int Int32Value;
            public uint UInt32Value;
            public RegularEnum RegularEnum;
            public UintEnum UintEnum;
            public RegularFlagsEnum RegularFlagsEnum;
            public UintFlagsEnum UintFlagsEnum;

            public override bool Equals(object? obj) =>
                (obj is SimpleStruct simpleStruct) && Equals(simpleStruct);

            public bool Equals(SimpleStruct simpleStruct) =>
                Int32Value == simpleStruct.Int32Value &&
                UInt32Value == simpleStruct.UInt32Value &&
                RegularEnum == simpleStruct.RegularEnum &&
                UintEnum == simpleStruct.UintEnum &&
                RegularFlagsEnum == simpleStruct.RegularFlagsEnum &&
                UintFlagsEnum == simpleStruct.UintFlagsEnum;

            public override int GetHashCode() => base.GetHashCode();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FooStruct
        {
            public uint Foo;
            public int Bar;
            public ulong Reserved1;

            public override bool Equals(object? obj) =>
                (obj is FooStruct fooStruct) && Equals(fooStruct);

            public bool Equals(FooStruct fooStruct) =>
                Foo == fooStruct.Foo &&
                Bar == fooStruct.Bar &&
                Reserved1 == fooStruct.Reserved1;

            public override int GetHashCode() => base.GetHashCode();

            public static bool operator ==(FooStruct a, FooStruct b) => a.Equals(b);
            public static bool operator !=(FooStruct a, FooStruct b) => a.Equals(b) is not true;
        }

        public struct InnerStruct
        {
            public ulong Foo;
            public long Bar;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct OuterStruct
        {
            public uint Foo;
            public int Bar;
            public InnerStruct InnerStruct;

            public override bool Equals(object? obj) =>
                (obj is OuterStruct outerStruct) && Equals(outerStruct);

            public bool Equals(OuterStruct outerStruct) =>
                Foo == outerStruct.Foo &&
                Bar == outerStruct.Bar &&
                InnerStruct.Foo == outerStruct.InnerStruct.Foo &&
                InnerStruct.Bar == outerStruct.InnerStruct.Bar;

            public override int GetHashCode() => base.GetHashCode();
        }
        #endregion

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
        private static readonly object[] Uint32BETestCases = new object[]
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

        private static readonly object[] UInt32BEUintEnumTestCases = new object[]
        {
            new object[] { Uint32BETestData, 0, UintEnum.Foo },
            new object[] { Uint32BETestData, 1 * sizeof(int), UintEnum.Bar },
            new object[] { Uint32BETestData, 2 * sizeof(int), UintEnum.Baz }
        };

        [TestCaseSource(nameof(UInt32BEUintEnumTestCases))]
        public void ReadUInt32BigEndian_UintEnum(ReadOnlyMemory<byte> mem, int position, UintEnum enumVal) =>
            Assert.That(mem.Span.ReadUInt32BigEndianEnum<UintEnum>(position), Is.EqualTo(enumVal));

        [TestCase(0)]
        [TestCase(1 * sizeof(int))]
        [TestCase(2 * sizeof(int))]
        public void ReadUInt32BigEndian_RegularEnum_ThrowsException(int position) =>
            Assert.That(() => Int32BETestData.Span.ReadUInt32BigEndianEnum<RegularEnum>(position),
                Throws.TypeOf<InvalidCastException>());

        private static readonly object[] Uint32BEUintFlagsEnumTestCases = new object[]
        {
            new object[] { Uint32BETestData, 0, UintFlagsEnum.None },
            new object[] { Uint32BETestData, 1 * sizeof(int), UintFlagsEnum.Foo },
            new object[] { Uint32BETestData, 2 * sizeof(int), UintFlagsEnum.Bar },
            new object[] { new ReadOnlyMemory<byte>(new byte[] { 0, 0, 0, 3 }), 0,
                UintFlagsEnum.Foo | UintFlagsEnum.Bar }
        };

        [TestCaseSource(nameof(Uint32BEUintFlagsEnumTestCases))]
        public void ReadUint32BigEndian_UintFlagsEnum(ReadOnlyMemory<byte> mem, int position, UintFlagsEnum enumVal) =>
            Assert.That(mem.Span.ReadUInt32BigEndianEnum<UintFlagsEnum>(position), Is.EqualTo(enumVal));

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
        private static readonly object[] Uint32LETestCases = new object[]
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

        private static readonly object[] UInt32LEUintEnumTestCases = new object[]
        {
            new object[] { Uint32LETestData, 0, UintEnum.Foo },
            new object[] { Uint32LETestData, 1 * sizeof(int), UintEnum.Bar },
            new object[] { Uint32LETestData, 2 * sizeof(int), UintEnum.Baz }
        };

        [TestCaseSource(nameof(UInt32LEUintEnumTestCases))]
        public void ReadUInt32LittleEndian_UintEnum(ReadOnlyMemory<byte> mem, int position, UintEnum enumVal) =>
            Assert.That(mem.Span.ReadUInt32LittleEndianEnum<UintEnum>(position), Is.EqualTo(enumVal));

        [TestCase(0)]
        [TestCase(1 * sizeof(int))]
        [TestCase(2 * sizeof(int))]
        public void ReadUInt32LittleEndian_RegularEnum_ThrowsException(int position) =>
            Assert.That(() => Int32LETestData.Span.ReadUInt32LittleEndianEnum<RegularEnum>(position),
                Throws.TypeOf<InvalidCastException>());

        private static readonly object[] Uint32LEUintFlagsEnumTestCases = new object[]
        {
            new object[] { Uint32LETestData, 0, UintFlagsEnum.None },
            new object[] { Uint32LETestData, 1 * sizeof(int), UintFlagsEnum.Foo },
            new object[] { Uint32LETestData, 2 * sizeof(int), UintFlagsEnum.Bar },
            new object[] { new ReadOnlyMemory<byte>(new byte[] { 3, 0, 0, 0 }), 0,
                UintFlagsEnum.Foo | UintFlagsEnum.Bar }
        };

        [TestCaseSource(nameof(Uint32LEUintFlagsEnumTestCases))]
        public void ReadUint32LittleEndian_UintFlagsEnum(ReadOnlyMemory<byte> mem, int position, UintFlagsEnum enumVal) =>
            Assert.That(mem.Span.ReadUInt32LittleEndianEnum<UintFlagsEnum>(position), Is.EqualTo(enumVal));

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
        private static readonly object[] Int32BETestCases = new object[]
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

        private static readonly object[] Int32BERegularEnumTestCases = new object[]
        {
            new object[] { Int32BETestData, 0, RegularEnum.Foo },
            new object[] { Int32BETestData, 1 * sizeof(int), RegularEnum.Bar },
            new object[] { Int32BETestData, 2 * sizeof(int), RegularEnum.Baz }
        };

        [TestCaseSource(nameof(Int32BERegularEnumTestCases))]
        public void ReadInt32BigEndian_RegularEnum(ReadOnlyMemory<byte> mem, int position, RegularEnum enumVal) =>
            Assert.That(mem.Span.ReadInt32BigEndianEnum<RegularEnum>(position), Is.EqualTo(enumVal));

        [TestCase(0)]
        [TestCase(1 * sizeof(int))]
        [TestCase(2 * sizeof(int))]
        public void ReadInt32BigEndian_UintEnum_ThrowsException(int position) =>
            Assert.That(() => Uint32BETestData.Span.ReadInt32BigEndianEnum<UintEnum>(position),
                Throws.TypeOf<InvalidCastException>());

        private static readonly object[] Int32BERegularFlagsEnumTestCases = new object[]
        {
            new object[] { Int32BETestData, 0, RegularFlagsEnum.None },
            new object[] { Int32BETestData, 1 * sizeof(int), RegularFlagsEnum.Foo },
            new object[] { Int32BETestData, 2 * sizeof(int), RegularFlagsEnum.Bar },
            new object[] { new ReadOnlyMemory<byte>(new byte[] { 0, 0, 0, 3 }), 0,
                RegularFlagsEnum.Foo | RegularFlagsEnum.Bar }
        };

        [TestCaseSource(nameof(Int32BERegularFlagsEnumTestCases))]
        public void ReadInt32BigEndian_RegularFlagsEnum(ReadOnlyMemory<byte> mem, int position, RegularFlagsEnum enumVal) =>
            Assert.That(mem.Span.ReadInt32BigEndianEnum<RegularFlagsEnum>(position), Is.EqualTo(enumVal));

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
        private static readonly object[] Int32LETestCases = new object[]
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

        private static readonly object[] Int32LERegularEnumTestCases = new object[]
        {
            new object[] { Int32LETestData, 0, RegularEnum.Foo },
            new object[] { Int32LETestData, 1 * sizeof(int), RegularEnum.Bar },
            new object[] { Int32LETestData, 2 * sizeof(int), RegularEnum.Baz }
        };

        [TestCaseSource(nameof(Int32LERegularEnumTestCases))]
        public void ReadInt32LittleEndian_RegularEnum(ReadOnlyMemory<byte> mem, int position, RegularEnum enumVal) =>
            Assert.That(mem.Span.ReadInt32LittleEndianEnum<RegularEnum>(position), Is.EqualTo(enumVal));

        [TestCase(0)]
        [TestCase(1 * sizeof(int))]
        [TestCase(2 * sizeof(int))]
        public void ReadInt32LittleEndian_UintEnum_ThrowsException(int position) =>
            Assert.That(() => Uint32LETestData.Span.ReadInt32LittleEndianEnum<UintEnum>(position),
                Throws.TypeOf<InvalidCastException>());

        private static readonly object[] Int32LERegularFlagsEnumTestCases = new object[]
        {
            new object[] { Int32LETestData, 0, RegularFlagsEnum.None },
            new object[] { Int32LETestData, 1 * sizeof(int), RegularFlagsEnum.Foo },
            new object[] { Int32LETestData, 2 * sizeof(int), RegularFlagsEnum.Bar },
            new object[] { new ReadOnlyMemory<byte>(new byte[] { 3, 0, 0, 0 }), 0,
                RegularFlagsEnum.Foo | RegularFlagsEnum.Bar }
        };

        [TestCaseSource(nameof(Int32LERegularFlagsEnumTestCases))]
        public void ReadInt32LittleEndian_RegularFlagsEnum(ReadOnlyMemory<byte> mem, int position, RegularFlagsEnum enumVal) =>
            Assert.That(mem.Span.ReadInt32LittleEndianEnum<RegularFlagsEnum>(position), Is.EqualTo(enumVal));

        [TestCase(-1)]
        [TestCase(500)]
        public void ReadInt32LittleEndian_Throws_OnInvalidPosition(int position) =>
            Assert.That(() => Int32LETestData.Span.ReadInt32LittleEndian(position),
                Throws.TypeOf<IndexOutOfRangeException>());
        #endregion

        #region CreateStruct
        [Test]
        public void CreateStruct_SimpleStruct_Works() =>
            Assert.That(new ReadOnlyMemory<byte>(new byte[]
                {
                    // Int32Value
                    0, 0, 0, 0,
                    // UInt32Value
                    16, 0, 0, 0,
                    // RegularEnum.Bar
                    1, 0, 0, 0,
                    // UintEnum.Baz
                    2, 0, 0, 0,
                    // RegularFlagsEnum
                    3, 0, 0, 0,
                    // UintFlagsEnum
                    0, 0, 0, 0
                }).CreateStruct<SimpleStruct>(0), Is.EqualTo(new SimpleStruct
                {
                    Int32Value = 0,
                    UInt32Value = 16,
                    RegularEnum = RegularEnum.Bar,
                    UintEnum = UintEnum.Baz,
                    RegularFlagsEnum = RegularFlagsEnum.Foo | RegularFlagsEnum.Bar,
                    UintFlagsEnum = UintFlagsEnum.None
                }));

        [Test]
        public void CreateStruct_FooStruct_Works() =>
            Assert.That(new ReadOnlyMemory<byte>(new byte[]
                {
                    //public uint Foo;
                    1, 0, 0, 0,
                    //public int Bar;
                    2, 0, 0, 0,
                    //public ulong Reserved1;
                    3, 0, 0, 0, 0, 0, 0, 0
                }).CreateStruct<FooStruct>(0), Is.EqualTo(new FooStruct
                {
                    Foo = 1,
                    Bar = 2,
                    Reserved1 = 3
                }));

        [Test]
        public void CreateStruct_OuterStruct_Works() =>
            Assert.That(new ReadOnlyMemory<byte>(new byte[]
                {
                    //public uint Foo;
                    1, 0, 0, 0,
                    //public int Bar;
                    255, 255, 255, 255,
                    //public ulong Foo;
                    255, 0, 0, 0, 0, 0, 0, 0,
                    //public long Bar;
                    255, 0, 0, 0, 0, 0, 0, 0
                }).CreateStruct<OuterStruct>(0), Is.EqualTo(new OuterStruct
                {
                    Foo = 1,
                    Bar = -1,
                    InnerStruct = new InnerStruct
                    {
                        Foo = 255,
                        Bar = 255
                    }
                }));
        #endregion

        #region WriteToMemory
        [Test]
        public void WriteToMemory_Works()
        {
            var @struct = new OuterStruct
            {
                Foo = 1,
                Bar = -1,
                InnerStruct = new InnerStruct
                {
                    Foo = 255,
                    Bar = 255
                }
            };
            CollectionAssert.AreEqual(
                new byte[]
                {
                    //public uint Foo;
                    1, 0, 0, 0,
                    //public int Bar;
                    255, 255, 255, 255,
                    //public ulong Foo;
                    255, 0, 0, 0, 0, 0, 0, 0,
                    //public long Bar;
                    255, 0, 0, 0, 0, 0, 0, 0
                },
                SpanExtensions.WriteToMemory(ref @struct).ToArray());
        }
        #endregion
    }
}