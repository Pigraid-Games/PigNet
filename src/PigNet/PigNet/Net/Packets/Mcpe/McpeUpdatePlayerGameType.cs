
namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdatePlayerGameType : Packet<McpeUpdatePlayerGameType>
{
	// Useless - same as SetDefaultGameType
	public McpeUpdatePlayerGameType()
	{
		Id = 0x97;
		IsMcpe = true;
	}
}