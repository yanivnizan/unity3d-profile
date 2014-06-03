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
using System;
using System.Runtime.InteropServices;

namespace Soomla.Profile {

	/// <summary>
	/// <c>SoomlaProfile</c> for Android. 
	/// This class holds the basic assets needed to operate the Profile module.
	/// This is the only class you need to initialize in order to use the SOOMLA Profile SDK.
	/// </summary>
	public class SoomlaProfileAndroid : SoomlaProfile {

#if UNITY_ANDROID

		/// <summary>
		/// Initializes the SOOMLA Profile SDK.
		/// </summary>
		/// </exception>
		protected override void _initialize() {

			AndroidJNI.PushLocalFrame(100);
			//init EventHandler
			using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.profile.unity.EventHandler")) {
				jniEventHandler.CallStatic("initialize");
			}

			using(AndroidJavaClass jniSoomlaProfileClass = new AndroidJavaClass("com.soomla.profile.SoomlaProfile")) {
				AndroidJavaObject jniSoomlaProfile = jniSoomlaProfileClass.CallStatic<AndroidJavaObject>("getInstance");
				jniSoomlaProfile.Call<bool>("initialize");
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>

		/// </summary>
		protected override void _updateStatus(Provider provider, string status, Reward reward) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "updateStatus", provider.ToString(), status, reward.toJSONObject().print());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>

		/// </summary>
		protected override UserProfile _getUserProfileLocally(Provider provider) {
			JSONObject upObj = null;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				string upJSON = ProfileJNIHandler.CallStatic<string>(jniSoomlaProfile, "getUserProfileLocally", provider.ToString());
				upObj = new JSONObject(upJSON);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);

			if (upObj) {
				return new UserProfile(upObj);
			} else {
				return null;
			}
		}

		/// <summary>/

		/// </summary>
		protected override void _logout(Provider provider) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "logout", provider.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>

		/// </summary>
		protected override void _login(Provider provider, Reward reward) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "login", provider.ToString(), reward.toJSONObject().print());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

#endif
	}
}
