
namespace PigNet.Net.Packets.Mcpe;

public class McpeLecternUpdate : Packet<McpeLecternUpdate>
{
	public McpeLecternUpdate()
	{
		Id = 0x7d;
		IsMcpe = true;
	}
}