
namespace PigNet.Net.Packets.Mcpe;

public class McpeSubClientLogin : Packet<McpeSubClientLogin>
{
	public McpeSubClientLogin()
	{
		Id = 0x5e;
		IsMcpe = true;
	}
}