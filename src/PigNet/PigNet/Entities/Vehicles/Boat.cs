using PigNet.Items;
using PigNet.Net.EnumerationsTable;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils.Metadata;
using PigNet.Worlds;

namespace PigNet.Entities.Vehicles;

public class Boat : Vehicle
{
	public Boat(Level level) : base(EntityType.Boat, level)
	{
		IsStackable = true;
		HasCollision = true;
		IsAffectedByGravity = true;
		Length = 1.4f;
		Height = 0.455f;
		NoAi = false;
	}

	public override Item[] GetDrops()
	{
		return new[]
		{
			new ItemBoat()
		};
	}

	public override void DoInteraction(int actionId, Player player)
	{
		player.Vehicle = EntityId;

		McpeSetActorLink link = McpeSetActorLink.CreateObject();
		link.linkType = ActorLinkType.Riding;
		link.riderId = player.EntityId;
		link.riddenId = EntityId;
		Level.RelayBroadcast(link);

		SendSetEntityData(player);
	}

	public void SendSetEntityData(Player player)
	{
		player.IsRiding = true;

		// FOR PLAYER
		MetadataDictionary metadata = player.GetMetadata();
		metadata[57] = new MetadataVector3(0, 1.02001f, 0);
		metadata[58] = new MetadataByte(1);
		metadata[59] = new MetadataFloat(90f);
		metadata[60] = new MetadataFloat(-90f);

		player.BroadcastSetEntityData(metadata);
	}
}