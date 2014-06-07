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
	/// </summary>
	public class RandomReward : Reward {
		public List<Reward> Rewards;

		/// <summary>
		/// Constructor.
		/// </summary>

		public RandomReward(string rewardId, string name, List<Reward> rewards)
			: base(rewardId, name)
		{
			Rewards = rewards;
		}

		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		public SingleUseVG(AndroidJavaObject jniSingleUseVG) 
//			: base(jniSingleUseVG)
//		{
//		}
//#endif

		/// <summary>
		/// see parent.
		/// </summary>
		public RandomReward(JSONObject jsonReward)
			: base(jsonReward)
		{
			ArrayList rewardsObj = jsonReward[PJSONConsts.BP_REWARDS].list;
			Rewards = new List<Reward>();
			foreach(JSONObject rewardObj in rewardsObj) {
				Rewards.Add(Reward.fromJSONObject(rewardObj));
			}
		}

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
//
//		/// <summary>
//		/// Saves this instance.
//		/// </summary>
//		public void save() 
//		{
//			save("SingleUseVG");
//		}
	}
}
