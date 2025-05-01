
namespace PigNet.Net.Packets.Mcpe;

public class McpePhotoTransfer : Packet<McpePhotoTransfer>
{
	public string photoName;
	public string photoData;
	public string bookId;
	public byte type;
	public byte sourceType;
	public long ownerId;
	public string newPhotoName;

	public McpePhotoTransfer()
	{
		Id = 0x63;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(photoName);
		Write(photoData);
		Write(bookId);
		Write(type);
		Write(sourceType);
		WriteVarLong(ownerId);
		Write(newPhotoName);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		photoName = ReadString();
		photoData = ReadString();
		bookId = ReadString();
		type = ReadByte();
		sourceType = ReadByte();
		ownerId = ReadVarLong();
		newPhotoName = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		photoName = default;
		photoData = default;
		bookId = default;
		type = default;
		sourceType = default;
		ownerId = default;
		newPhotoName = default;
	}
}