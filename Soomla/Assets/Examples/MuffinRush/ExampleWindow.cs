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
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

using Soomla;
using Soomla.Profile;
using Soomla.Example;

/// <summary>
/// This class contains functions that initialize the game and that display the different screens of the game.
/// </summary>
public class ExampleWindow : MonoBehaviour {

	private static ExampleWindow instance = null;

	public string fontSuffix = "";
	
	private static bool isVisible = false;
	private bool isInit = false;


	/// <summary>
	/// Initializes the game state before the game starts. 
	/// </summary>
	void Awake(){
		if(instance == null){ 	//making sure we only initialize one instance.
			instance = this;
			GameObject.DontDestroyOnLoad(this.gameObject);
		} else {					//Destroying unused instances.
			GameObject.Destroy(this);
		}
		
		//FONT
		//using max to be certain we have the longest side of the screen, even if we are in portrait.
		if(Mathf.Max(Screen.width, Screen.height) > 640){ 
			fontSuffix = "_2X"; //a nice suffix to show the fonts are twice as big as the original
		}
	}

	private Texture2D tBackground;
	private Texture2D tShed;
	private Texture2D tBGBar;
	
	private Texture2D tShareDisable;
	private Texture2D tShare;
	private Texture2D tSharePress;
	
	private Texture2D tShareStoryDisable;
	private Texture2D tShareStory;
	private Texture2D tShareStoryPress;
	
	private Texture2D tUploadDisable;
	private Texture2D tUpload;
	private Texture2D tUploadPress;
	
	private Texture2D tConnect;
	private Texture2D tConnectPress;
	
	private Texture2D tLogout;
	private Texture2D tLogoutPress;

	private Font fgoodDog;

//	private bool bScreenshot = false;


	/// <summary>
	/// Starts this instance.
	/// Use this for initialization.
	/// </summary>
	void Start () {

		fgoodDog = (Font)Resources.Load("Fonts/GoodDog" + fontSuffix);

		tBackground = (Texture2D)Resources.Load("Profile/BG");
		tShed = (Texture2D)Resources.Load("Profile/Headline");
		tBGBar = (Texture2D)Resources.Load("Profile/BG-Bar");

		tShareDisable = (Texture2D)Resources.Load("Profile/BTN-Share-Disable");
		tShare = (Texture2D)Resources.Load("Profile/BTN-Share-Normal");
		tSharePress = (Texture2D)Resources.Load("Profile/BTN-Share-Press");

		tShareStoryDisable = (Texture2D)Resources.Load("Profile/BTN-ShareStory-Disable");
		tShareStory = (Texture2D)Resources.Load("Profile/BTN-ShareStory-Normal");
		tShareStoryPress = (Texture2D)Resources.Load("Profile/BTN-ShareStory-Press");

		tUploadDisable = (Texture2D)Resources.Load("Profile/BTN-Upload-Disable");
		tUpload = (Texture2D)Resources.Load("Profile/BTN-Upload-Normal");
		tUploadPress = (Texture2D)Resources.Load("Profile/BTN-Upload-Press");

		tConnect = (Texture2D)Resources.Load("Profile/BTN-Connect");
		tConnectPress = (Texture2D)Resources.Load("Profile/BTN-Connect-Press");

		tLogout = (Texture2D)Resources.Load("Profile/BTN-LogOut");
		tLogoutPress = (Texture2D)Resources.Load("Profile/BTN-LogOut-Press");


		// examples of catching fired events
		ProfileEvents.OnSoomlaProfileInitialized += () => {
			Soomla.SoomlaUtils.LogDebug("ExampleWindow", "SoomlaProfile Initialized !");
			isInit = true;
		};
		
		ProfileEvents.OnLoginFinished += (UserProfile UserProfile) => {
			Soomla.SoomlaUtils.LogDebug("ExampleWindow", "login finished for: " + UserProfile.toJSONObject().print());
			SoomlaProfile.GetContacts(Provider.FACEBOOK);
		};
		
		ProfileEvents.OnGetContactsFinished += (Provider provider, List<UserProfile> profiles) => {
			Soomla.SoomlaUtils.LogDebug("ExampleWindow", "get contacts for: " + profiles.Count);
		};



		SoomlaProfile.Initialize();

		#if UNITY_IPHONE
		Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.Gray);
		#elif UNITY_ANDROID
		Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
		#endif
	}
	
	/// <summary>
	/// Sets the window to open, and sets the GUI state to welcome. 
	/// </summary>
	public static void OpenWindow(){
		isVisible = true;
	}

	/// <summary>
	/// Sets the window to closed. 
	/// </summary>
	public static void CloseWindow(){
		isVisible = false;
	}

	/// <summary>
	/// Implements the game behavior of MuffinRush. 
	/// Overrides the superclass function in order to provide functionality for our game. 
	/// </summary>
	void Update () {
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKeyUp(KeyCode.Escape)) {
				//quit application on back button
				Application.Quit();
				return;
			}
		}
	}

