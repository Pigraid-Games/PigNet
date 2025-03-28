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

namespace MiNET.Net.EnumerationsTable;

public enum EasingType
{
	Linear = 0,
	Spring = 1,
    
	// Quadratic
	InQuad = 2,
	OutQuad = 3,
	InOutQuad = 4,
    
	// Cubic
	InCubic = 5,
	OutCubic = 6,
	InOutCubic = 7,
    
	// Quartic
	InQuart = 8,
	OutQuart = 9,
	InOutQuart = 10,
    
	// Quintic
	InQuint = 11,
	OutQuint = 12,
	InOutQuint = 13,
    
	// Sinusoidal
	InSine = 14,
	OutSine = 15,
	InOutSine = 16,
    
	// Exponential
	InExpo = 17,
	OutExpo = 18,
	InOutExpo = 19,
    
	// Circular
	InCirc = 20,
	OutCirc = 21,
	InOutCirc = 22,
    
	// Bounce
	InBounce = 23,
	OutBounce = 24,
	InOutBounce = 25,
    
	// Back
	InBack = 26,
	OutBack = 27,
	InOutBack = 28,
    
	// Elastic
	InElastic = 29,
	OutElastic = 30,
	InOutElastic = 31,
    
	// Special values
	Count = 32,
	Invalid = 33
}