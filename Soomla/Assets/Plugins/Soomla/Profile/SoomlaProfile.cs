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
		/// Initializes the SOOMLA Profile SDK.
		/// </summary>
		public static void Initialize() {
			instance._initialize();
		}



		public static void UpdateStatus(Provider provider, string status, Reward reward) {
			instance._updateStatus(provider, status, reward);
		}

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
//		public static void UploadImage(Provider provider, string message, string filePath,
//		                               Reward reward) {
//			instance._uploadImage(provider, message, filePath, reward);
//		}

		public static void GetContacts(Provider provider, Reward reward) {
			instance._getContacts(provider, reward);
		}

		public static UserProfile GetStoredUserProfile(Provider provider) {
			return instance._getStoredUserProfile(provider);
		}

		public static void Logout(Provider provider) {
			instance._logout(provider);
		}

		public static void Login(Provider provider, Reward reward) {
			instance._login(provider, reward);
		}


		protected virtual void _initialize() { }

		protected virtual void _updateStatus(Provider provider, string status, Reward reward) { }

		protected virtual void _updateStory(Provider provider, string message, string name, 
		                                    string caption, string description, string link,
		                                    string pictureUrl, Reward reward) { }

//		protected virtual void _uploadImage(Provider provider, string message, string filename,
//		                                    byte[] imageBytes, int quality, Reward reward) { }
//
//		protected virtual void _uploadImage(Provider provider, string message, string filePath,
//		                                    Reward reward) { }

		protected virtual void _getContacts(Provider provider, Reward reward) { }

		protected virtual UserProfile _getStoredUserProfile(Provider provider) { return null; }

		protected virtual void _logout(Provider provider) { }

		protected virtual void _login(Provider provider, Reward reward) { }
	}
}

