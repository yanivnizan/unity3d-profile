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
using Soomla.Store;
using Soomla.Example;

//namespace Soomla.Example {

	/// <summary>
	/// This class contains functions that initialize the game and that display the different screens of the game.
	/// </summary>
	public class ExampleWindow : MonoBehaviour {

		private static ExampleWindow instance = null;
		
		private GUIState guiState = GUIState.WELCOME;
		private Vector2 goodsScrollPosition = Vector2.zero;
		private Vector2 productScrollPosition = Vector2.zero;
		private bool isDragging = false;
		private Vector2 startTouch = Vector2.zero;
		private static ExampleEventHandler handler;
		
		public string fontSuffix = "";
	
		private enum GUIState{
			WELCOME,
			PRODUCTS,
			GOODS
		}
		
		private static bool isVisible = false;
		private bool isFBReady = false;

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

		private Texture2D tImgDirect;
		private Texture2D tLogoNew;
		private Font fgoodDog;
		private Font fgoodDogSmall;
		private Font fTitle;
		private Texture2D tWhitePixel;
		private Texture2D tMuffins;
		private Font fName;
		private Font fDesc;
		private Font fBuy;
		private Texture2D tBack;
		private Texture2D tGetMore;
		private Font tTitle;
		private Texture2D tFacebook;
		private Texture2D tTwitter;
		private Dictionary<string, Texture2D> itemsTextures;

		private bool bScreenshot = false;


		/// <summary>
		/// Starts this instance.
		/// Use this for initialization.
		/// </summary>
		void Start () {
			handler = new ExampleEventHandler();

			tImgDirect = (Texture2D)Resources.Load("SoomlaStore/images/img_direct");
			fgoodDog = (Font)Resources.Load("SoomlaStore/GoodDog" + fontSuffix);
			fgoodDogSmall = (Font)Resources.Load("SoomlaStore/GoodDog_small" + fontSuffix);
			tLogoNew = (Texture2D)Resources.Load("SoomlaStore/images/soomla_logo_new");
			tWhitePixel = (Texture2D)Resources.Load("SoomlaStore/images/white_pixel");
			fTitle = (Font)Resources.Load("SoomlaStore/Title" + fontSuffix);
			tMuffins = (Texture2D)Resources.Load("SoomlaStore/images/Muffins");
			fName = (Font)Resources.Load("SoomlaStore/Name" + fontSuffix);
			fDesc = (Font)Resources.Load("SoomlaStore/Description" + fontSuffix);
			fBuy = (Font)Resources.Load("SoomlaStore/Buy" + fontSuffix);
			tBack = (Texture2D)Resources.Load("SoomlaStore/images/back");
			tGetMore = (Texture2D)Resources.Load("SoomlaStore/images/GetMore");
			tTitle = (Font)Resources.Load("SoomlaStore/Title" + fontSuffix);
			tFacebook = (Texture2D)Resources.Load("SoomlaStore/images/facebook");
			tTwitter = (Texture2D)Resources.Load("SoomlaStore/images/twitter");

			ProfileEvents.OnLoginFinished += OnLoginFinished;
			ProfileEvents.OnSocialActionFinished += OnSocialActionFinished;
			ProfileEvents.OnSocialActionFailed += OnSocialActionFailed;
			ProfileEvents.OnGetContactsFinished += onGetContactsFinished;
			ProfileEvents.OnGetContactsFailed += onGetContactsFailed;
			ProfileEvents.OnGetFeedFinished += onGetFeedFinished;
			ProfileEvents.OnGetFeedFailed += onGetFeedFailed;
//			StoreEvents.OnSoomlaStoreInitialized += OnSoomlaStoreInitialized;

			SoomlaStore.Initialize(new MuffinRushAssets());
			SoomlaProfile.Initialize(true);
			FB.Init (
				onInitComplete: () => { isFBReady = true; }
			);

			#if UNITY_IPHONE
			Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.Gray);
			#elif UNITY_ANDROID
			Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
			#endif
		}

		public static ExampleWindow GetInstance() {
			return instance;
		}

		private VirtualGood vgToGive = null;
		private void OnLoginFinished(UserProfile userProfile) {
			if (userProfile.Provider == Provider.FACEBOOK) {
				Reward reward = new VirtualItemReward("status_" + vgToGive.ItemId, "", vgToGive.ItemId, 10);
				reward.Schedule = Schedule.AnyTimeUnLimited();

//				SoomlaProfile.UpdateStatus(Provider.FACEBOOK, "I love SOOMLA !", reward);
				FB.UpdateStatus("I love SOOMLA !", FB.RewardCallback(reward));

//				SoomlaProfile.UpdateStory(Provider.FACEBOOK,
//				                          "I think i love SOOMLA",
//				                          "Refaelos",
//				                          "this is a caption",
//				                          "Trying to test a story", 
//				                          "http://soom.la",
//				                          "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-xfp1/t31.0-1/c112.36.400.400/p480x480/902919_358601500912799_1525904972_o.jpg",
//				                          reward);

				FB.Feed(toId: "", //own timeline, can be ID of friend
				        link: "http://soom.la",
				        linkName: "Refaelos",
				        linkCaption: "this is a caption",
				        linkDescription: "Trying to test a story",
				        picture: "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-xfp1/t31.0-1/c112.36.400.400/p480x480/902919_358601500912799_1525904972_o.jpg",
				        callback: FB.RewardCallback(reward)
				        );

//				SoomlaProfile.GetContacts(Provider.FACEBOOK, null);
//				SoomlaProfile.GetFeed(Provider.FACEBOOK, null);

				FB.GetFriends();
				FB.GetFeed();

				// code for this needs to be done in LateUpdate/TakeScreenshot (see there)
//				bScreenshot = true;

			} else if (userProfile.Provider == Provider.TWITTER) {
				Reward reward = new VirtualItemReward("status_" + vgToGive.ItemId, "", vgToGive.ItemId, 11);
				reward.Schedule = Schedule.AnyTimeUnLimited();
//				SoomlaProfile.UpdateStatus(Provider.TWITTER, "I love SOOMLA !", reward);
//				SoomlaProfile.UpdateStory(Provider.TWITTER, "I think i love SOOMLA", "Refaelos", "this is a caption", "Trying to test a story", "http://soom.la", "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-xfp1/t31.0-1/c112.36.400.400/p480x480/902919_358601500912799_1525904972_o.jpg", reward);
			} else {
				SoomlaUtils.LogError("SOOMLA/UNITY ExampleWindow", "Unknown provider after login finished.");
			}
		}

		private void OnSocialActionFinished(Provider provider, SocialActionType socialActionType) {
			Handheld.StopActivityIndicator();
		}

		private void OnSocialActionFailed(Provider provider, SocialActionType socialActionType, string message) {
			Handheld.StopActivityIndicator();
		}

		private void onGetContactsFinished(Provider provider, List<UserProfile> contacts) {
			Handheld.StopActivityIndicator();
			SoomlaUtils.LogError("SOOMLA/UNITY ExampleWindow[onGetContactsFinished]", "friends:" + contacts);
		}
		private void onGetContactsFailed(Provider provider, string reason) {
			Handheld.StopActivityIndicator();
			SoomlaUtils.LogError("SOOMLA/UNITY ExampleWindow[onGetContactsFailed]", "reason:" + reason);
		}
		private void onGetFeedFinished(Provider provider, List<string> posts) {
			Handheld.StopActivityIndicator();
			SoomlaUtils.LogError("SOOMLA/UNITY ExampleWindow[onGetFeedFinished]", "posts:" + posts);
		}
		private void onGetFeedFailed(Provider provider, string reason) {
			Handheld.StopActivityIndicator();
			SoomlaUtils.LogError("SOOMLA/UNITY ExampleWindow[onGetFeedFailed]", "reason:" + reason);
		}

		/// <summary>
		/// Handles a soomla store initialized event.
		/// </summary>
