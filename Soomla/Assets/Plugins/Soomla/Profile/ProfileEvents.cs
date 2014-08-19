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
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Soomla.Profile {

	/// <summary>
	/// This class provides functions for event handling.
	/// </summary>
	public class ProfileEvents : MonoBehaviour {

		private const string TAG = "SOOMLA ProfileEvents";

		private static ProfileEvents instance = null;
		#pragma warning disable 414
		private static ProfileEventPusher pep = null;
		#pragma warning restore 414

		/// <summary>
		/// Initializes game state before the game starts.
		/// </summary>
		void Awake(){
			if(instance == null){ 	// making sure we only initialize one instance.
				SoomlaUtils.LogDebug(TAG, "Initializing ProfileEvents (Awake)");

				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);

				// now we initialize the event pusher
				#if UNITY_ANDROID && !UNITY_EDITOR
				pep = new ProfileEventPusherAndroid();
				#elif UNITY_IOS && !UNITY_EDITOR
				pep = new ProfileEventPusherIOS();
				#endif

			} else {				// Destroying unused instances.
				GameObject.Destroy(this.gameObject);
			}
		}

		/// <summary>
		/// Will be called when login is cancelled.
		/// </summary>
		/// <param name="message">Will contain the provider as string</param>
		public void onLoginCancelled(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLoginCancelledEvent");

			ProfileEvents.OnLoginCancelled(Provider.fromString(message));
		}

		/// <summary>
		/// Will be called when a user profile is updated.
		/// </summary>
		/// <param name="message">will contain a JSON representation of the UserProfile.</param>
		/// the provider is found in the UserProfile
		public void onUserProfileUpdated(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onUserProfileUpdated");

			JSONObject upJSON = new JSONObject(message);
			UserProfile up = new UserProfile(upJSON);
			ProfileEvents.OnUserProfileUpdated(up);
		}

		/// <summary>
		/// Will be called when login fails.
		/// </summary>
		/// <param name="message">Will contain the failure message.</param>
		public void onLoginFailed(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLoginFailed:" + message);

			JSONObject resObj = new JSONObject(message);
			String providerStr = resObj ["provider"].str;
			String errMsg = resObj["errMsg"].str;
			Provider provider = Provider.fromString (providerStr);

			ProfileEvents.OnLoginFailed(provider, errMsg);
		}

		/// <summary>
		/// Will be called when login finished.
		/// </summary>
		/// <param name="message">Will contain a JSON representation of the UserProfile.</param>
		/// the provider is found in the UserProfile
		public void onLoginFinished(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLoginFinished:" + message);

			JSONObject upJSON = new JSONObject(message);
			UserProfile up = new UserProfile(upJSON);
			ProfileEvents.OnLoginFinished(up);
		}

		/// <summary>
		/// Will be called when login started.
		/// </summary>
		/// <param name="message">Will contain the provider as string.</param>
		public void onLoginStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLoginStarted:" + message);

			ProfileEvents.OnLoginStarted(Provider.fromString(message));
		}

		/// <summary>
		/// Will be called when logout fails.
		/// </summary>
		/// <param name="message">Will contain provider as string and
		 /// the failure message.</param>
		public void onLogoutFailed(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLogoutFailed:" + message);
		
			JSONObject resObj = new JSONObject(message);
			String providerStr = resObj ["provider"].str;
			String errMsg = resObj["errMsg"].str;
			Provider provider = Provider.fromString (providerStr);

			ProfileEvents.OnLogoutFailed(provider, errMsg);
		}

		/// <summary>
		/// Will be called when logout finished.
		/// </summary>
		/// <param name="message">Will contain a JSON representation of the UserProfile.</param>
		public void onLogoutFinished(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLogoutFinished:" + message);

			ProfileEvents.OnLogoutFinished(Provider.fromString (message));
		}

		/// <summary>
		/// Will be called when logout started.
		/// </summary>
		/// <param name="message">Will contain the provider as string.</param>
		public void onLogoutStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLogoutStarted:" + message);

			ProfileEvents.OnLogoutStarted(Provider.fromString(message));
		}

		/// <summary>
		/// Will be called when a social action fails.
		/// </summary>
		/// <param name="message">Will contain the social action type as string and the error message.</param>
		public void onSocialActionFailed(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onSocialActionFailed:" + message);

			JSONObject resObj = new JSONObject(message);
			string providerStr = resObj["provider"].str;
			string satStr = resObj["socialActionType"].str;
			string errMsg = resObj["errMsg"].str;

			Provider provider = Provider.fromString (providerStr);
			SocialActionType sat = SocialActionType.fromString (satStr);
			ProfileEvents.OnSocialActionFailed(provider, sat, errMsg);
		}

		/// <summary>
		/// Will be called when a social action is cancelled.
		/// </summary>
		/// <param name="message">Will contain the provider as string and 
		/// a string represenatation of the <c>SocialActionType</c>.</param>
		public void onSocialActionCancelled(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onSocialActionCancelled:" + message);

			JSONObject resObj = new JSONObject(message);
			string providerStr = resObj["provider"].str;
			string satStr = resObj["socialActionType"].str;

			Provider provider = Provider.fromString (providerStr);
			SocialActionType sat = SocialActionType.fromString (satStr);
			ProfileEvents.OnSocialActionCancelled (provider, sat);
		}

		/// <summary>
		/// Will be called when a social action finishes.
		/// </summary>
		/// <param name="message">Will contain provider as string and 
		/// a string represenatation of th<c>SocialActionType</c>.</param>
		public void onSocialActionFinished(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onSocialActionFinished:" + message);
			
			JSONObject resObj = new JSONObject(message);
			string providerStr = resObj["provider"].str;
			string satStr = resObj["socialActionType"].str;
			
			Provider provider = Provider.fromString (providerStr);
			SocialActionType sat = SocialActionType.fromString (satStr);

			ProfileEvents.OnSocialActionFinished(provider, sat);
		}

		/// <summary>
		/// Will be called when a social action started.
		/// </summary>
		/// <param name="message">Will contain provider as string and
		 /// a string represenatation of th<c>SocialActionType</c>.</param>
		public void onSocialActionStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onSocialActionStarted:" + message);

			JSONObject resObj = new JSONObject(message);
			string providerStr = resObj["provider"].str;
			string satStr = resObj["socialActionType"].str;
			
			Provider provider = Provider.fromString (providerStr);
			SocialActionType sat = SocialActionType.fromString (satStr);

			ProfileEvents.OnSocialActionStarted(provider, sat);
		}

		/// <summary>
		/// Will be called when contacts were failed to be fetched.
		/// </summary>
		/// <param name="message">Will contain the provider as string and failure message.</param>
		public void onGetContactsFailed(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGetContactsFailed:" + message);

			JSONObject resObj = new JSONObject(message);
			String providerStr = resObj ["provider"].str;
			String errMsg = resObj["errMsg"].str;
			Provider provider = Provider.fromString (providerStr);

			ProfileEvents.OnGetContactsFailed(provider, errMsg);
		}

		/// <summary>
		/// Will be called when contacts were fetched from the social provider.
		/// </summary>
		/// <param name="message">Will contain provider as string and 
		/// an array of UserProfiles as JSON.</param>
		public void onGetContactsFinished(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGetContactsFinished:" + message);

			JSONObject resObj = new JSONObject(message);
			String providerStr = resObj ["provider"].str;
			Provider provider = Provider.fromString (providerStr);
			JSONObject contactsObj = resObj.GetField ("contacts");
			List<UserProfile> contacts = new List<UserProfile>();
			foreach (JSONObject upObj in contactsObj.list) {
				contacts.Add(new UserProfile(upObj));
			}
			ProfileEvents.OnGetContactsFinished(provider, contacts);
		}

		/// <summary>
		/// Will be called when contacts fetching process has started.
		/// </summary>
		/// <param name="message">provider as string</param>
		public void onGetContactsStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGetContactsStarted");
			ProfileEvents.OnGetContactsStarted(Provider.fromString(message));
		}

		/// <summary>
		/// Will be called when feed was failed to be fetched.
		/// </summary>
		/// <param name="message">Will contain the provider as string and failure message.</param>
		public void onGetFeedFailed(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGetFeedFailed:" + message);

			JSONObject resObj = new JSONObject(message);
			String providerStr = resObj ["provider"].str;
			String errMsg = resObj["errMsg"].str;
			Provider provider = Provider.fromString (providerStr);

			ProfileEvents.OnGetFeedFailed(provider, errMsg);
		}
		
		/// <summary>
		/// Will be called when feed was fetched from the social provider.
		/// </summary>
		/// <param name="message">Will contain an array of posts as string (later JSON).</param>
		public void onGetFeedFinished(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGetFeedFinished:" + message);

			JSONObject resObj = new JSONObject(message);
			String providerStr = resObj ["provider"].str;
			Provider provider = Provider.fromString (providerStr);
			JSONObject postsObj = resObj.GetField ("posts");
			List<string> posts = new List<string>();
			foreach (JSONObject postObj in postsObj.list) {
				posts.Add(postObj.str);
			}

			ProfileEvents.OnGetFeedFinished(provider, posts);
		}
		
		/// <summary>
		/// Will be called when feed fetching process has started.
		/// </summary>
		/// <param name="message">provider as string</param>
		public void onGetFeedStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGetFeedStarted");
			ProfileEvents.OnGetFeedStarted(Provider.fromString(message));
		}


		public delegate void Action();

		public static Action OnSoomlaProfileInitialized = delegate {};

		public static Action<Provider> OnLoginCancelled = delegate {};

		public static Action<UserProfile> OnUserProfileUpdated = delegate {};

		public static Action<Provider, string> OnLoginFailed = delegate {};

		public static Action<UserProfile> OnLoginFinished = delegate {};

		public static Action<Provider> OnLoginStarted = delegate {};

		public static Action<Provider, string> OnLogoutFailed = delegate {};
		
		public static Action<Provider> OnLogoutFinished = delegate {}; 

		public static Action<Provider> OnLogoutStarted = delegate {};

		public static Action<Provider, SocialActionType, string> OnSocialActionFailed = delegate {};

		public static Action<Provider, SocialActionType> OnSocialActionFinished = delegate {};

		public static Action<Provider, SocialActionType> OnSocialActionStarted = delegate {};

		public static Action<Provider, SocialActionType> OnSocialActionCancelled = delegate {};

		public static Action<Provider, string> OnGetContactsFailed = delegate {};
		
		public static Action<Provider, List<UserProfile>> OnGetContactsFinished = delegate {};
		
		public static Action<Provider> OnGetContactsStarted = delegate {};

		public static Action<Provider, string> OnGetFeedFailed = delegate {};
		
		public static Action<Provider, List<string>> OnGetFeedFinished = delegate {};
		
		public static Action<Provider> OnGetFeedStarted = delegate {};


		public class ProfileEventPusher {
			public ProfileEventPusher() {
				ProfileEvents.OnLoginCancelled += _pushEventLoginStarted;
				ProfileEvents.OnLoginFailed += _pushEventLoginFailed;
				ProfileEvents.OnLoginFinished += _pushEventLoginFinished;
				ProfileEvents.OnLoginStarted += _pushEventLoginStarted;
				ProfileEvents.OnLogoutFailed += _pushEventLogoutFailed;
				ProfileEvents.OnLogoutFinished += _pushEventLogoutFinished;
				ProfileEvents.OnLogoutStarted += _pushEventLogoutStarted;
				ProfileEvents.OnSocialActionCancelled += _pushEventSocialActionCancelled;
				ProfileEvents.OnSocialActionFailed += _pushEventSocialActionFailed;
				ProfileEvents.OnSocialActionFinished += _pushEventSocialActionFinished;
				ProfileEvents.OnSocialActionStarted += _pushEventSocialActionStarted;
			}

			// event pushing back to native (when using FB Unity SDK)
			protected virtual void _pushEventLoginStarted(Provider provider) {}
			protected virtual void _pushEventLoginFinished(UserProfile userProfileJson){}
			protected virtual void _pushEventLoginFailed(Provider provider, string message){}
			protected virtual void _pushEventLoginCancelled(Provider provider){}
			protected virtual void _pushEventLogoutStarted(Provider provider){}
			protected virtual void _pushEventLogoutFinished(Provider provider){}
			protected virtual void _pushEventLogoutFailed(Provider provider, string message){}
			protected virtual void _pushEventSocialActionStarted(Provider provider, SocialActionType actionType){}
			protected virtual void _pushEventSocialActionFinished(Provider provider, SocialActionType actionType){}
			protected virtual void _pushEventSocialActionCancelled(Provider provider, SocialActionType actionType){}
			protected virtual void _pushEventSocialActionFailed(Provider provider, SocialActionType actionType, string message){}
		}
	}
}
