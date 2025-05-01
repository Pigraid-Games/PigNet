
namespace PigNet.Net.Packets.Mcpe;

public class McpeStructureBlockUpdate : Packet<McpeStructureBlockUpdate>
{
	public McpeStructureBlockUpdate()
	{
		Id = 0x5a;
		IsMcpe = true;
	}
}