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
using Facebook.MiniJSON;
using System.Text.RegularExpressions;
using System.Linq;

namespace Soomla.Profile
{
	public class FBSocialProvider : SocialProvider
	{
		private static string TAG = "SOOMLA FBSocialProvider";

		public FBSocialProvider ()
		{
			FB.Init(OnInitComplete, OnHideUnity);
		}

		public override void UpdateStatus(string status, SocialActionSuccess success, SocialActionFailed fail) {
			var formData = new Dictionary<string, string>
			{
				{ "message", status }
			};
			FB.API ("/me/feed", Facebook.HttpMethod.POST, 
			        (FBResult result) => {

						if (result.Error != null) {
							SoomlaUtils.LogDebug(TAG, "UpdateStatusCallback[result.Error]:"+result.Error);
							fail(result.Error);
						}
						else {
							SoomlaUtils.LogDebug(TAG, "UpdateStatusCallback[result.Text]:"+result.Text);
							SoomlaUtils.LogDebug(TAG, "UpdateStatusCallback[result.Texture]:"+result.Texture);
							success();
						}
		
					}
					, formData);
		}
		
		public override void UpdateStory(string message, string name, string caption,
		                                 string link, string pictureUrl, SocialActionSuccess success, SocialActionFailed fail, SocialActionCancel cancel) {
			FB.Feed(
				link: link,
				linkName: name,
				linkCaption: caption,
				linkDescription: message,
				picture: pictureUrl,
				callback: (FBResult result) => {

						if (result.Error != null) {
							fail(result.Error);
						}
						else {
							SoomlaUtils.LogDebug(TAG, "FeedCallback[result.Text]:"+result.Text);
							SoomlaUtils.LogDebug(TAG, "FeedCallback[result.Texture]:"+result.Texture);
							var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;
							object obj = 0;
							if (responseObject.TryGetValue("cancelled", out obj)) {
								cancel();
							}
							else /*if (responseObject.TryGetValue ("id", out obj))*/ {
								success();
							}
						}
					
					}

				);
		}

		public override void UploadImage(Texture2D tex2D, string fileName, string message, SocialActionSuccess success, SocialActionFailed fail, SocialActionCancel cancel) {
			byte[] texBytes = tex2D.EncodeToPNG();
			
			var wwwForm = new WWWForm();
			wwwForm.AddBinaryData("image", texBytes, fileName);
			wwwForm.AddField("message", message);

			FB.API("/me/photos", Facebook.HttpMethod.POST, 
			       (FBResult result) => {
					
						if (result.Error != null) {
							SoomlaUtils.LogDebug(TAG, "UploadImageCallback[result.Error]: "+result.Error);
//							ProfileEvents.OnSocialActionFailed (Provider.FACEBOOK, SocialActionType.UPLOAD_IMAGE, result.Error);
							fail(result.Error);
					     }
						else {
							SoomlaUtils.LogDebug(TAG, "UploadImageCallback[result.Text]: "+result.Text);
							SoomlaUtils.LogDebug(TAG, "UploadImageCallback[result.Texture]: "+result.Texture);
							var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;
							object obj = 0;
							if (responseObject.TryGetValue("cancelled", out obj)) {
//								ProfileEvents.OnSocialActionCancelled(Provider.FACEBOOK, SocialActionType.UPLOAD_IMAGE);
								cancel();
							}
							else /*if (responseObject.TryGetValue ("id", out obj))*/ {
//								ProfileEvents.OnSocialActionFinished(Provider.FACEBOOK, SocialActionType.UPLOAD_IMAGE);
								success();
							}
						}

					}, wwwForm);
		}

		public override void GetContacts(ContactsSuccess success, ContactsFailed fail) {
			FB.API ("/me/friends?fields=id,name,picture,email,first_name,last_name",
			        Facebook.HttpMethod.GET,
			        (FBResult result) => {
						if (result.Error != null) {
							SoomlaUtils.LogDebug(TAG, "GetContactsCallback[result.Error]: "+result.Error);
//							ProfileEvents.OnSocialActionFailed (Provider.FACEBOOK, SocialActionType.GET_CONTACTS, result.Error);
							fail(result.Error);
					     }
						else {
							SoomlaUtils.LogDebug(TAG, "GetContactsCallback[result.Text]: "+result.Text);
							SoomlaUtils.LogDebug(TAG, "GetContactsCallback[result.Texture]: "+result.Texture);
//							ProfileEvents.OnSocialActionFinished(Provider.FACEBOOK, SocialActionType.GET_CONTACTS);
							JSONObject jsonContacts = new JSONObject(result.Text);
							success(UserProfilesFromFBJsonObjs(jsonContacts["data"].list));
						}
					});
		}

//		public override void GetFeed(FeedSuccess success, FeedFailed fail) {
//
//		}

		public override void Logout(LogoutSuccess success, LogoutFailed fail) {
			FB.Logout();
			success();
		}

