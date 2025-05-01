
namespace PigNet.Net.Packets.Mcpe;

public class McpeClientToServerHandshake : Packet<McpeClientToServerHandshake>
{
	public McpeClientToServerHandshake()
	{
		Id = 0x04;
		IsMcpe = true;
	}
}