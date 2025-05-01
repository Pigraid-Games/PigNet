
namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateAdventureSettings : Packet<McpeUpdateAdventureSettings>
{
	public bool autoJump;
	public bool immutableWorld;
	public bool noMvp;

	public bool noPvm;
	public bool showNametags;

	public McpeUpdateAdventureSettings()
	{
		Id = 0xbc;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(noPvm);
		Write(noMvp);
		Write(immutableWorld);
		Write(showNametags);
		Write(autoJump);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		noPvm = ReadBool();
		noMvp = ReadBool();
		immutableWorld = ReadBool();
		showNametags = ReadBool();
		autoJump = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		noPvm = default;
		noMvp = default;
		immutableWorld = default;
		showNametags = default;
		autoJump = default;
	}
}