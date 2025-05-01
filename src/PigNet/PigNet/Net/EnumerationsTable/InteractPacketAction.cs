
namespace PigNet.Net.EnumerationsTable;

public enum InteractPacketAction : byte
{
	Invalid = 0,
	StopRiding = 3,
	InteractUpdate = 4,
	NpcOpen = 5,
	OpenInventory = 6
}