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
/// limitations under the License.using System;

using UnityEngine;
using System.Text.RegularExpressions;

namespace Soomla.Profile
{
	/// <summary>
	/// This is the main class controlling the whole SOOMLA Profile module.
	/// Use this class to perform various social and authentication operations on users.
	/// The Profile module will work with the social and authentication plugins you provide and
	/// define in AndroidManifest.xml or your iOS project's plist.
	/// </summary>
	public class SoomlaProfile
	{
		static SoomlaProfile _instance = null;
		static SoomlaProfile instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new SoomlaProfileAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new SoomlaProfileIOS();
					#else
					_instance = new SoomlaProfile();
					#endif
				}
				return _instance;
			}
		}

		/// <summary>
		/// Initializes the SOOMLA Profile Module.
		/// </summary>
		public static void Initialize(bool usingUnityProvider) {
			instance._initialize(usingUnityProvider);
		}

		/// <summary>
		/// Will post a status to the user's social page.
		/// 
		/// This operation requires a successful login.
		/// </summary>
		/// <param name="provider">The <c>Provider</c> the given status should be posted to.</param>
		/// <param name="status">The actual status text.</param>
		/// <param name="reward">A <c>Reward</c> to give to the user after a successful posting.</param>
		public static void UpdateStatus(Provider provider, string status, Reward reward) {
			instance._updateStatus(provider, status, reward);
		}

		/// <summary>
		/// Will post a full story to the user's social page.
		/// A story contains: title, description, image and more.
		/// 
		/// This operation requires a successful login.
		/// </summary>
		/// <param name="provider">The <c>Provider</c> the given story should be posted to.</param>
		/// <param name="message">A message that will be shown along with the story.</param>
		/// <param name="name">The name (title) of the story.</param>
		/// <param name="caption">A caption.</param>
		/// <param name="description">The actual story text.</param>
		/// <param name="link">A link to a web page.</param>
		/// <param name="pictureUrl">A link to an image on the web.</param>
		/// <param name="reward">A <c>Reward</c> to give to the user after a successful posting.</param>
		public static void UpdateStory(Provider provider, string message, string name, 
		                               string caption, string description, string link,
		                               string pictureUrl, Reward reward) {
			instance._updateStory(provider, message, name, caption, description, link, pictureUrl, reward);
		}

//		public static void UploadImage(Provider provider, string message, string filename,
//		                               byte[] imageBytes, int quality, Reward reward) {
//			instance._uploadImage(provider, message, filename, imageBytes, quality, reward);
//		}
//
		public static void UploadImage(Provider provider, string message, string filePath,
		                               Reward reward) {
			instance._uploadImage(provider, message, filePath, reward);
		}

		/// <summary>
		/// Will fetch UserProfiles of contacts of the current user.
		/// 
		/// This operation requires a successful login.
		/// </summary>
		/// <param name="provider">The <c>Provider</c> we should try to fetch contacts from.</param>
		/// <param name="reward">A <c>Reward</c> to give to the user after a successful fetching.</param>
		public static void GetContacts(Provider provider, Reward reward) {
			instance._getContacts(provider, reward);
		}

		/// <summary>
		///  Will fetch posts from user feed 
		///
		/// </summary>
		/// <param name="provider">Provider.</param>
		/// <param name="reward">Reward.</param>
		public static void GetFeed(Provider provider, Reward reward) {
			instance._getFeed(provider, reward);
		}

		/// <summary>
		/// This will fetch the UserProfile that is saved for the given provider.
		/// UserProfiles are automatically saved in the local storage for a provider after a successful login.
		/// </summary>
		/// <returns>The stored user profile.</returns>
		/// <param name="provider">The provider you need to fetch UserProfile for.</param>
		public static UserProfile GetStoredUserProfile(Provider provider) {
			return instance._getStoredUserProfile(provider);
		}

		/// <summary>
		/// Stores the user profile.
		/// The UserProfile should contain the provider internally
		/// </summary>
		/// <param name="userProfile">User profile.</param>
		/// <param name="notify">If set to <c>true</c> notify.</param>
		public static void StoreUserProfile (UserProfile userProfile, bool notify = false) {
			instance._storeUserProfile (userProfile, notify);
		}

		/// <summary>
		/// Will log you out from the given provider.
		/// </summary>
		/// <param name="provider">The provider to log out from.</param>
		public static void Logout(Provider provider) {
			instance._logout(provider);
		}

		/// <summary>
		/// Will log you in to the given provider.
		/// </summary>
		/// <param name="provider">The provider to log in to.</param>
		/// <param name="reward">Give your users a reward for logging in.</param>
		public static void Login(Provider provider, Reward reward) {
			instance._login(provider, reward);
		}


		// push events
		public static void PushEventLoginStarted(Provider provider) { 
			instance._pushEventLoginStarted(provider);
		}
		public static void PushEventLoginFinished(string userProfileJson) {
			instance._pushEventLoginFinished(userProfileJson);
		}
		public static void PushEventLoginFailed(Provider provider, string message) { 
			instance._pushEventLoginFailed(provider, message); 
		}
		public static void PushEventLoginCancelled(Provider provider) { 
			instance._pushEventLoginCancelled(provider);
		}
		public static void PushEventLogoutStarted(Provider provider) {
			instance._pushEventLogoutStarted(provider); 
		}
		public static void PushEventLogoutFinished(Provider provider) {
			instance._pushEventLogoutFinished(provider);
		}
		public static void PushEventLogoutFailed(Provider provider, string message) {
			instance._pushEventLogoutFailed(provider, message); 
		}
		public static void PushEventSocialActionStarted(Provider provider, SocialActionType actionType) { 
			instance._pushEventSocialActionStarted(provider, actionType);
		}
		public static void PushEventSocialActionFinished(Provider provider, SocialActionType actionType) { 
			instance._pushEventSocialActionFinished(provider, actionType);
		}
		public static void PushEventSocialActionCancelled(Provider provider, SocialActionType actionType) { 
			instance._pushEventSocialActionCancelled(provider, actionType);
		}
		public static void PushEventSocialActionFailed(Provider provider, SocialActionType actionType, string message) {
			instance._pushEventSocialActionFailed (provider, actionType, message);
		}
			

		protected virtual void _initialize(bool usingUnityProvider) { }

		protected virtual void _login(Provider provider, Reward reward) { }
		
		protected virtual void _logout(Provider provider) { }

		protected virtual void _updateStatus(Provider provider, string status, Reward reward) { }

		protected virtual void _updateStory(Provider provider, string message, string name, 
		                                    string caption, string description, string link,
		                                    string pictureUrl, Reward reward) { }

