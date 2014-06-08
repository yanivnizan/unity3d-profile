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
	/// A reward is an entity which can be earned by the user for meeting certain
	/// criteria in game progress.  For example - a user can earn a badge for completing
	/// a mission. Dealing with <code>Reward</code>s is very similar to dealing with
	/// <code>VirtualItem</code>s: grant a reward by giving it and recall a
	/// reward by taking it.
	/// 
	/// In the Profile module, rewards can be attached to various actions. For example:
	/// You can give your user 100 coins for logging in through Facebook.
	/// </summary>
	public abstract class Reward {

		public string RewardId;
		public string Name;
		public bool   Repeatable;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="rewardId">the reward's ID (unique id to distinguish between rewards).</param>
		/// <param name="name">the reward's name.</param>
		public Reward(string rewardId, string name)
		{
			RewardId = rewardId;
			Name = name;
			Repeatable = false;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonReward">A JSONObject representation of <c>Reward</c>.</param>
		public Reward(JSONObject jsonReward)
		{
			RewardId = jsonReward[PJSONConsts.BP_REWARD_REWARDID].str;
			Name = jsonReward[PJSONConsts.BP_NAME].str;
			JSONObject repeatObj = jsonReward[PJSONConsts.BP_REWARD_REPEAT];
			if (repeatObj) {
				Repeatable = repeatObj.b;
			} else {
				Repeatable = false;
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>A JSONObject representation of <c>Reward</c>.</return>
		public virtual JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(PJSONConsts.BP_REWARD_REWARDID, RewardId);
			obj.AddField(PJSONConsts.BP_NAME, Name);
			obj.AddField(PJSONConsts.BP_REWARD_REPEAT, Repeatable);
			
			return obj;
		}

		/// <summary>
		/// Factory method to easily create a reward according to the given JSONObject.
		/// </summary>
		/// <returns>A JSONObject representation of <c>Reward</c>.</returns>
		/// <param name="rewardObj">The actual reward according to the given JSONObject.</param>
		public static Reward fromJSONObject(JSONObject rewardObj) {
			Reward reward = null;
			if (rewardObj) {
				string type = rewardObj[PJSONConsts.BP_TYPE].str;
				if (type == "badge") {
					reward = new BadgeReward(rewardObj);
				} else if (type == "item") {
					reward = new VirtualItemReward(rewardObj);
				} else if (type == "random") {
					reward = new RandomReward(rewardObj);
				} else if (type == "sequence") {
					reward = new SequenceReward(rewardObj);
				} else {
					Utils.LogError("SOOMLA/UNITY Reward", "Unknown reward type: " + type);
				}
			}
			return reward;
		}

	}
}
