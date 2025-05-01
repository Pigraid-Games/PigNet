
using PigNet.Utils;
using PigNet.Utils.Skins;

namespace PigNet.Net.Packets.Mcpe;

public class McpePlayerSkin : Packet<McpePlayerSkin>
{
	public bool isVerified;
	public string oldSkinName;
	public Skin skin;
	public string skinName;

	public UUID uuid;

	public McpePlayerSkin()
	{
		Id = 0x5d;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(uuid);
		Write(skin);
		Write(skinName);
		Write(oldSkinName);
		Write(isVerified);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		uuid = ReadUUID();
		skin = ReadSkin();
		skinName = ReadString();
		oldSkinName = ReadString();
		isVerified = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		uuid = default;
		skin = default;
		skinName = default;
		oldSkinName = default;
		isVerified = default;
	}
}