//		protected virtual void _uploadImage(Provider provider, string message, string filename,
//		                                    byte[] imageBytes, int quality, Reward reward) { }
//
		protected virtual void _uploadImage(Provider provider, string message, string filePath,
		                                    Reward reward) { }

		protected virtual void _getContacts(Provider provider, Reward reward) { }

		protected virtual void _getFeed(Provider provider, Reward reward) { }

		// event pushing back to native (when using FB Unity SDK)
		protected virtual void _pushEventLoginStarted(Provider provider) { }
		protected virtual void _pushEventLoginFinished(string userProfileJson) { }
		protected virtual void _pushEventLoginFailed(Provider provider, string message) { }
		protected virtual void _pushEventLoginCancelled(Provider provider) { }
		protected virtual void _pushEventLogoutStarted(Provider provider) { }
		protected virtual void _pushEventLogoutFinished(Provider provider) { }
		protected virtual void _pushEventLogoutFailed(Provider provider, string message) { }
		protected virtual void _pushEventSocialActionStarted(Provider provider, SocialActionType actionType) { }
		protected virtual void _pushEventSocialActionFinished(Provider provider, SocialActionType actionType) { }
		protected virtual void _pushEventSocialActionCancelled(Provider provider, SocialActionType actionType) { }
		protected virtual void _pushEventSocialActionFailed(Provider provider, SocialActionType actionType, string message) { }


		protected virtual UserProfile _getStoredUserProfile(Provider provider) { 
#if UNITY_EDITOR
			string key = keyUserProfile(provider);
			string value = PlayerPrefs.GetString (key);
			return new UserProfile (new JSONObject (value));
#endif
			return null;
		}

		protected virtual void _storeUserProfile(UserProfile userProfile, bool notify) { 
#if UNITY_EDITOR
			string key = keyUserProfile(userProfile.Provider);
			string val = userProfile.toJSONObject().ToString();
			SoomlaUtils.LogDebug(TAG, "key/val:" + key + "/" + val);
			PlayerPrefs.SetString(key, val);
			
			if (notify) {
				ProfileEvents.OnUserProfileUpdated(userProfile);
			}
#endif
		}

		/** keys **/
#if UNITY_EDITOR
		private const string DB_KEY_PREFIX = "soomla.profile.";

		private static string keyUserProfile(Provider provider) {
			return DB_KEY_PREFIX + "userprofile." + provider.ToString();
		}
#endif

		/// <summary> Class Members </summary>

		protected const string TAG = "SOOMLA SoomlaProfile";
	}
}

