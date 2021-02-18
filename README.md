# BinarySpanReader
Binary Reader for Span&lt;T> and Memory&lt;T>.  As of right now, this consists of various
extension methods to ReadOnlySpan&lt;byte> and ReadOnlyMemory&lt;byte>.

# Usage
You can either:

* read individual `uint` or `int` values using Big/Little Endian at a specific byte
  position within a `ReadOnlySpan&lt;byte>` or `ReadOnlyMemory&lt;byte>` using one of the
  various extension methods (e.g., `ReadInt32LittleEndian()`); or
* create a readonly reference to a `struct` created using `CreateStruct&lt;TStruct>()`, although
  the endianess cannot currently be controlled with this extension method and so the endianess of
  the various `TStruct` data types will be determined based on your machine's endianess.

## Reading Enum Values
You can also, optionally, read individual `uint` or `int` values as an `enum` (including enums with the
`[Flags]` attribute), however, _this will incur a boxing and unboxing performance penality_.

## Creating Structs
Perhaps the most useful extension method is the `CreateStruct&lt;TStruct>()` method which takes in
a `TStruct` generic type (which must be a `struct` and ideally a `readonly struct`) and byte position
within a `ReadOnlySpan&lt;byte>` or `ReadOnlyMemory&lt;byte>` and creates the struct in a fashion
similar to `Marshal.StructureToPtr()`, but uses `MemoryMarshal.AsRef&lt;TStruct>()` instead.

The memory layout of the `TStruct` _should_ be controlled via the `[StructLayout]` attribute.

Of particular note, the endianess of various data types contained within the `TStruct` will be based on the
_endianess of the machine_.

Finally, the `CreateStruct&lt;TStruct>()` method returns a `readonly ref` to the `TStruct` which
was created instead of returning a copy of the `TStruct` by value.