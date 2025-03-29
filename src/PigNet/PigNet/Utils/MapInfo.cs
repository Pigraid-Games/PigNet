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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using PigNet.Utils.Vectors;

namespace PigNet.Utils;

public class MapInfo : ICloneable
{
	public int Col;
	public byte[] Data;
	public MapDecorator[] Decorators = new MapDecorator[0];
	public long MapId;
	public BlockCoordinates Origin = new();
	public int Row;
	public int Scale;
	public MapTrackedObject[] TrackedObjects = new MapTrackedObject[0];
	public byte UpdateType;
	public byte X;
	public int XOffset;
	public byte Z;
	public int ZOffset;

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		return $"MapId: {MapId}, UpdateType: {UpdateType}, X: {X}, Z: {Z}, Col: {Col}, Row: {Row}, X-offset: {XOffset}, Z-offset: {ZOffset}, Data: {Data?.Length}";
	}
}

public class MapDecorator
{
	public uint Color;
	public byte Icon;
	public string Label;
	public byte Rotation;
	protected int Type;
	public byte X;
	public byte Z;
}

public class BlockMapDecorator : MapDecorator
{
	public BlockCoordinates Coordinates;

	public BlockMapDecorator()
	{
		Type = 1;
	}
}

public class EntityMapDecorator : MapDecorator
{
	public long EntityId;

	public EntityMapDecorator()
	{
		Type = 0;
	}
}

public class MapTrackedObject
{
	protected int Type;
}

public class EntityMapTrackedObject : MapTrackedObject
{
	public long EntityId;

	public EntityMapTrackedObject()
	{
		Type = 0;
	}
}

public class BlockMapTrackedObject : MapTrackedObject
{
	public BlockCoordinates Coordinates;

	public BlockMapTrackedObject()
	{
		Type = 1;
	}
}