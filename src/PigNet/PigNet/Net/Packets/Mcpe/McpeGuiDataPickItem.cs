
namespace PigNet.Net.Packets.Mcpe;

public class McpeGuiDataPickItem : Packet<McpeGuiDataPickItem>
{
	public McpeGuiDataPickItem()
	{
		Id = 0x36;
		IsMcpe = true;
	}
}