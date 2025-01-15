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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;

namespace MiNET.Items;

public enum DyeMetadata
{
	Red = 1,
	Green = 2,
	Purple = 5,
	Cyan = 6,
	LightGray = 7,
	Gray = 8,
	Pink = 9,
	Lime = 10,
	Yellow = 11,
	LightBlue = 12,
	Magenta = 13,
	Orange = 14,
	Black = 16,
	Brown = 17,
	Blue = 18,
	White = 19,
}

public enum DyeColor
{
	White = 0,
	Orange = 1,
	Magenta = 2,
	LightBlue = 3,
	Yellow = 4,
	Lime = 5,
	Pink = 6,
	Gray = 7,
	LightGray = 8,
	Cyan = 9,
	Purple = 10,
	Blue = 11,
	Brown = 12,
	Red = 14,
	Green = 13,
	Black = 15,
	Unknown = 255
}

public class ItemDye() : Item("minecraft:dye", canInteract: false)
{
	public static byte ToColorCode(int metadata)
	{
		// Check if the metadata is valid in the DyeMetadata enum
		if (!Enum.IsDefined(typeof(DyeMetadata), metadata))
		{
			return (byte) DyeColor.Unknown;
		}

		DyeMetadata dyeMetadata = (DyeMetadata) metadata;

		return dyeMetadata switch
		{
			DyeMetadata.Red => (byte) DyeColor.Red,
			DyeMetadata.Green => (byte) DyeColor.Green,
			DyeMetadata.Purple => (byte) DyeColor.Purple,
			DyeMetadata.Cyan => (byte) DyeColor.Cyan,
			DyeMetadata.LightGray => (byte) DyeColor.LightGray,
			DyeMetadata.Gray => (byte) DyeColor.Gray,
			DyeMetadata.Pink => (byte) DyeColor.Pink,
			DyeMetadata.Lime => (byte) DyeColor.Lime,
			DyeMetadata.Yellow => (byte) DyeColor.Yellow,
			DyeMetadata.LightBlue => (byte) DyeColor.LightBlue,
			DyeMetadata.Magenta => (byte) DyeColor.Magenta,
			DyeMetadata.Orange => (byte) DyeColor.Orange,
			DyeMetadata.Black => (byte) DyeColor.Black,
			DyeMetadata.Brown => (byte) DyeColor.Brown,
			DyeMetadata.Blue => (byte) DyeColor.Blue,
			DyeMetadata.White => (byte) DyeColor.White,
			_ => (byte) DyeColor.Unknown
		};
	}
}