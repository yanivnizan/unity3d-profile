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
		public static void Initialize() {
			instance._initialize();
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
		public static void UploadImage(Provider provider, string message, string filePath, Reward reward) {
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


		protected virtual void _initialize() { }

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

		protected virtual UserProfile _getStoredUserProfile(Provider provider) { return null; }

		/// <summary> Class Members </summary>
		
		protected const string TAG = "SOOMLA SoomlaProfile";
	}
}

