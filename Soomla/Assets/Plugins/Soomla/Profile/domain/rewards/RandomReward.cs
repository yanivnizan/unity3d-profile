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
using System.Collections.Generic;


namespace Soomla.Profile {	

	/// <summary>
	/// A specific type of <code>Reward</code> that holds of list of other
	/// rewards. When this reward is given, it randomly chooses a reward from
	///	the list of rewards it internally holds.  For example: a user can earn a mystery box
	///	reward (<code>RandomReward</code>, which in fact grants the user a random reward between a
	/// "Mayor" badge (<code>BadgeReward</code>) and a speed boost (<code>VirtualItemReward</code>)
	/// </summary>
	public class RandomReward : Reward {
		public List<Reward> Rewards;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="rewardId">see parent.</param>
		/// <param name="name">see parent.</param>
		/// <param name="rewards">A list of rewards from which to choose the reward randomly.</param>
		public RandomReward(string rewardId, string name, List<Reward> rewards)
			: base(rewardId, name)
		{
			Rewards = rewards;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonReward">see parent.</param>
		public RandomReward(JSONObject jsonReward)
			: base(jsonReward)
		{
			ArrayList rewardsObj = jsonReward[PJSONConsts.BP_REWARDS].list;
			Rewards = new List<Reward>();
			foreach(JSONObject rewardObj in rewardsObj) {
				Rewards.Add(Reward.fromJSONObject(rewardObj));
			}
		}

		/// <summary>
		/// see parent.
		/// </summary>
		/// <returns>see parent.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(PJSONConsts.BP_TYPE, "random");

			JSONObject rewardsObj = new JSONObject(JSONObject.Type.ARRAY);
			foreach(Reward r in Rewards) {
				rewardsObj.Add(r.toJSONObject());
			}
			obj.AddField(PJSONConsts.BP_REWARDS, rewardsObj);
			
			return obj;
		}

	}
}
