namespace MiNET.Net.Packets.Mcpe;

public class McpeRequestAbility : Packet<McpeRequestAbility>
{
	public object Value = false;
	public int ability;
	
	public McpeRequestAbility()
	{
		Id = 0xb8;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		
		WriteVarInt(ability);
		
		switch (Value)
		{
			case bool boolean:
			{
				Write((byte) 1);
				Write(boolean);
				Write(0f);
				break;
			}

			case float floatingPoint:
			{
				Write((byte) 2);
				Write(false);
				Write(floatingPoint);
				break;
			}
		}
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		
		ability = ReadVarInt();
		
		byte type = ReadByte();
		bool boolValue = ReadBool();
		float floatValue = ReadFloat();

		switch (type)
		{
			case 1:
				Value = boolValue;
				break;
			case 2:
				Value = floatValue;
				break;
		}
	}

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		
		ability = default;
		Value = false;
	}
}