
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class BookEditReplacePage
{
	public byte PageIndex { get; set; }
	public string Text1 { get; set; }
	public string Text2 { get; set; }
}

public class BookEditAddPage
{
	public byte PageIndex { get; set; }
	public string Text1 { get; set; }
	public string Text2 { get; set; }
}

public class BookEditDeletePage
{
	public byte PageIndex { get; set; }
}

public class BookEditSwapPages
{
	public byte PageIndex1 { get; set; }
	public byte PageIndex2 { get; set; }
}

public class BookEditFinalize
{
	public string TextA { get; set; }
	public string TextB { get; set; }
	public string Xuid { get; set; }
}

public class McpeBookEdit : Packet<McpeBookEdit>
{
	public BookEditAction action;
	public byte bookSlot;
	public object data;
	
	public McpeBookEdit()
	{
		Id = 0x61;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		
		Write((byte) action);
		Write(bookSlot);

		switch (action)
		{
			case BookEditAction.ReplacePage:
				if (data is BookEditReplacePage replacePage)
				{
					Write(replacePage.PageIndex);
					Write(replacePage.Text1);
					Write(replacePage.Text2);
				}
				break;
			case BookEditAction.AddPage:
				if (data is BookEditAddPage addPage)
				{
					Write(addPage.PageIndex);
					Write(addPage.Text1);
					Write(addPage.Text2);
				}
				break;
			case BookEditAction.DeletePage:
				if (data is BookEditDeletePage deletePage) Write(deletePage.PageIndex);
				break;
			case BookEditAction.SwapPages:
				if (data is BookEditSwapPages swapPages)
				{
					Write(swapPages.PageIndex1);
					Write(swapPages.PageIndex2);
				}
				break;
			case BookEditAction.Finalize:
				if (data is BookEditFinalize finalize)
				{
					Write(finalize.TextA);
					Write(finalize.TextB);
					Write(finalize.Xuid);
				}
				break;
			default:
				data = null;
				break;
		}
	}
	
	protected override void DecodePacket()
	{
		base.DecodePacket();

		action = (BookEditAction) ReadByte();
		bookSlot = ReadByte();

		data = action switch
		{
			BookEditAction.ReplacePage => new BookEditReplacePage
			{
				PageIndex = ReadByte(),
				Text1 = ReadString(),
				Text2 = ReadString()
			},
			BookEditAction.AddPage => new BookEditAddPage
			{
				PageIndex = ReadByte(),
				Text1 = ReadString(),
				Text2 = ReadString()
			},
			BookEditAction.DeletePage => new BookEditDeletePage { PageIndex = ReadByte() },
			BookEditAction.SwapPages => new BookEditSwapPages
			{
				PageIndex1 = ReadByte(),
				PageIndex2 = ReadByte()
			},
			BookEditAction.Finalize => new BookEditFinalize
			{
				TextA = ReadString(),
				TextB = ReadString(),
				Xuid = ReadString()
			},
			_ => null
		};
	}
	
	protected override void ResetPacket()
	{
		base.ResetPacket();
		
		action = default;
		bookSlot = default;
		data = default;
	}
}