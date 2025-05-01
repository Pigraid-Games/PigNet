
using PigNet.Net.EnumerationsTable;
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeResourcePackClientResponse : Packet<McpeResourcePackClientResponse>
{

	public ResourcePackIds resourcepackids;
	public ResourcePackResponse responseStatus;

	public McpeResourcePackClientResponse()
	{
		Id = 0x08;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write((byte) responseStatus);
		Write(resourcepackids);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		responseStatus = (ResourcePackResponse) ReadByte();
		resourcepackids = ReadResourcePackIds();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		responseStatus = default;
		resourcepackids = default;
	}
}