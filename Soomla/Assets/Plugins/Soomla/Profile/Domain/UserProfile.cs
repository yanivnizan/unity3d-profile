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
using System.Runtime.InteropServices;
using System;

namespace Soomla.Profile {

	/// <summary>

	/// </summary>
	public class UserProfile {

//#if UNITY_IOS && !UNITY_EDITOR
//		[DllImport ("__Internal")]
//		private static extern int storeAssets_Save(string type, string viJSON);
//#endif

		private const string TAG = "SOOMLA UserProfile";

		public Provider Provider;

		public string ProfileId;
		public string Email;
		public string FirstName;
		public string LastName;
		public string AvatarLink;
		public string Location;
		public string Gender;
		public string Language;
		public string Birthday;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		protected UserProfile(Provider provider, string profileId, string firstName, string lastName, string email)
		{
			this.Provider = provider;
			this.ProfileId = profileId;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.Email = email;
		}
		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		protected VirtualItem(AndroidJavaObject jniVirtualItem) {
//			this.Name = jniVirtualItem.Call<string>("getName");
//			this.Description = jniVirtualItem.Call<string>("getDescription");
//			this.ItemId = jniVirtualItem.Call<string>("getItemId");
//		}
//#endif

		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>UserProfile</c> from the given <c>JSONObject</c>.
		/// </summary>
		/// <param name="jsonUP">A JSONObject representation of the wanted <c>UserProfile</c>.</param>
		public UserProfile(JSONObject jsonUP) {
			this.Provider = Provider.fromString(jsonUP[PJSONConsts.UP_PROVIDER].str);
			this.ProfileId = jsonUP[PJSONConsts.UP_PROFILEID].str;
			this.FirstName = jsonUP[PJSONConsts.UP_FIRSTNAME].str;
			this.LastName = jsonUP[PJSONConsts.UP_LASTNAME].str;
			this.Email = jsonUP[PJSONConsts.UP_EMAIL].str;

			if (jsonUP[PJSONConsts.UP_AVATAR]) {
				this.AvatarLink = jsonUP[PJSONConsts.UP_AVATAR].str;
			} else {
				this.AvatarLink = "";
			}
			if (jsonUP[PJSONConsts.UP_LOCATION]) {
				this.Location = jsonUP[PJSONConsts.UP_LOCATION].str;
			} else {
				this.Location = "";
			}
			if (jsonUP[PJSONConsts.UP_GENDER]) {
				this.Gender = jsonUP[PJSONConsts.UP_GENDER].str;
			} else {
				this.Gender = "";
			}
			if (jsonUP[PJSONConsts.UP_LANGUAGE]) {
				this.Language = jsonUP[PJSONConsts.UP_LANGUAGE].str;
			} else {
				this.Language = "";
			}
			if (jsonUP[PJSONConsts.UP_BIRTHDAY]) {
				this.Birthday = jsonUP[PJSONConsts.UP_BIRTHDAY].str;
			} else {
				this.Birthday = "";
			}
		}
//		
//		/// <summary>
//		/// Converts the current <c>VirtualItem</c> to a JSONObject.
//		/// </summary>
//		/// <returns>A <c>JSONObject</c> representation of the current <c>VirtualItem</c>.</returns>
//		public virtual JSONObject toJSONObject() {
//			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
//			obj.AddField(JSONConsts.ITEM_NAME, this.Name);
//			obj.AddField(JSONConsts.ITEM_DESCRIPTION, this.Description);
//			obj.AddField(JSONConsts.ITEM_ITEMID, this.ItemId);
//			
//			return obj;
//		}


		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		private static bool isInstanceOf(AndroidJavaObject jniItem, string classJniStr) {
//			System.IntPtr cls = AndroidJNI.FindClass(classJniStr);
//			return AndroidJNI.IsInstanceOf(jniItem.GetRawObject(), cls);
//		}
//		
//		public static VirtualItem factoryItemFromJNI(AndroidJavaObject jniItem) {
//			StoreUtils.LogDebug(TAG, "Trying to create VirtualItem with itemId: " + jniItem.Call<string>("getItemId"));
//			
//			if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/SingleUseVG")) {
//				return new SingleUseVG(jniItem);
//			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/EquippableVG")) {
//				return new EquippableVG(jniItem);
//			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/UpgradeVG")) {
//				return new UpgradeVG(jniItem);
//			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/LifetimeVG")) {
//				return new LifetimeVG(jniItem);
//			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/SingleUsePackVG")) {
//				return new SingleUsePackVG(jniItem);
//			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualCurrencies/VirtualCurrency")) {
//				return new VirtualCurrency(jniItem);
//			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualCurrencies/VirtualCurrencyPack")) {
//				return new VirtualCurrencyPack(jniItem);
//			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/NonConsumableItem")) {
//				return new NonConsumableItem(jniItem);
//			} else {
//				StoreUtils.LogError(TAG, "Couldn't determine what type of class is the given jniItem.");
//			}
//			
//			return null;
//		}
//#endif

//		/// <summary>
//		/// Saves this instance according to type.
//		/// </summary>
//		/// <param name="type">type</param>
//		protected void save(string type) 
//		{
//#if !UNITY_EDITOR
//			string viJSON = this.toJSONObject().print();
//#if UNITY_IOS
//			storeAssets_Save(type, viJSON);
//#elif UNITY_ANDROID
//			AndroidJNI.PushLocalFrame(100);
//			using(AndroidJavaClass jniStoreAssets = new AndroidJavaClass("com.soomla.unity.StoreAssets")) {
//				jniStoreAssets.CallStatic("save", type, viJSON);
//			}
//			AndroidJNI.PopLocalFrame(IntPtr.Zero);
//#endif
//#endif
//		}
	}
}