//		public void OnSoomlaStoreInitialized() {
//			
//			// some usage examples for add/remove currency
//			// some examples
//			if (StoreInfo.GetVirtualCurrencies().Count>0) {
//				try {
//					StoreInventory.GiveItem(StoreInfo.GetVirtualCurrencies()[0].ItemId,4000);
//					SoomlaUtils.LogDebug("SOOMLA ExampleEventHandler", "Currency balance:" + StoreInventory.GetItemBalance(StoreInfo.GetVirtualCurrencies()[0].ItemId));
//				} catch (VirtualItemNotFoundException ex){
//					SoomlaUtils.LogError("SOOMLA ExampleEventHandler", ex.Message);
//				}
//			}
//			
//			setupItemsTextures();
//		}

		public void setupItemsTextures() {
			itemsTextures = new Dictionary<string, Texture2D>();
			
			foreach(VirtualGood vg in ExampleLocalStoreInfo.VirtualGoods){
				itemsTextures[vg.ItemId] = (Texture2D)Resources.Load("SoomlaStore/images/" + vg.Name);
			}
			foreach(VirtualCurrencyPack vcp in ExampleLocalStoreInfo.VirtualCurrencyPacks){
				itemsTextures[vcp.ItemId] = (Texture2D)Resources.Load("SoomlaStore/images/" + vcp.Name);
			}
		}

		/// <summary>
		/// Sets the window to open, and sets the GUI state to welcome. 
		/// </summary>
		public static void OpenWindow(){
			instance.guiState = GUIState.WELCOME;
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
			if(isVisible){
				//code to be able to scroll without the scrollbars.
				if(Input.GetMouseButtonDown(0)){
					startTouch = Input.mousePosition;
				}else if(Input.GetMouseButtonUp(0)){
					isDragging = false;
				}else if(Input.GetMouseButton(0)){
					if(!isDragging){
						if( Mathf.Abs(startTouch.y-Input.mousePosition.y) > 10f){
							isDragging = true;
						}
					}else{
						if(guiState == GUIState.GOODS){
							goodsScrollPosition.y -= startTouch.y - Input.mousePosition.y;
							startTouch = Input.mousePosition;
						}else if(guiState == GUIState.PRODUCTS){
							productScrollPosition.y -= startTouch.y - Input.mousePosition.y;
							startTouch = Input.mousePosition;
						}
					}
				}
			}

			if (Application.platform == RuntimePlatform.Android) {
				if (Input.GetKeyUp(KeyCode.Escape)) {
					//quit application on back button
					Application.Quit();
					return;
				}
			}
		}

		void LateUpdate() {
			if(bScreenshot) {
				bScreenshot = false;
				StartCoroutine(TakeScreenshot());
			}
		}

		/// <summary>
		/// Calls the relevant function to display the correct screen of the game.
		/// </summary>
		void OnGUI(){
			if(!isVisible){
				return;
			}
			//GUI.skin.verticalScrollbar.fixedWidth = 0;
			//GUI.skin.verticalScrollbar.fixedHeight = 0;
			//GUI.skin.horizontalScrollbar.fixedWidth = 0;
			//GUI.skin.horizontalScrollbar.fixedHeight = 0;
			GUI.skin.horizontalScrollbar = GUIStyle.none;
			GUI.skin.verticalScrollbar = GUIStyle.none;
			
			//disabling warnings because we use GUIStyle.none which result in warnings
			if(guiState == GUIState.WELCOME){
				welcomeScreen();
			}else if(guiState == GUIState.GOODS){
				goodsScreen();
			}else if(guiState == GUIState.PRODUCTS){
				currencyScreen();
			}	
		}
	
		private static string ScreenShotName(int width, int height) {
			return string.Format("{0}/screen_{1}x{2}_{3}.png", 
			                     Application.persistentDataPath, 
			                     width, height, 
			                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
		}

		private IEnumerator TakeScreenshot() {
			yield return new WaitForEndOfFrame ();

			// screenshot
			Texture2D image = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			image.ReadPixels(new Rect(0,0,Screen.width, Screen.height), 0,0,false);
			image.Apply();
			
			// using byte[]
//			byte[] imageBytes = image.EncodeToJPG();
			byte[] imageBytes = image.EncodeToPNG();
			Debug.Log ("imageBytes.Length:" + imageBytes.Length);
			Destroy (image);
//				SoomlaProfile.UploadImage(Provider.FACEBOOK, "screenshotBytes", imageBytes, "screen.png", 100, null);
			
			// using file
			// TODO: if using this code to upload via file, include the following permission in AndroidManifest.xml
			// TODO: <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
			//string screenshotPath = Application.dataPath + "/../SavedScreen.jpg";
			string screenshotPath = ScreenShotName (Screen.width, Screen.height);
			File.WriteAllBytes(screenshotPath, imageBytes);
			Debug.Log(string.Format("Took screenshot to: {0}", screenshotPath));

			FB.UploadImage ("screenshotFile", screenshotPath);
//			SoomlaProfile.UploadImage(Provider.FACEBOOK, "screenshotFile", screenshotPath, null);

			yield return null;
		}

		/// <summary>
		/// Displays the welcome screen of the game. 
		/// </summary>
		void welcomeScreen()
		{
			//drawing background, just using a white pixel here
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),tImgDirect);
			//changing the font and alignment the label, and making a backup so we can put it back.
			Font backupFont = GUI.skin.label.font;
			TextAnchor backupAlignment = GUI.skin.label.alignment;
			GUI.skin.label.font = fgoodDog;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			//writing the text.
			GUI.Label(new Rect(Screen.width/8,Screen.height/8f,Screen.width*6f/8f,Screen.height*0.3f),"Soomla Profile\nExample");
			//select the small font
			GUI.skin.label.font = fgoodDogSmall;
			GUI.Label(new Rect(Screen.width/8,Screen.height*7f/8f,Screen.width*6f/8f,Screen.height/8f),"Press the SOOMLA-bot to open store");
			//set font back to original
			GUI.skin.label.font = backupFont;
			GUI.Label(new Rect(Screen.width*0.25f,Screen.height/2-50,Screen.width*0.5f,100),"[ Your game here ]");
			//drawing button and testing if it has been clicked
			if(GUI.Button(new Rect(Screen.width*2/6,Screen.height*5f/8f,Screen.width*2/6,Screen.width*2/6),tLogoNew)){
				guiState = GUIState.GOODS;
#if UNITY_ANDROID && !UNITY_EDITOR
				SoomlaStore.StartIabServiceInBg();
#endif
			}
			//set alignment to backup
			GUI.skin.label.alignment = backupAlignment;
		}
	
		/// <summary>
		/// Display the goods screen of the game's store. 
		/// </summary>
		void goodsScreen()
		{
			//white background
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), tWhitePixel);
			Color backupColor = GUI.color;
			TextAnchor backupAlignment = GUI.skin.label.alignment;
			Font backupFont = GUI.skin.label.font;
			
			GUI.color = Color.red;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect(10,10,Screen.width-10,Screen.height-10),"SOOMLA Example Store");
			GUI.color = Color.black;
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.Label(new Rect(10,10,Screen.width-40,Screen.height),""+ ExampleLocalStoreInfo.CurrencyBalance);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.label.font = fTitle;
			GUI.Label(new Rect(0,Screen.height/8f,Screen.width,Screen.height/8f),"Virtual Goods");
			
			GUI.color = backupColor;
			GUI.DrawTexture(new Rect(Screen.width-30,10,30,30), tMuffins);
			float productSize = Screen.width*0.30f;
			float totalHeight = ExampleLocalStoreInfo.VirtualGoods.Count*productSize;
			//Here we start a scrollView, the first rectangle is the position of the scrollView on the screen,
			//the second rectangle is the size of the panel inside the scrollView.
			//All rectangles after this point are relative to the position of the scrollView.
			goodsScrollPosition = GUI.BeginScrollView(new Rect(0,Screen.height*2f/8f,Screen.width,Screen.height*5f/8f),goodsScrollPosition,new Rect(0,0,Screen.width,totalHeight));
			float y = 0;
			foreach(VirtualGood vg in ExampleLocalStoreInfo.VirtualGoods){
				GUI.color = backupColor;
				GUI.DrawTexture(new Rect(0,y,Screen.width,productSize),tWhitePixel);
				//We draw a button so we can detect a touch and then draw an image on top of it.
				//TODO
				//Resources.Load(path) The path is the relative path starting from the Resources folder.
				//Make sure the images used for UI, have the textureType GUI. You can change this in the Unity editor.
				GUI.color = backupColor;
				GUI.DrawTexture(new Rect(0+productSize/8f, y+productSize/8f,productSize*6f/8f,productSize*6f/8f), itemsTextures[vg.ItemId]);
				GUI.color = Color.black;
				GUI.skin.label.font = fName;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUI.Label(new Rect(productSize,y,Screen.width,productSize/3f),vg.Name);
				GUI.skin.label.font = fDesc;
				GUI.Label(new Rect(productSize + 10f,y+productSize/3f,Screen.width-productSize-15f,productSize/3f),vg.Description);

				GUI.color = Color.white;
				if(isFBReady && GUI.Button(new Rect(Screen.width/4f,y+productSize*2/3f,60f, 60f), tFacebook, GUIStyle.none)) {
					Debug.Log("SOOMLA/UNITY facebook clicked");
					Handheld.StartActivityIndicator();      
					vgToGive = vg;
//					SoomlaProfile.Login(Provider.FACEBOOK, null);
					FB.Login();
	            }
//	            if(GUI.Button(new Rect(70f+Screen.width/4f,y+productSize*2/3f,60f, 60f), tTwitter, GUIStyle.none)) {
//					Debug.Log("SOOMLA/UNITY twitter clicked");
//					Handheld.StartActivityIndicator();      
//					vgToGive = vg;
////					SoomlaProfile.Login(Provider.TWITTER, null);
//				}
				GUI.color = Color.black;

				GUI.Label(new Rect(Screen.width/2f,y+productSize*2/3f,Screen.width,productSize/3f),"price:" + ((PurchaseWithVirtualItem)vg.PurchaseType).Amount);
				GUI.Label(new Rect(Screen.width*3/4f,y+productSize*2/3f,Screen.width,productSize/3f), "Balance:" + ExampleLocalStoreInfo.GoodsBalances[vg.ItemId]);

				GUIStyle style = new GUIStyle("button");
				if(GUI.Button(new Rect(Screen.width*3/4f,y,Screen.width,productSize/2),"Click to buy", style) && !isDragging){
					Debug.Log("SOOMLA/UNITY wants to buy: " + vg.Name);
					try {
						StoreInventory.BuyItem(vg.ItemId);
					} catch (Exception e) {
						Debug.Log ("SOOMLA/UNITY " + e.Message);
					}
				}

				GUI.color = Color.white;
//				GUI.skin.label.alignment = TextAnchor.UpperRight;
//				GUI.skin.label.font = fBuy;
//				GUI.Label(new Rect(Screen.width*3/4f,y,Screen.width-10,productSize),"Click to buy");			
//				GUI.color = Color.grey;
//				GUI.DrawTexture(new Rect(0,y+productSize-1,Screen.width,1),tWhitePixel);
				y+= productSize;
			}
			GUI.EndScrollView();
			//We have just ended the scroll view this means that all the positions are relative top-left corner again.
			GUI.skin.label.alignment = backupAlignment;
			GUI.color = backupColor;
			GUI.skin.label.font = backupFont;
			
			float height = Screen.height/8f;
			float borderSize = height/8f;
			float buttonHeight = height-2*borderSize;
			float width = buttonHeight*180/95;
			if(GUI.Button(new Rect(Screen.width*2f/7f-width/2f,Screen.height*7f/8f+borderSize,width,buttonHeight), "back")){
				guiState = GUIState.WELCOME;
#if UNITY_ANDROID && !UNITY_EDITOR
				SoomlaStore.StopIabServiceInBg();
#endif
			}
			GUI.DrawTexture(new Rect(Screen.width*2f/7f-width/2f,Screen.height*7f/8f+borderSize,width,buttonHeight),tBack);
			width = buttonHeight*227/94;
			if(GUI.Button(new Rect(Screen.width*5f/7f-width/2f,Screen.height*7f/8f+borderSize,width,buttonHeight), "back")){
				guiState = GUIState.PRODUCTS;
			}
			GUI.DrawTexture(new Rect(Screen.width*5f/7f-width/2f,Screen.height*7f/8f+borderSize,width,buttonHeight),tGetMore);
		}
	
		/// <summary>
		/// Displays the currencies screen of the game's store. 
		/// </summary>
		void currencyScreen()
		{
			//white background
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),tWhitePixel);
			Color backupColor = GUI.color;
			TextAnchor backupAlignment = GUI.skin.label.alignment;
			Font backupFont = GUI.skin.label.font;
			
			GUI.color = Color.red;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect(10,10,Screen.width-10,Screen.height-10),"SOOMLA Example Store");
			GUI.color = Color.black;
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.Label(new Rect(10,10,Screen.width-40,Screen.height),""+ExampleLocalStoreInfo.CurrencyBalance);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.label.font = tTitle;
			GUI.Label(new Rect(0,Screen.height/8f,Screen.width,Screen.height/8f),"Virtual Currency Packs");
			
			GUI.color = backupColor;
			GUI.DrawTexture(new Rect(Screen.width-30,10,30,30),tMuffins);
			float productSize = Screen.width*0.30f;
			float totalHeight = ExampleLocalStoreInfo.VirtualGoods.Count*productSize;
			//Here we start a scrollView, the first rectangle is the position of the scrollView on the screen,
			//the second rectangle is the size of the panel inside the scrollView.
			//All rectangles after this point are relative to the position of the scrollView.
			productScrollPosition = GUI.BeginScrollView(new Rect(0,Screen.height*2f/8f,Screen.width,Screen.height*5f/8f),productScrollPosition,new Rect(0,0,Screen.width,totalHeight));
			float y = 0;
			foreach(VirtualCurrencyPack cp in ExampleLocalStoreInfo.VirtualCurrencyPacks){
				GUI.color = backupColor;
				//We draw a button so we can detect a touch and then draw an image on top of it.
				if(GUI.Button(new Rect(0,y,Screen.width,productSize),"") && !isDragging){
					Debug.Log("SOOMLA/UNITY Wants to buy: " + cp.Name);
					try {
						StoreInventory.BuyItem(cp.ItemId);
					} catch (Exception e) {
						Debug.Log ("SOOMLA/UNITY " + e.Message);
					}
				}
				GUI.DrawTexture(new Rect(0,y,Screen.width,productSize),tWhitePixel);
				//Resources.Load(path) The path is the relative path starting from the Resources folder.
				//Make sure the images used for UI, have the textureType GUI. You can change this in the Unity editor.
				GUI.DrawTexture(new Rect(0+productSize/8f, y+productSize/8f,productSize*6f/8f,productSize*6f/8f),itemsTextures[cp.ItemId]);
				GUI.color = Color.black;
				GUI.skin.label.font = fName;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUI.Label(new Rect(productSize,y,Screen.width,productSize/3f),cp.Name);
				GUI.skin.label.font = fDesc;
				GUI.Label(new Rect(productSize + 10f,y+productSize/3f,Screen.width-productSize-15f,productSize/3f),cp.Description);
				GUI.Label(new Rect(Screen.width*3/4f,y+productSize*2/3f,Screen.width,productSize/3f),"price:" + ((PurchaseWithMarket)cp.PurchaseType).MarketItem.Price.ToString("0.00"));
				GUI.skin.label.alignment = TextAnchor.UpperRight;
				GUI.skin.label.font = fBuy;
				GUI.Label(new Rect(0,y,Screen.width-10,productSize),"Click to buy");
				GUI.color = Color.grey;
				GUI.DrawTexture(new Rect(0,y+productSize-1,Screen.width,1),tWhitePixel);
				y+= productSize;
			}
			GUI.EndScrollView();
			//We have just ended the scroll view this means that all the positions are relative top-left corner again.
			GUI.skin.label.alignment = backupAlignment;
			GUI.color = backupColor;
			GUI.skin.label.font = backupFont;
			
			float height = Screen.height/8f;
			float borderSize = height/8f;
			float buttonHeight = height-2*borderSize;
			float width = buttonHeight*180/95;
			if(GUI.Button(new Rect(Screen.width/2f-width/2f,Screen.height*7f/8f+borderSize,width,buttonHeight), "back")){
				guiState = GUIState.GOODS;
			}
			GUI.DrawTexture(new Rect(Screen.width/2f-width/2f,Screen.height*7f/8f+borderSize,width,buttonHeight),tBack);
		}
	
	}
//}

