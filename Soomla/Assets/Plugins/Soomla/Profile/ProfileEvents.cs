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

		public delegate void Action();

		public static Action OnSoomlaProfileInitialized = delegate {};

		public static Action<Provider, string> OnLoginCancelled = delegate {};

		public static Action<UserProfile> OnUserProfileUpdated = delegate {};

		public static Action<Provider, string, string> OnLoginFailed = delegate {};

		public static Action<UserProfile, string> OnLoginFinished = delegate {};

		public static Action<Provider, string> OnLoginStarted = delegate {};

		public static Action<Provider, string> OnLogoutFailed = delegate {};
		
		public static Action<Provider> OnLogoutFinished = delegate {}; 

		public static Action<Provider> OnLogoutStarted = delegate {};

		public static Action<Provider, SocialActionType, string, string> OnSocialActionFailed = delegate {};

		public static Action<Provider, SocialActionType, string> OnSocialActionFinished = delegate {};

		public static Action<Provider, SocialActionType, string> OnSocialActionStarted = delegate {};

		public static Action<Provider, SocialActionType, string> OnSocialActionCancelled = delegate {};

		public static Action<Provider, string, string> OnGetContactsFailed = delegate {};
		
		public static Action<Provider, List<UserProfile>, string> OnGetContactsFinished = delegate {};
		
		public static Action<Provider, string> OnGetContactsStarted = delegate {};

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
			protected virtual void _pushEventLoginStarted(Provider provider, string payload) {}
			protected virtual void _pushEventLoginFinished(UserProfile userProfileJson, string payload){}
			protected virtual void _pushEventLoginFailed(Provider provider, string message, string payload){}
			protected virtual void _pushEventLoginCancelled(Provider provider, string payload){}
			protected virtual void _pushEventLogoutStarted(Provider provider){}
			protected virtual void _pushEventLogoutFinished(Provider provider){}
			protected virtual void _pushEventLogoutFailed(Provider provider, string message){}
			protected virtual void _pushEventSocialActionStarted(Provider provider, SocialActionType actionType, string payload){}
			protected virtual void _pushEventSocialActionFinished(Provider provider, SocialActionType actionType, string payload){}
			protected virtual void _pushEventSocialActionCancelled(Provider provider, SocialActionType actionType, string payload){}
			protected virtual void _pushEventSocialActionFailed(Provider provider, SocialActionType actionType, string message, string payload){}
		}
	}
}
