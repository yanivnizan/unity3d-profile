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
using System.IO;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Soomla.Profile
{
	
	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	/// <summary>
	/// This class holds the store's configurations. 
	/// </summary>
	public class ProfileSettings : ISoomlaSettings
	{
		
		#if UNITY_EDITOR
		
		static ProfileSettings instance = new ProfileSettings();
		static ProfileSettings()
		{
			SoomlaEditorScript.addSettings(instance);
		}
		
		bool showAndroidSettings = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android);
		bool showIOSSettings = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iPhone);
		
		GUIContent fbAppId = new GUIContent("FB app Id:");
		GUIContent fbAppNS = new GUIContent("FB app namespace:");
		
		public void OnEnable() {
			// Generating AndroidManifest.xml
			//			ManifestTools.GenerateManifest();
		}
		
		public void OnModuleGUI() {
//			AndroidGUI();
//			EditorGUILayout.Space();
//			IOSGUI();
		}
		
		public void OnInfoGUI() {
			
		}
		
		public void OnSoomlaGUI() {
			
		}
		
		private void IOSGUI()
		{
			showIOSSettings = EditorGUILayout.Foldout(showIOSSettings, "iOS Build Settings");
			if (showIOSSettings)
			{
				EditorGUILayout.BeginHorizontal();
				SoomlaEditorScript.SelectableLabelField(fbAppId, FB_APP_ID_DEFAULT);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				
				EditorGUILayout.BeginHorizontal();
				SoomlaEditorScript.SelectableLabelField(fbAppNS, PlayerSettings.productName);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Space();
		}
		
		private void AndroidGUI()
		{
			showAndroidSettings = EditorGUILayout.Foldout(showAndroidSettings, "Android Settings");
			if (showAndroidSettings)
			{
				EditorGUILayout.BeginHorizontal();
				SoomlaEditorScript.SelectableLabelField(fbAppId, FB_APP_ID_DEFAULT);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space();

				EditorGUILayout.BeginHorizontal();
				SoomlaEditorScript.SelectableLabelField(fbAppNS, PlayerSettings.productName);
				EditorGUILayout.EndHorizontal();

//				EditorGUILayout.Space();
//				EditorGUILayout.HelpBox("Social Provider Selection", MessageType.None);
			}
			EditorGUILayout.Space();
		}
		

		/** Social Providers util functions **/
		
		private void setCurrentBPUpdate(string bpKey) {
			spUpdate[bpKey] = true;
			var buffer = new List<string>(spUpdate.Keys);
			foreach(string key in buffer) {
				if (key != bpKey) {
					spUpdate[key] = false;
				}
			}
		}
		
		private Dictionary<string, bool> spUpdate = new Dictionary<string, bool>();
		private static string spRootPath = Application.dataPath + "/Soomla/compilations/android-social-services/";
		
		public static void handleFBJars(bool remove) {
			try {
				if (remove) {
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreGooglePlay.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreGooglePlay.jar.meta");
				} else {
					FileUtil.CopyFileOrDirectory(spRootPath + "google-play/AndroidStoreGooglePlay.jar",
					                             Application.dataPath + "/Plugins/Android/AndroidStoreGooglePlay.jar");
				}
			}catch {}
		}
		
		public static void handleSocialAuthJars(bool remove) {
			try {
				if (remove) {
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreAmazon.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreAmazon.jar.meta");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/in-app-purchasing-1.0.3.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/in-app-purchasing-1.0.3.jar.meta");
				} else {
					FileUtil.CopyFileOrDirectory(spRootPath + "amazon/AndroidStoreAmazon.jar",
					                             Application.dataPath + "/Plugins/Android/AndroidStoreAmazon.jar");
					FileUtil.CopyFileOrDirectory(spRootPath + "amazon/in-app-purchasing-1.0.3.jar",
					                             Application.dataPath + "/Plugins/Android/in-app-purchasing-1.0.3.jar");
				}
			}catch {}
		}
		
		
		
		#endif
		
		

		
		/** Store Specific Variables **/
		
		
		public static string FB_APP_ID_DEFAULT = "YOUR FB APP ID";
		
		public static string FBAppId
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("FBAppId", out value) ? value : FB_APP_ID_DEFAULT;
			}
			set 
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("FBAppId", out v);
				if (v != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("FBAppId",value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static string FB_APP_NS_DEFAULT = "YOUR FB APP ID";
		
		public static string FBAppNamespace
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("FBAppNS", out value) ? value : FB_APP_NS_DEFAULT;
			}
			set 
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("FBAppNS", out v);
				if (v != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("FBAppNS",value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}
	}
}