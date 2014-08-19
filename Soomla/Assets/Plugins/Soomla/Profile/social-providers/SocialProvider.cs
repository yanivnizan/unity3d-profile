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

using System;
using UnityEngine;
using System.Collections.Generic;

namespace Soomla.Profile
{


	public abstract class SocialProvider
	{
		public delegate void LoginSuccess(UserProfile userProfile);
		public delegate void LoginFailed(string message);
		public delegate void LoginCancelled();
		public delegate void LogoutFailed(string message);
		public delegate void LogoutSuccess();
//		public delegate void FeedFailed(string message);
//		public delegate void FeedSuccess(List<String> feeds);
		public delegate void ContactsFailed(string message);
		public delegate void ContactsSuccess(List<UserProfile> userProfiles);
		public delegate void UserProfileFailed(string message);
		public delegate void UserProfileSuccess(UserProfile userProfile);
		public delegate void SocialActionFailed(string message);
		public delegate void SocialActionSuccess();
		public delegate void SocialActionCancel();



		public abstract void UpdateStatus(string status, SocialActionSuccess success, SocialActionFailed fail);
		public abstract void UpdateStory(string message, string name, string caption, 
		                                 string link, string pictureUrl, SocialActionSuccess success, SocialActionFailed fail, SocialActionCancel cancel);
		public abstract void UploadImage(Texture2D tex2D, string fileName, string message, SocialActionSuccess success, SocialActionFailed fail, SocialActionCancel cancel);
		public abstract void GetContacts(ContactsSuccess success, ContactsFailed fail);
//		public abstract void GetFeed(FeedSuccess success, FeedFailed fail);
		public abstract void Logout(LogoutSuccess success, LogoutFailed fail);
		public abstract void Login(LoginSuccess success, LoginFailed fail, LoginCancelled cancel);
		public abstract bool IsLoggedIn();
	}
}