//	void LateUpdate() {
//		if(bScreenshot) {
//			bScreenshot = false;
//			StartCoroutine(TakeScreenshot());
//		}
//	}

	/// <summary>
	/// Calls the relevant function to display the correct screen of the game.
	/// </summary>
	void OnGUI(){
		if(!isVisible){
			return;
		}

		GUI.skin.horizontalScrollbar = GUIStyle.none;
		GUI.skin.verticalScrollbar = GUIStyle.none;
		
		welcomeScreen();
	}

	/// <summary>
	/// Displays the welcome screen of the game. 
	/// </summary>
	void welcomeScreen()
	{
		Color backupColor = GUI.color;
		float vertGap = 80f;

		//drawing background, just using a white pixel here
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),tBackground);
		GUI.DrawTexture(new Rect(0,0,Screen.width,timesH(240f)), tShed, ScaleMode.StretchToFill, true);


		float rowsTop = 300.0f;
		float rowsHeight = 120.0f;

		GUI.DrawTexture(new Rect(timesW(65.0f),timesH(rowsTop+10f),timesW(516.0f),timesH(102.0f)), tBGBar, ScaleMode.StretchToFill, true);

		if (SoomlaProfile.IsLoggedIn(Provider.FACEBOOK)) {
			GUI.skin.button.normal.background = tShare;
			GUI.skin.button.hover.background = tShare;
			GUI.skin.button.active.background = tSharePress;
			if(GUI.Button(new Rect(timesW(50.0f),timesH(rowsTop),timesW(212.0f),timesH(120.0f)), "")){
				SoomlaProfile.UpdateStatus(Provider.FACEBOOK, "I LOVE SOOMLA !  http://www.soom.la", null);
			}

//			GUI.DrawTexture(new Rect(timesW(50.0f),timesH(rowsTop),timesW(212.0f),timesH(120.0f)), tShare, 
//			                ScaleMode.StretchToFill, true);
		} else {
			GUI.DrawTexture(new Rect(timesW(50.0f),timesH(rowsTop),timesW(212.0f),timesH(120.0f)), tShareDisable, 
			                ScaleMode.StretchToFill, true);
		}

		GUI.color = Color.black;
		GUI.skin.label.font = fgoodDog;
		GUI.skin.label.fontSize = 30;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(timesW(270.0f),timesH(rowsTop),timesW(516.0f-212.0f),timesH(120.0f)),"I Love SOOMLA!");
		GUI.color = backupColor;


		rowsTop += vertGap + rowsHeight;


		GUI.DrawTexture(new Rect(timesW(65.0f),timesH(rowsTop+10f),timesW(516.0f),timesH(102.0f)), tBGBar, ScaleMode.StretchToFill, true);

		if (SoomlaProfile.IsLoggedIn(Provider.FACEBOOK)) {
			GUI.skin.button.normal.background = tShareStory;
			GUI.skin.button.hover.background = tShareStory;
			GUI.skin.button.active.background = tShareStoryPress;
			if(GUI.Button(new Rect(timesW(50.0f),timesH(rowsTop),timesW(212.0f),timesH(120.0f)), "")){
				SoomlaProfile.UpdateStory(Provider.FACEBOOK,
				                          "This is the story of a very strong and brave SOOMBOT on his jurney from SOOMBOTIA to a far away galaxy. That galaxy contains a blue planet where all human game developers love to eat food spiced with marshmallow.",
				                          "The story of SOOMBOT (Profile Test App)",
				                          "SOOMBOT Story",
				                          "http://about.soom.la/soombots",
				                          "http://about.soom.la/wp-content/uploads/2014/05/330x268-spockbot.png",
				                          null);
			}
//			GUI.DrawTexture(new Rect(timesW(50.0f),timesH(rowsTop),timesW(212.0f),timesH(120.0f)), tShareStory, 
//			                ScaleMode.StretchToFill, true);
		} else {
			GUI.DrawTexture(new Rect(timesW(50.0f),timesH(rowsTop),timesW(212.0f),timesH(120.0f)), tShareStoryDisable, 
			                ScaleMode.StretchToFill, true);
		}
		
		GUI.color = Color.black;
		GUI.skin.label.font = fgoodDog;
		GUI.skin.label.fontSize = 25;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(timesW(270.0f),timesH(rowsTop),timesW(516.0f-212.0f),timesH(120.0f)),"Full story of The SOOMBOT!");
		GUI.color = backupColor;



		rowsTop += vertGap + rowsHeight;
		
		
		GUI.DrawTexture(new Rect(timesW(65.0f),timesH(rowsTop+10f),timesW(516.0f),timesH(102.0f)), tBGBar, ScaleMode.StretchToFill, true);
		
		if (SoomlaProfile.IsLoggedIn(Provider.FACEBOOK)) {
			GUI.skin.button.normal.background = tUpload;
			GUI.skin.button.hover.background = tUpload;
			GUI.skin.button.active.background = tUploadPress;
			if(GUI.Button(new Rect(timesW(50.0f),timesH(rowsTop),timesW(212.0f),timesH(120.0f)), "")){
				SoomlaProfile.UploadCurrentScreenShot(this, Provider.FACEBOOK, "Awesome Test App of SOOMLA Profile!", "This a screenshot of the current state of SOOMLA's test app on my computer.", null);
			}
//			GUI.DrawTexture(new Rect(timesW(50.0f),timesH(rowsTop),timesW(212.0f),timesH(120.0f)), tUpload, 
//			                ScaleMode.StretchToFill, true);
		} else {
			GUI.DrawTexture(new Rect(timesW(50.0f),timesH(rowsTop),timesW(212.0f),timesH(120.0f)), tUploadDisable, 
			                ScaleMode.StretchToFill, true);
		}
		
		GUI.color = Color.black;
		GUI.skin.label.font = fgoodDog;
		GUI.skin.label.fontSize = 28;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(timesW(270.0f),timesH(rowsTop),timesW(516.0f-212.0f),timesH(120.0f)),"Current Screenshot");
		GUI.color = backupColor;



		if (SoomlaProfile.IsLoggedIn(Provider.FACEBOOK)) {

			GUI.skin.button.normal.background = tLogout;
			GUI.skin.button.hover.background = tLogout;
			GUI.skin.button.active.background = tLogoutPress;
			if(GUI.Button(new Rect(timesW(20.0f),timesH(950f),timesW(598.0f),timesH(141.0f)), "")){
				SoomlaProfile.Logout(Provider.FACEBOOK);
			}
//			GUI.DrawTexture(new Rect(timesW(20.0f),timesH(950f),timesW(598.0f),timesH(141.0f)), tLogout, 
//			                ScaleMode.StretchToFill, true);

		} else if (isInit) {
			GUI.skin.button.normal.background = tConnect;
			GUI.skin.button.hover.background = tConnect;
			GUI.skin.button.active.background = tConnectPress;
			if(GUI.Button(new Rect(timesW(20.0f),timesH(950f),timesW(598.0f),timesH(141.0f)), "")){
				SoomlaProfile.Login(Provider.FACEBOOK, null);
			}
//			GUI.DrawTexture(new Rect(timesW(20.0f),timesH(950f),timesW(598.0f),timesH(141.0f)), tConnect, 
//			                ScaleMode.StretchToFill, true);
		}

	}


	private static string ScreenShotName(int width, int height) {
		return string.Format("{0}/screen_{1}x{2}_{3}.png", 
		                     Application.persistentDataPath, 
		                     width, height, 
		                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}
	
	private float timesW(float f) {
		return (float)(f/640.0)*Screen.width;
	}
	
	private float timesH(float f) {
		return (float)(f/1136.0)*Screen.height;
	}

}

