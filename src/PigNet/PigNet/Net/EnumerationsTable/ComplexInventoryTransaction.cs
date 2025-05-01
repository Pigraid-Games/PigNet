
namespace PigNet.Net.EnumerationsTable;

public enum ComplexInventoryTransaction
{
	NormalTransaction = 0,
	InventoryMismatch = 1,
	ItemUseTransaction = 2,
	ItemUseOnEntityTransaction = 3,
	ItemReleaseTransaction = 4
}