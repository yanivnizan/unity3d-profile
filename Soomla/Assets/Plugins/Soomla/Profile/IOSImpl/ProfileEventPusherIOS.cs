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

	public class ProfileEventPusherIOS : Soomla.Profile.ProfileEvents.ProfileEventPusher {
#if UNITY_IOS && !UNITY_EDITOR

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


		// event pushing back to native (when using FB Unity SDK)
		protected override void _pushEventLoginStarted(Provider provider, string payload) {
			soomlaProfile_PushEventLoginStarted(provider.ToString());
		}
		protected override void _pushEventLoginFinished(UserProfile userProfile, string payload) { 
			soomlaProfile_PushEventLoginFinished(userProfile.toJSONObject().print());
		}
		protected override void _pushEventLoginFailed(Provider provider, string message, string payload) {
			soomlaProfile_PushEventLoginFailed(provider.ToString(), message);
		}
		protected override void _pushEventLoginCancelled(Provider provider, string payload) { 
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
		protected override void _pushEventSocialActionStarted(Provider provider, SocialActionType actionType, string payload) { 
			soomlaProfile_PushEventSocialActionStarted(provider.ToString(), actionType.ToString());
		}
		protected override void _pushEventSocialActionFinished(Provider provider, SocialActionType actionType, string payload) {
			soomlaProfile_PushEventSocialActionFinished(provider.ToString(), actionType.ToString());
		}
		protected override void _pushEventSocialActionCancelled(Provider provider, SocialActionType actionType, string payload) {
			soomlaProfile_PushEventSocialActionCancelled(provider.ToString(), actionType.ToString());
		}
		protected override void _pushEventSocialActionFailed(Provider provider, SocialActionType actionType, string message, string payload) { 
			soomlaProfile_PushEventSocialActionFailed(provider.ToString(), actionType.ToString(), message);
		}

#endif
	}
}
