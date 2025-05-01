
namespace PigNet.Net.Packets.Mcpe;

public class PurchaseReceipts
{
	public string[] ProofsOfPurchase { get; set; }
}

public class McpePurchaseReceipt : Packet<McpePurchaseReceipt>
{
	public uint size;
	public PurchaseReceipts purchaseReceipts;
	
	public McpePurchaseReceipt()
	{
		Id = 0x5c;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		
		WriteUnsignedVarInt(size);
		foreach (string proof in purchaseReceipts.ProofsOfPurchase) Write(proof);
	}
	
	protected override void DecodePacket()
	{
		base.DecodePacket();
		
		size = ReadUnsignedVarInt();
		purchaseReceipts = new PurchaseReceipts
		{
			ProofsOfPurchase = new string[size]
		};
		for (int i = 0; i < size; i++) purchaseReceipts.ProofsOfPurchase[i] = ReadString();
	}
}