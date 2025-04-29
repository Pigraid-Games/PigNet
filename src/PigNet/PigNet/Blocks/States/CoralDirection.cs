#region LICENSE
// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;

namespace PigNet.Blocks.States;

public partial class CoralDirection
{
	internal CoralDirection() { }

	private CoralDirection(int value)
	{
		Value = value;
	}

	/// <summary>
	/// Value = 0
	/// </summary>
	public static readonly CoralDirection West = new(0);

	/// <summary>
	/// Value = 1
	/// </summary>
	public static readonly CoralDirection East = new(1);

	/// <summary>
	/// Value = 2
	/// </summary>
	public static readonly CoralDirection North = new CoralDirection(2);
		
	/// <summary>
	/// Value = 3
	/// </summary>
	public static readonly CoralDirection South = new CoralDirection(3);

	public static implicit operator CoralDirection(Utils.Direction direction)
	{
		return direction switch
		{
			Utils.Direction.South => South,
			Utils.Direction.West => West,
			Utils.Direction.North => North,
			Utils.Direction.East => East,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}

	public static implicit operator CoralDirection(PigNet.BlockFace face)
	{
		return face switch
		{
			PigNet.BlockFace.South => South,
			PigNet.BlockFace.West => West,
			PigNet.BlockFace.North => North,
			PigNet.BlockFace.East => East,
			_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
		};
	}
}