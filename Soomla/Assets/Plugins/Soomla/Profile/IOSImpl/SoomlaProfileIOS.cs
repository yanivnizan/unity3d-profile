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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Soomla.Profile {
	
	/// <summary>
	/// <c>SoomlaProfile</c> for iOS.
	/// This class holds the basic assets needed to interact with Profile.
	/// You can use it to integrate users, login/logout, update status etc.
	/// This is the only class you need to initialize in order to use the SOOMLA SDK.
	/// </summary>
	public class SoomlaProfileIOS : SoomlaProfile {
		#if UNITY_IOS && !UNITY_EDITOR
		
		/// Functions that call iOS-store functions.
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_Initialize(bool usingUnityProvider);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_Login(string provider, string rewardJSON);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_Logout(string provider);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_UpdateStatus(string provider, string status, string rewardJSON);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_UpdateStory(string provider, string message, string name, 
		                                                     string caption, string description, string link,
		                                                     string pictureUrl, string rewardJSON);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_GetContacts(string provider, string rewardJSON);
		[DllImport ("__Internal")]
		private static extern int soomlaProfile_GetStoredUserProfile(string provider, out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int soomlaProfile_SetStoredUserProfile(string userProfileJson, bool notify);

		// event pushing back to native (when using FB Unity SDK)
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventLoginStarted(string provider);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventLoginFinished(string userProfileJson);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventLoginFailed(string provider, string message);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventLoginCancelled(string provider);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventLogoutStarted(string provider);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventLogoutFinished(string provider);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventLogoutFailed(string provider, string message);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventSocialActionStarted(string provider, string actionType);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventSocialActionFinished(string provider, string actionType);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventSocialActionCancelled(string provider, string actionType);
		[DllImport ("__Internal")]
		private static extern void soomlaProfile_PushEventSocialActionFailed(string provider, string actionType, string message);


		protected override void _initialize (bool usingUnityProvider) {
			soomlaProfile_Initialize(usingUnityProvider);
		}

		protected override void _login(Provider provider, Reward reward) {
			soomlaProfile_Login(provider.ToString(), GetRewardJsonStr(reward));
		}
		
		protected override void _logout(Provider provider) {
			soomlaProfile_Logout(provider.ToString());
		}
		
		protected override void _updateStatus(Provider provider, string status, Reward reward) { 
			soomlaProfile_UpdateStatus(provider.ToString(), status, GetRewardJsonStr(reward));
		}
		
		protected override void _updateStory(Provider provider, string message, string name, 
		                                    string caption, string description, string link,
		                                    string pictureUrl, Reward reward) {
			soomlaProfile_UpdateStory(provider.ToString(), message, name, caption, description,
			                          link, pictureUrl, GetRewardJsonStr(reward));
		}
		
		//		protected override void _uploadImage(Provider provider, string message, string filename,
		//		                                    byte[] imageBytes, int quality, Reward reward) { }
		//
		//		protected override void _uploadImage(Provider provider, string message, string filePath,
		//		                                    Reward reward) { }
		
		protected override void _getContacts(Provider provider, Reward reward) {
			soomlaProfile_GetContacts(provider.ToString(), GetRewardJsonStr(reward));
		}
		
		protected override UserProfile _getStoredUserProfile(Provider provider) { 
			IntPtr p = IntPtr.Zero;
			int err = soomlaProfile_GetStoredUserProfile(provider.ToString(), out p);
			IOS_ProfileErrorCodes.CheckAndThrowException(err);
			
			string json = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			SoomlaUtils.LogDebug(TAG, "Got json: " + json);
			
			JSONObject obj = new JSONObject(json);
			return new UserProfile(obj);
		}

		protected override void _storeUserProfile(UserProfile userProfile, bool notify) {
			soomlaProfile_SetStoredUserProfile(userProfile.toJSONObject().ToString(), notify);
		}

		// event pushing back to native (when using FB Unity SDK)
		protected override void _pushEventLoginStarted(Provider provider) {
			soomlaProfile_PushEventLoginStarted(provider.ToString());
		}
		protected override void _pushEventLoginFinished(string userProfileJson) { 
			soomlaProfile_PushEventLoginFinished(userProfileJson);
		}
		protected override void _pushEventLoginFailed(Provider provider, string message) {
			soomlaProfile_PushEventLoginFailed(provider.ToString(), message);
		}
		protected override void _pushEventLoginCancelled(Provider provider) { 
			soomlaProfile_PushEventLoginCancelled(provider.ToString());
		}
		protected override void _pushEventLogoutStarted(Provider provider) { 
			soomlaProfile_PushEventLogoutStarted(provider.ToString());
		}
		protected override void _pushEventLogoutFinished(Provider provider) { 
			soomlaProfile_PushEventLogoutFinished(provider.ToString());
		}
		protected override void _pushEventLogoutFailed(Provider provider, string message) {
			soomlaProfile_PushEventLogoutFailed(provider.ToString(), message);
		}
		protected override void _pushEventSocialActionStarted(Provider provider, SocialActionType actionType) { 
			soomlaProfile_PushEventSocialActionStarted(provider.ToString(), actionType.ToString());
		}
		protected override void _pushEventSocialActionFinished(Provider provider, SocialActionType actionType) {
			soomlaProfile_PushEventSocialActionFinished(provider.ToString(), actionType.ToString());
		}
		protected override void _pushEventSocialActionCancelled(Provider provider, SocialActionType actionType) {
			soomlaProfile_PushEventSocialActionCancelled(provider.ToString(), actionType.ToString());
		}
		protected override void _pushEventSocialActionFailed(Provider provider, SocialActionType actionType, string message) { 
			soomlaProfile_PushEventSocialActionFailed(provider.ToString(), actionType.ToString(), message);
		}

		private string GetRewardJsonStr(Reward reward) {
			string rewardJson = null;
			if(reward != null) {
				rewardJson = reward.toJSONObject().ToString();
			}

			return rewardJson;
		}

		#endif
	}
}
