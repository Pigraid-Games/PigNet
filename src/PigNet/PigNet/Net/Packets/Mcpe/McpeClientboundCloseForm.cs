
namespace PigNet.Net.Packets.Mcpe;

public class McpeClientboundCloseForm : Packet<McpeClientboundCloseForm>
{
	public McpeClientboundCloseForm()
	{
		Id = 0x136;
		IsMcpe = true;
	}
}