		public override void Login(LoginSuccess success, LoginFailed fail, LoginCancelled cancel) {
			FB.Login("email,publish_actions", (FBResult result) => {
				if (result.Error != null) {
					SoomlaUtils.LogDebug (TAG, "LoginCallback[result.Error]: " + result.Error);
					fail(result.Error);
				}
				else if (!FB.IsLoggedIn) {
					SoomlaUtils.LogDebug (TAG, "LoginCallback[cancelled]");
					cancel();
				}
				else {
					FB.API("/me/permissions", Facebook.HttpMethod.GET, delegate (FBResult response) {
						// inspect the response and adapt your UI as appropriate
						// check response.Text and response.Error
						SoomlaUtils.LogWarning(TAG, "me/permissions " + response.Text);
					});
					
					FB.API("/me?fields=id,name,email,first_name,last_name,picture",
					       Facebook.HttpMethod.GET, (FBResult result2) => {
						if (result2.Error != null) {
							SoomlaUtils.LogDebug (TAG, "ProfileCallback[result.Error]: " + result2.Error);

							fail(result2.Error);
						}
						else {
							SoomlaUtils.LogDebug(TAG, "ProfileCallback[result.Text]: "+result2.Text);
							SoomlaUtils.LogDebug(TAG, "ProfileCallback[result.Texture]: "+result2.Texture);
							string fbUserJson = result2.Text;
							UserProfile userProfile = UserProfileFromFBJsonString(fbUserJson);
							
							SoomlaProfile.StoreUserProfile (userProfile, true);

							success(userProfile);
						}
					});
				}
			});
		}

		public override bool IsLoggedIn() {
			return FB.IsLoggedIn;
		}

		public override void AppRequest(string message, string[] to, string extraData, string dialogTitle, AppRequestSuccess success, AppRequestFailed fail) {
			FB.AppRequest(message,
			              to,
			              "", null, null,
			              extraData,
			              dialogTitle,
			              (FBResult result) => {
								if (result.Error != null) {
									SoomlaUtils.LogError(TAG, "AppRequest[result.Error]: "+result.Error);
									fail(result.Error);
								}
								else {
									SoomlaUtils.LogDebug(TAG, "AppRequest[result.Text]: "+result.Text);
									SoomlaUtils.LogDebug(TAG, "AppRequest[result.Texture]: "+result.Texture);
									JSONObject jsonResponse = new JSONObject(result.Text);
									List<JSONObject> jsonRecipinets = jsonResponse["to"].list;
									List<string> recipients = new List<string>();
									foreach (JSONObject o in jsonRecipinets) {
										recipients.Add(o.str);
									}
									success(jsonResponse["request"].str, recipients);
								}
							});
		}


		/** Initialize Callbacks **/

		private void OnInitComplete()
		{
			SoomlaUtils.LogDebug(TAG, "FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		}

		private void OnHideUnity(bool isGameShown)
		{
			SoomlaUtils.LogDebug(TAG, "Is game showing? " + isGameShown);
		}



		/** Login Callbacks **/
		
		private void ProfileCallback(FBResult result) {

		}


		private static UserProfile UserProfileFromFBJsonString(string fbUserJsonStr) {
			return UserProfileFromFBJson(new JSONObject (fbUserJsonStr));
		}
		
		private static UserProfile UserProfileFromFBJson(JSONObject fbJsonObject) {
			JSONObject soomlaJsonObject = new JSONObject ();
			soomlaJsonObject.AddField(PJSONConsts.UP_PROVIDER, Provider.FACEBOOK.ToString ());
			soomlaJsonObject.AddField(PJSONConsts.UP_PROFILEID, fbJsonObject["id"].str);
			string name = fbJsonObject ["name"].str;
			soomlaJsonObject.AddField(PJSONConsts.UP_USERNAME, name);
			string email = fbJsonObject ["email"] != null ? fbJsonObject ["email"].str : null;
			if (email == null) {
				email = Regex.Replace(name, @"\s+", ".") + "@facebook.com";
			}
			soomlaJsonObject.AddField(PJSONConsts.UP_EMAIL, email);
			soomlaJsonObject.AddField(PJSONConsts.UP_FIRSTNAME, fbJsonObject["first_name"].str);
			soomlaJsonObject.AddField(PJSONConsts.UP_LASTNAME, fbJsonObject["last_name"].str);
			soomlaJsonObject.AddField(PJSONConsts.UP_AVATAR, fbJsonObject["picture"]["data"]["url"].str);
			UserProfile userProfile = new UserProfile (soomlaJsonObject);
			
			return userProfile;
		}
		
		private static List<UserProfile> UserProfilesFromFBJsonObjs(List<JSONObject> fbUserObjects) {
			List<UserProfile> profiles = new List<UserProfile>();
			foreach(JSONObject userObj in fbUserObjects) {
				profiles.Add(UserProfileFromFBJson(userObj));
			}
			return profiles;
		}


	}
}

