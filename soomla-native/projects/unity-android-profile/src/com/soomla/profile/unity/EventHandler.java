package com.soomla.profile.unity;

import com.soomla.BusProvider;
import com.soomla.profile.domain.IProvider;
import com.soomla.profile.domain.UserProfile;
import com.soomla.profile.events.auth.LoginCancelledEvent;
import com.soomla.profile.events.auth.LoginFailedEvent;
import com.soomla.profile.events.auth.LoginFinishedEvent;
import com.soomla.profile.events.auth.LoginStartedEvent;
import com.soomla.profile.events.auth.LogoutFailedEvent;
import com.soomla.profile.events.auth.LogoutFinishedEvent;
import com.soomla.profile.events.auth.LogoutStartedEvent;
import com.soomla.profile.events.social.SocialActionCancelledEvent;
import com.soomla.profile.events.social.SocialActionFailedEvent;
import com.soomla.profile.events.social.SocialActionFinishedEvent;
import com.soomla.profile.events.social.SocialActionStartedEvent;
import com.soomla.profile.social.ISocialProvider;

import org.json.JSONObject;

public class EventHandler {

    // events pushed from external provider (Unity FB SDK etc.)

    public static void pushEventLoginStarted(String providerStr) {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LoginStartedEvent(provider));
    }

    public static void pushEventLoginFinished(String userProfileJSON) throws Exception {
        JSONObject jsonObject = new JSONObject(userProfileJSON);
        UserProfile userProfile = new UserProfile(jsonObject);
        BusProvider.getInstance().post(new LoginFinishedEvent(userProfile));
    }

    public static void pushEventLoginFailed(String providerStr, String message) {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LoginFailedEvent(provider, message));
    }

    public static void pushEventLoginCancelled(String providerStr) {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LoginCancelledEvent(provider));
    }

    public static void pushEventLogoutStarted(String providerStr) {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LogoutStartedEvent(provider));
    }

    public static void pushEventLogoutFinished(String providerStr) throws Exception {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LogoutFinishedEvent(provider));
    }

    public static void pushEventLogoutFailed(String providerStr, String message) {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LogoutFailedEvent(provider, message));
    }

    public static void pushEventSocialActionStarted(String providerStr, String actionTypeStr) {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        ISocialProvider.SocialActionType socialActionType = ISocialProvider.SocialActionType.getEnum(actionTypeStr);
        BusProvider.getInstance().post(new SocialActionStartedEvent(provider, socialActionType));
    }

    public static void pushEventSocialActionFinished(String providerStr, String actionTypeStr) {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        ISocialProvider.SocialActionType socialActionType = ISocialProvider.SocialActionType.getEnum(actionTypeStr);
        BusProvider.getInstance().post(new SocialActionFinishedEvent(provider, socialActionType));
    }

    public static void pushEventSocialActionCancelled(String providerStr, String actionTypeStr) {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        ISocialProvider.SocialActionType socialActionType = ISocialProvider.SocialActionType.getEnum(actionTypeStr);
        BusProvider.getInstance().post(new SocialActionCancelledEvent(provider, socialActionType));
    }

    public static void pushEventSocialActionFailed(String providerStr, String actionTypeStr, String message) {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        ISocialProvider.SocialActionType socialActionType = ISocialProvider.SocialActionType.getEnum(actionTypeStr);
        BusProvider.getInstance().post(new SocialActionFailedEvent(provider, socialActionType, message));
    }
}
