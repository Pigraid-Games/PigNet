
using PigNet.Utils;
using PigNet.Utils.Skins;

namespace PigNet.Net.Packets.Mcpe;

public class FtlCreatePlayer : Packet<FtlCreatePlayer>
{
	public long clientId;
	public UUID clientuuid;
	public string serverAddress;
	public Skin skin;

	public string username;

	public FtlCreatePlayer()
	{
		Id = 0x01;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();


		Write(username);
		Write(clientuuid);
		Write(serverAddress);
		Write(clientId);
		Write(skin);

	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		username = ReadString();
		clientuuid = ReadUUID();
		serverAddress = ReadString();
		clientId = ReadLong();
		skin = ReadSkin();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		username = default;
		clientuuid = default;
		serverAddress = default;
		clientId = default;
		skin = default;
	}
}