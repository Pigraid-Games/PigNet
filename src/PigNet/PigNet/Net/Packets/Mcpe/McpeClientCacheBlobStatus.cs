
namespace PigNet.Net.Packets.Mcpe;

public class McpeClientCacheBlobStatus : Packet<McpeClientCacheBlobStatus>
{
	public ulong[] hashMisses;
	public ulong[] hashHits;
	
	public McpeClientCacheBlobStatus()
	{
		Id = 0x87;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt((uint) hashMisses.Length);
		WriteUnsignedVarInt((uint) hashHits.Length);
		WriteSpecial(hashMisses);
		WriteSpecial(hashHits);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		var lenMisses = ReadUnsignedVarInt();
		var lenHits = ReadUnsignedVarInt();

		hashMisses = ReadUlongsSpecial(lenMisses);
		hashHits = ReadUlongsSpecial(lenHits);
	}

	public void WriteSpecial(ulong[] values)
	{
		if (values == null) return;

		if (values.Length == 0) return;
		for (int i = 0; i < values.Length; i++)
		{
			ulong val = values[i];
			Write(val);
		}
	}

	public ulong[] ReadUlongsSpecial(uint len)
	{
		var values = new ulong[len];
		for (int i = 0; i < values.Length; i++)
		{
			values[i] = ReadUlong();
		}
		return values;
	}
}