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
	/// </summary>
	public class BadgeReward : Reward {
		public string IconUrl;

		/// <summary>
		/// Constructor.
		/// </summary>
		public BadgeReward(string rewardId, string name)
			: base(rewardId, name)
		{
		}

		public BadgeReward(string rewardId, string name, string iconUrl)
			: base(rewardId, name)
		{
			IconUrl = iconUrl;
		}
		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		public SingleUseVG(AndroidJavaObject jniSingleUseVG) 
//			: base(jniSingleUseVG)
//		{
//		}
//#endif
//		/// <summary>
//		/// see parent.
//		/// </summary>
//		public SingleUseVG(JSONObject jsonVg)
//			: base(jsonVg)
//		{
//		}
//
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(PJSONConsts.BP_REWARD_ICONURL, IconUrl);
			
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
