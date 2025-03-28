#region LICENSE
// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

using MiNET.Net.EnumerationsTable;

namespace MiNET.Net.Packets.Mcpe;

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