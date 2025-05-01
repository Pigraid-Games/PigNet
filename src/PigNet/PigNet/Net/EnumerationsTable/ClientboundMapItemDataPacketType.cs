
using System;

namespace PigNet.Net.EnumerationsTable;

[Flags]
public enum ClientboundMapItemDataPacketType
{
	Invalid = 0,
	TextureUpdate = 1 << 1,    // 2
	DecorationUpdate = 1 << 2, // 4
	Creation = 1 << 3          // 8
}