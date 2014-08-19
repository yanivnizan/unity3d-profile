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
		private static extern void soomlaProfile_Initialize();
		[DllImport ("__Internal")]
		private static extern int soomlaProfile_GetStoredUserProfile(string provider, out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int soomlaProfile_SetStoredUserProfile(string userProfileJson, bool notify);

		[DllImport ("__Internal")]
		private static extern void soomlaProfile_OpenAppRatingPage();

		protected override void _initialize () {
			soomlaProfile_Initialize();
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

		protected override void _openAppRatingPage() {
			soomlaProfile_OpenAppRatingPage();
		}

		#endif
	}
}
