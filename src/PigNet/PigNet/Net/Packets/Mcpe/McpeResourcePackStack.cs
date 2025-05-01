
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeResourcePackStack : Packet<McpeResourcePackStack>
{
	public ResourcePackIdVersions behaviorpackidversions;
	public Experiments experiments;
	public bool experimentsPreviouslyToggled;
	public string gameVersion;
	public bool hasEditorPacks;

	public bool mustAccept;
	public ResourcePackIdVersions resourcepackidversions;

	public McpeResourcePackStack()
	{
		Id = 0x07;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(mustAccept);
		Write(behaviorpackidversions);
		Write(resourcepackidversions);
		Write(gameVersion);
		Write(experiments);
		Write(experimentsPreviouslyToggled);
		Write(hasEditorPacks);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		mustAccept = ReadBool();
		behaviorpackidversions = ReadResourcePackIdVersions();
		resourcepackidversions = ReadResourcePackIdVersions();
		gameVersion = ReadString();
		experiments = ReadExperiments();
		experimentsPreviouslyToggled = ReadBool();
		hasEditorPacks = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		mustAccept = default;
		behaviorpackidversions = default;
		resourcepackidversions = default;
		gameVersion = default;
		experiments = default;
		experimentsPreviouslyToggled = default;
		hasEditorPacks = default;
	}
}