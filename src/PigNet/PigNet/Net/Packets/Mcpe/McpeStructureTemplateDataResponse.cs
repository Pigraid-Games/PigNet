
namespace PigNet.Net.Packets.Mcpe;

public class McpeStructureTemplateDataResponse : Packet<McpeStructureTemplateDataResponse>
{
	// TODO: Implement : https://mojang.github.io/bedrock-protocol-docs/html/StructureTemplateDataResponsePacket.html
	public McpeStructureTemplateDataResponse()
	{
		Id = 0x85;
		IsMcpe = true;
	}
}