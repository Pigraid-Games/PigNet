
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeMapInfoRequest : Packet<McpeMapInfoRequest>
{
	public long mapUniqueId;
	public PixelList clientPixelList;

	public McpeMapInfoRequest()
	{
		Id = 0x44;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(mapUniqueId);
		WriteUnsignedVarInt(0);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		mapUniqueId = ReadSignedVarLong();
		clientPixelList = ReadPixelList();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		mapUniqueId = default;
		clientPixelList = default;
	}
}