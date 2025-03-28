﻿#region LICENSE

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

using PigNet.Entities.Passive;

namespace PigNet.Entities.Behaviors
{
	public class OwnerHurtByTargetBehavior : BehaviorBase
	{
		private Wolf _wolf;

		public OwnerHurtByTargetBehavior(Wolf wolf)
		{
			_wolf = wolf;
		}

		public override bool ShouldStart()
		{
			if (!_wolf.IsTamed) return false;

			Player owner = (Player) _wolf.Owner;

			if (owner.HealthManager.LastDamageSource == null) return false;

			_wolf.SetTarget(owner.HealthManager.LastDamageSource);

			return true;
		}

		public override bool CanContinue()
		{
			return false;
		}
	}
}