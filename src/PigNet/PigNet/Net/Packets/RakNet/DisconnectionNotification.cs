
namespace PigNet.Net.Packets.RakNet;

public class DisconnectionNotification : Packet<DisconnectionNotification>
{
	public DisconnectionNotification()
	{
		Id = 0x15;
		IsMcpe = false;
	}
}