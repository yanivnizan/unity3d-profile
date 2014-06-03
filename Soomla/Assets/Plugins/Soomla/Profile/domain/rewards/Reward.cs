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
	public abstract class Reward {

		public string RewardId;
		public string Name;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public Reward(string rewardId, string name)
		{
			RewardId = rewardId;
			Name = name;
		}
		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		public VirtualGood(AndroidJavaObject jniVirtualGood) 
//			: base(jniVirtualGood)
//		{
//		}
//#endif
//		/// <summary>
//		/// see parent.
//		/// </summary>
//		public Reward(JSONObject jsonVg)
//		{
//
//		}

		/// <summary>
		/// see parent.
		/// </summary>
		public virtual JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(PJSONConsts.BP_REWARD_REWARDID, RewardId);
			obj.AddField(PJSONConsts.BP_NAME, Name);
			
			return obj;
		}

	}
}
