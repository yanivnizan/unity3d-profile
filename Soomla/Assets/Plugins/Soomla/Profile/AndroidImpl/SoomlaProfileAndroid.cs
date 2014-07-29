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
	/// 
	/// See comments for functions in parent.
	/// </summary>
	public class SoomlaProfileAndroid : SoomlaProfile {

#if UNITY_ANDROID

		protected override void _initialize(bool usingUnityProvider) {

			AndroidJNI.PushLocalFrame(100);
			//init EventHandler
			using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.profile.unity.EventHandler")) {
				jniEventHandler.CallStatic("initialize");
			}

			using(AndroidJavaClass jniSoomlaProfileClass = new AndroidJavaClass("com.soomla.profile.SoomlaProfile")) {
				AndroidJavaObject jniSoomlaProfile = jniSoomlaProfileClass.CallStatic<AndroidJavaObject>("getInstance");
				jniSoomlaProfile.Call("initialize", usingUnityProvider);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _updateStatus(Provider provider, string status, Reward reward) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				string rewardJSON = "";
				if (reward != null) {
					rewardJSON = reward.toJSONObject().print();
				}
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "updateStatus", provider.ToString(), status, rewardJSON);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _updateStory(Provider provider, string message, string name, 
		                                    string caption, string description, string link,
		                                    string pictureUrl, Reward reward) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				string rewardJSON = "";
				if (reward != null) {
					rewardJSON = reward.toJSONObject().print();
				}
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "updateStory", provider.ToString(), message, name,
				                                 caption, description, link, pictureUrl, rewardJSON);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _getContacts(Provider provider, Reward reward) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				string rewardJSON = "";
				if (reward != null) {
					rewardJSON = reward.toJSONObject().print();
				}
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "getContacts", provider.ToString(), rewardJSON);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override UserProfile _getStoredUserProfile(Provider provider) {
			JSONObject upObj = null;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				string upJSON = ProfileJNIHandler.CallStatic<string>(jniSoomlaProfile, "getStoredUserProfile", provider.ToString());
				if(upJSON != null) {
					upObj = new JSONObject(upJSON);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);

			if (upObj) {
				return new UserProfile(upObj);
			} else {
				return null;
			}
		}

		protected override void _storeUserProfile(UserProfile userProfile, bool notify) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "storeUserProfile",
				                                 userProfile.toJSONObject().ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _logout(Provider provider) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "logout", provider.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _login(Provider provider, Reward reward) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				string rewardJSON = "";
				if (reward != null) {
					rewardJSON = reward.toJSONObject().print();
				}
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "login", provider.ToString(), rewardJSON);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		// event pushing back to native (when using FB Unity SDK)
		protected override void _pushEventLoginStarted(Provider provider) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLoginStarted", provider.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLoginFinished(string userProfileJson) { 
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLoginFinished", userProfileJson);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLoginFailed(Provider provider, string message) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLoginFailed", provider.ToString(), message);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLoginCancelled(Provider provider) { 
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLoginCancelled", provider.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLogoutStarted(Provider provider) { 
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLogoutStarted", provider.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLogoutFinished(Provider provider) { 
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLogoutFinished", provider.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLogoutFailed(Provider provider, string message) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLogoutFailed",
				                                 provider.ToString(), message);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventSocialActionStarted(Provider provider, SocialActionType actionType) { 
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSocialActionStarted",
				                                 provider.ToString(), actionType.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventSocialActionFinished(Provider provider, SocialActionType actionType) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSocialActionFinished",
				                                 provider.ToString(), actionType.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventSocialActionCancelled(Provider provider, SocialActionType actionType) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSocialActionCancelled",
				                                 provider.ToString(), actionType.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventSocialActionFailed(Provider provider, SocialActionType actionType, string message) { 
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.UnitySoomlaProfile")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSocialActionFailed", 
				                                 provider.ToString(), actionType.ToString(), message);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

#endif
	}
}
