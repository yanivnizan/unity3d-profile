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
	/// A specific type of <code>Reward</code> that represents a badge
	/// with an icon. For example: when the user achieves a top score,
	/// the user can earn a "Highest Score" badge reward.
	/// </summary>
	public class BadgeReward : Reward {
		public string IconUrl;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="rewardId">see parent</param>
		/// <param name="name">see parent</param>
		public BadgeReward(string rewardId, string name)
			: base(rewardId, name)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="rewardId">see parent</param>
		/// <param name="name">see parent</param>
		/// <param name="iconUrl">A url to the icon of this Badge on the device.</param>
		public BadgeReward(string rewardId, string name, string iconUrl)
			: base(rewardId, name)
		{
			IconUrl = iconUrl;
		}

		/// <summary>
		/// see parent.
		/// </summary>
		public BadgeReward(JSONObject jsonReward)
			: base(jsonReward)
		{
			IconUrl = jsonReward[PJSONConsts.BP_REWARD_ICONURL].str;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(PJSONConsts.BP_REWARD_ICONURL, IconUrl);
			obj.AddField(PJSONConsts.BP_TYPE, "badge");

			
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
