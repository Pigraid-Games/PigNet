
namespace PigNet.Net.Packets.Mcpe;

public class McpeSetTitle : Packet<McpeSetTitle>
{
	public int fadeInTime;
	public int fadeOutTime;
	public string filteredString;
	public string platformOnlineId;
	public int stayTime;
	public string text;

	public int type;
	public string xuid;

	public McpeSetTitle()
	{
		Id = 0x58;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(type);
		Write(text);
		WriteSignedVarInt(fadeInTime);
		WriteSignedVarInt(stayTime);
		WriteSignedVarInt(fadeOutTime);
		Write(xuid);
		Write(platformOnlineId);
		Write(filteredString);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		type = ReadSignedVarInt();
		text = ReadString();
		fadeInTime = ReadSignedVarInt();
		stayTime = ReadSignedVarInt();
		fadeOutTime = ReadSignedVarInt();
		xuid = ReadString();
		platformOnlineId = ReadString();
		filteredString = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		type = default;
		text = default;
		fadeInTime = default;
		stayTime = default;
		fadeOutTime = default;
		xuid = default;
		platformOnlineId = default;
	}
}