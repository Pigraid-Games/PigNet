
namespace PigNet.Net.Packets.Mcpe;

public class McpeServerSettingsRequest : Packet<McpeServerSettingsRequest> // will be removed in the future
{
	public McpeServerSettingsRequest()
	{
		Id = 0x66;
		IsMcpe = true;
	}
}