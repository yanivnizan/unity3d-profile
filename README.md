*This project is a part of The [SOOMLA](http://www.soom.la) Framework which is a series of open source initiatives with a joint goal to help mobile game developers do more together. SOOMLA encourages better game designing, economy modeling and faster development.*

Haven't you ever wanted a status sharing one liner that looks like this ?!

```cs
SoomlaProfile.UpdateStatus(Provider.FACEBOOK, "I love this game !", new VirtualItemReward( ... ));
```

unity3d-profile
---

*SOOMLA's Profile Module for Unity3d*

* More documentation and information in SOOMLA's [Knowledge Base](http://know.soom.la/docs/platforms/unity)  
* For issues you can use the [issues](https://github.com/soomla/unity3d-profile/issues) section or SOOMLA's [Answers Website](http://answers.soom.la)

unity3d-profile is the Unity3d flavor of SOOMLA's Profile Module.

![SOOMLA's Profile Module](http://know.soom.la/img/tutorial_img/soomla_diagrams/EconomyModel.png)


## Download

####Pre baked unitypackage: [unity3d-profile v1.0](http://bit.ly/1sUDdG0)  

## Debugging

If you want to see full debug messages from android-store and ios-store you just need to check the box that says "Debug Messages" in the SOOMLA Settings.
Unity debug messages will only be printed out if you build the project with _Development Build_ checked.

## Cloning

There are some necessary files in submodules linked with symbolic links. If you're cloning the project make sure you clone it with the `--recursive` flag.

```
$ git clone --recursive git@github.com:soomla/unity3d-profile.git
```

## Getting Started

1. Go over the guidelines for downloading and importing the official Facebook SDK: https://developers.facebook.com/docs/unity/getting-started/canvas    - You don't need to initialize FB. SoomlaProfile will initialize it for you.
2. Move the folder `Facebook` from `Assets` to `Assets/Plugins`  -  SOOMLA works from the Plugins folder so it'll be available to UnityScript devs. So you'll have to move Facebook in there as well.
3. Download and import [soomla-unity3d-core.unitypackage](https://github.com/soomla/unity3d-profile/blob/master/soomla-unity3d-core.unitypackage) and [unity3d-profile.unitypackage](http://bit.ly/1sUDdG0). If you also want to use Store related rewards you'll need to go over the instructions of [unity3d-store](https://github.com/soomla/unity3d-store)
4. Drag the "CoreEvents" and "ProfileEvents" Prefabs from `../Assets/Soomla/Prefabs` into your scene. You should see it listed in the "Hierarchy" panel. [This step MUST be done for unity3d-profile to work properly!]
5. On the menu bar click "Window -> Soomla -> Edit Settings" and change the value for "Soomla Secret".
    - _Soomla Secret_ - is an encryption secret you provide that will be used to secure your data. (If you used versions before v1.5.2 this secret MUST be the same as Custom Secret)  
    **Choose the secret wisely. You can't change them after you launch your game!**
6. Initialize _SoomlaProfile_:

    ```cs
    SoomlaProfile.Initialize();
    ```

    > Initialize _SoomlaProfile_ ONLY ONCE when your application loads.

    > Initialize _SoomlaProfile_ in the "Start()" function of a 'MonoBehaviour' and **NOT** in the "Awake()" function. SOOMLA has its own 'MonoBehaviour' and it needs to be "Awakened" before you initialize.

    > _SoomlaProfile_ will initialize the social providers. Don't initialize them on your own (for example, **don't** call `FB.Init()` !).

7. Call all the social functions you can from _SoomlaProfile_ class. Otherwise, you won't be able to work with SOOMLA correctly. You can still call functions from the `FB` class but only those that are not provided by _SoomlaProfile_.

8. You'll need event handlers in order to be notified about in-app purchasing related events. refer to the [Event Handling](https://github.com/soomla/unity3d-profile#event-handling) section for more information.

And that's it ! You have social capabilities for your game.


## What's next? Social Actions.

The Profile module is young and only a few social actions are provided. We're always working on extending the social capabilities and hope the community will "jump" on the chance to create them and even connect them with SOOMLA's modules (Store and LevelUp).

Here is an example of sharing a story on the user's feed:

After you initialized SoomlaProfile and logged the user in:

```cs
  SoomlaProfile.UpdateStory(
                  Provider.FACEBOOK,
                  "Check out this great story by SOOMLA !",  
                  "SOOMLA is 2 years young!",
                  "soomla_2_years",
                  "http://blog.soom.la/2014/08/congratulations-soomla-is-2-years-young.html",
                  "http://blog.soom.la/wp-content/uploads/2014/07/Birthday-bot-300x300.png",
                  new BadgeReward("sherriff", "Sheriff"));
```

And that's it! unity3d-profile knows how to contact Facebook and share a story with the information you provided.  
It will also give the user the `BadgeReward` we configured in the function call.


Storage
---

unity3d-profile is caching user information on the device. You can access it through:

```cs
UserProfile userProfile = SoomlaProfile.GetStoredUserProfile(Provider.FACEBOOK);
```

The on-device storage is encrypted and kept in a SQLite database. SOOMLA is preparing a cloud-based storage service that will allow this SQLite to be synced to a cloud-based repository that you'll define.

Event Handling
---

SOOMLA lets you subscribe to profile events, get notified and implement your own application specific behavior to those events.

> Your behavior is an addition to the default behavior implemented by SOOMLA. You don't replace SOOMLA's behavior.

The 'ProfileEvents' class is where all event go through. To handle various events, just add your specific behavior to the delegates in the Events class.

For example, if you want to 'listen' to a MarketPurchase event:

```cs
StoreEvents.OnMarketPurchase += onMarketPurchase;

ProfileEvents.OnLoginFinished += (UserProfile UserProfile) => {
			Soomla.SoomlaUtils.LogDebug("My Perfect Game", "login finished with profile: " + UserProfile.toJSONObject().print());
			SoomlaProfile.GetContacts(Provider.FACEBOOK);
};
```

Contribution
---

We want you!

Fork -> Clone -> Implement -> Insert Comments -> Test -> Pull-Request.

We have great RESPECT for contributors.

Code Documentation
---

unity3d-profile follows strict code documentation conventions. If you would like to contribute please read our [Documentation Guidelines](https://github.com/soomla/unity3d-profile/tree/master/documentation.md) and follow them. Clear, consistent  comments will make the code easier to understand.

SOOMLA, Elsewhere ...
---

+ [Framework Website](http://www.soom.la/)
+ [On Facebook](https://www.facebook.com/pages/The-SOOMLA-Project/389643294427376).
+ [On AngelList](https://angel.co/the-soomla-project)

License
---
Apache License. Copyright (c) 2012-2014 SOOMLA. http://www.soom.la
+ http://opensource.org/licenses/Apache-2.0
