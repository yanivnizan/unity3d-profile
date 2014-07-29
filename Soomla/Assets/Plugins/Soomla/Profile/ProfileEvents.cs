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

		/// <summary>
		/// Initializes game state before the game starts.
		/// </summary>
		void Awake(){
			if(instance == null){ 	// making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
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
		public void onGetContactsFailedEvent(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGetContactsFailedEvent:" + message);

			JSONObject resObj = new JSONObject(message);
			String providerStr = resObj ["provider"].str;
			String errMsg = resObj["errMsg"].str;
			Provider provider = Provider.fromString (providerStr);

			ProfileEvents.OnGetContactsFailedEvent(provider, errMsg);
		}

		/// <summary>
		/// Will be called when contacts were fetched from the social provider.
		/// </summary>
		/// <param name="message">Will contain provider as string and 
		/// an array of UserProfiles as JSON.</param>
		public void onGetContactsFinishedEvent(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGetContactsFinishedEvent:" + message);

			JSONObject resObj = new JSONObject(message);
			String providerStr = resObj ["provider"].str;
			Provider provider = Provider.fromString (providerStr);
			JSONObject contactsObj = resObj.GetField ("contacts");
			List<UserProfile> contacts = new List<UserProfile>();
			foreach (JSONObject upObj in contactsObj.list) {
				contacts.Add(new UserProfile(upObj));
			}
			ProfileEvents.OnGetContactsFinishedEvent(provider, contacts);
		}

		/// <summary>
		/// Will be called when contacts fetching process has started.
		/// </summary>
		/// <param name="message">provider as string</param>
		public void onGetContactsStartedEvent(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGetContactsStartedEvent");
			ProfileEvents.OnGetContactsStartedEvent(Provider.fromString(message));
		}

		/// <summary>
		/// Will be called when a reward was given to the user.
		/// </summary>
		/// <param name="message">Will contain a JSON representation of a <c>Reward</c> and a flag saying if it's a Badge or not.</param>
		public void onRewardGivenEvent(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onRewardGivenEvent:" + message);

			JSONObject resObj = new JSONObject(message);
			JSONObject rewardObj = resObj["reward"];
			Reward reward = Reward.fromJSONObject(rewardObj);
			bool isBadge = resObj["isBadge"].b;
			ProfileEvents.OnRewardGivenEvent(reward, isBadge);
		}


		public delegate void Action();

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

		public static Action<Provider, string> OnGetContactsFailedEvent = delegate {};
		
		public static Action<Provider, List<UserProfile>> OnGetContactsFinishedEvent = delegate {};
		
		public static Action<Provider> OnGetContactsStartedEvent = delegate {};

		public static Action<Reward, bool> OnRewardGivenEvent = delegate {};
	}
}
