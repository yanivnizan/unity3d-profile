/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System.Collections;


namespace Soomla.Profile {	


	/// <summary>
	/// A specific type of <code>Reward</code> that has an associated
	/// virtual item.  The user is given this virtual item when the reward
	///	is granted.  For example: a user can earn a life reward (<code>VirtualItemReward</code>)
	/// which rewards the user with one life (<code>SingleUseVG</code>).
	/// </summary>
	public class VirtualItemReward : Reward {
		public int Amount;
		public string AssociatedItemId;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="rewardId">see parent.</param>
		/// <param name="name">see parent.</param>
		/// <param name="amount">The amount to give of the associated item when the reward is given.</param>
		/// <param name="associatedItemId">The ID of the virtual item associated with this reward.</param>
		public VirtualItemReward(string rewardId, string name, int amount, string associatedItemId)
			: base(rewardId, name)
		{
			AssociatedItemId = associatedItemId;
			Amount = amount;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonReward">see parent.</param>
		public VirtualItemReward(JSONObject jsonReward)
			: base(jsonReward)
		{
			AssociatedItemId = jsonReward[PJSONConsts.BP_ASSOCITEMID].str;
			Amount = (int)jsonReward[PJSONConsts.BP_REWARD_AMOUNT].n;
		}

		/// <summary>
		/// see parent.
		/// </summary>
		/// <returns>see parent.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(PJSONConsts.BP_ASSOCITEMID, AssociatedItemId);
			obj.AddField(PJSONConsts.BP_REWARD_AMOUNT, Amount);
			obj.AddField(PJSONConsts.BP_TYPE, "item");
			
			return obj;
		}

	}
}
