
namespace PigNet.Net.Packets.RakNet;


public class DetectLostConnections : Packet<DetectLostConnections>
{
	public DetectLostConnections()
	{
		Id = 0x04;
		IsMcpe = false;
	}
}
