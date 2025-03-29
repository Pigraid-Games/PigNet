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

namespace PigNet.Net.EnumerationsTable;

public enum MapDecorationType
{
	MarkerWhite = 0,
	MarkerGreen = 1,
	MarkerRed = 2,
	MarkerBlue = 3,
	XWhite = 4,
	TriangleRed = 5,
	SquareWhite = 6,
	MarkerSign = 7,
	MarkerPink = 8,
	MarkerOrange = 9,
	MarkerYellow = 10,
	MarkerTeal = 11,
	TriangleGreen = 12,
	SmallSquareWhite = 13,
	Mansion = 14,
	Monument = 15,
	NoDraw = 16,
	VillageDesert = 17,
	VillagePlains = 18,
	VillageSavanna = 19,
	VillageSnowy = 20,
	VillageTaiga = 21,
	JungleTemple = 22,
	WitchHut = 23,
	TrialChambers = 24,
	Count = 25,
	Player = MarkerWhite,
	PlayerOffMap = SquareWhite,
	PlayerOffLimits = SmallSquareWhite,
	PlayerHidden = NoDraw,
	ItemFrame = MarkerGreen
}