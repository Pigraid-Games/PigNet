
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSetScoreboardIdentity : Packet<McpeSetScoreboardIdentity>
{
	public enum Operations
	{
		RegisterIdentity = 0,
		ClearIdentity = 1
	}

	public ScoreboardIdentityEntries entries;

	public McpeSetScoreboardIdentity()
	{
		Id = 0x70;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(entries);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		entries = ReadScoreboardIdentityEntries();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		entries = default;
	}
}