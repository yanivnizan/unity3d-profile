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

		/// </summary>
		/// <param name="message">Not used here.</param>
		public void onLoginCancelled(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onLoginCancelledEvent");

			ProfileEvents.OnLoginCancelled();
		}

		/// <summary>

		/// </summary>
		public void onUserProfileUpdated(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onUserProfileUpdated");

			JSONObject upJSON = new JSONObject(message);
			UserProfile up = new UserProfile(upJSON);
			ProfileEvents.OnUserProfileUpdated(up);
		}

		/// <summary>

		/// </summary>
		public void onLoginFailed(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onLoginFailed:" + message);

			ProfileEvents.OnLoginFailed(message);
		}

		public void onLoginFinished(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onLoginFinished:" + message);

			JSONObject upJSON = new JSONObject(message);
			UserProfile up = new UserProfile(upJSON);
			ProfileEvents.OnLoginFinished(up);
		}

		public void onLoginStarted(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onLoginStarted:" + message);

			ProfileEvents.OnLoginStarted(Provider.fromString(message));
		}

		public void onLogoutFailed(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onLogoutFailed:" + message);
			
			ProfileEvents.OnLogoutFailed(message);
		}
		
		public void onLogoutFinished(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onLogoutFinished:" + message);
			
			JSONObject upJSON = new JSONObject(message);
			UserProfile up = new UserProfile(upJSON);
			ProfileEvents.OnLogoutFinished(up);
		}
		
		public void onLogoutStarted(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onLogoutStarted:" + message);

			ProfileEvents.OnLogoutStarted(Provider.fromString(message));
		}

		public void onSocialActionFailed(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onSocialActionFailed:" + message);

			JSONObject jsonObj = new JSONObject(message);
			string satStr = jsonObj["socialActionType"].str;
			string errMsg = jsonObj["errMsg"].str;

			SocialActionType sat = (SocialActionType)Enum.Parse(typeof(SocialActionType), satStr.ToUpper());
			ProfileEvents.OnSocialActionFailed(sat, errMsg);
		}

		public void onSocialActionFinished(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onSocialActionFinished:" + message);
			
			SocialActionType sat = (SocialActionType)Enum.Parse(typeof(SocialActionType), message.ToUpper());
			ProfileEvents.OnSocialActionFinished(sat);
		}

		public void onSocialActionStarted(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onSocialActionStarted:" + message);
			
			SocialActionType sat = (SocialActionType)Enum.Parse(typeof(SocialActionType), message.ToUpper());
			ProfileEvents.OnSocialActionStarted(sat);
		}

		public void onGetContactsFailedEvent(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onGetContactsFailedEvent:" + message);

			ProfileEvents.OnGetContactsFailedEvent(message);
		}
		
		public void onGetContactsFinishedEvent(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onGetContactsFinishedEvent:" + message);

			JSONObject contactsObj = new JSONObject(message);
			List<UserProfile> contacts = new List<UserProfile>();
			foreach (JSONObject upObj in contactsObj.list) {
				contacts.Add(new UserProfile(upObj));
			}
			ProfileEvents.OnGetContactsFinishedEvent(contacts);
		}
		
		public void onGetContactsStartedEvent(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onGetContactsStartedEvent");
			ProfileEvents.OnGetContactsStartedEvent();
		}

		public void onRewardGivenEvent(string message) {
			Utils.LogDebug(TAG, "SOOMLA/UNITY onRewardGivenEvent:" + message);

			JSONObject resObj = new JSONObject(message);
			JSONObject rewardObj = new JSONObject(resObj["reward"]);
			Reward reward = Reward.fromJSONObject(rewardObj);
			bool isBadge = resObj["isBadge"].b;
			ProfileEvents.OnRewardGivenEvent(reward, isBadge);
		}


		public delegate void Action();

		public static Action OnLoginCancelled = delegate {};

		public static Action<UserProfile> OnUserProfileUpdated = delegate {};

		public static Action<string> OnLoginFailed = delegate {};

		public static Action<UserProfile> OnLoginFinished = delegate {};

		public static Action<Provider> OnLoginStarted = delegate {};

		public static Action<string> OnLogoutFailed = delegate {};
		
		public static Action<UserProfile> OnLogoutFinished = delegate {};

		public static Action<Provider> OnLogoutStarted = delegate {};

		public static Action<SocialActionType, string> OnSocialActionFailed = delegate {};

		public static Action<SocialActionType> OnSocialActionFinished = delegate {};

		public static Action<SocialActionType> OnSocialActionStarted = delegate {};

		public static Action<string> OnGetContactsFailedEvent = delegate {};
		
		public static Action<List<UserProfile>> OnGetContactsFinishedEvent = delegate {};
		
		public static Action OnGetContactsStartedEvent = delegate {};

		public static Action<Reward, bool> OnRewardGivenEvent = delegate {};
	}
}
