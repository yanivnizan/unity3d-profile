package com.soomla.profile.unity;

import com.soomla.profile.domain.UserProfile;
import com.soomla.profile.events.LoginCancelledEvent;
import com.soomla.profile.events.UserProfileUpdatedEvent;
import com.soomla.profile.events.auth.LoginFailedEvent;
import com.soomla.profile.events.auth.LoginFinishedEvent;
import com.soomla.profile.events.auth.LoginStartedEvent;
import com.soomla.profile.events.auth.LogoutFailedEvent;
import com.soomla.profile.events.auth.LogoutFinishedEvent;
import com.soomla.profile.events.auth.LogoutStartedEvent;
import com.soomla.profile.events.social.SocialActionFailedEvent;
import com.soomla.profile.events.social.SocialActionFinishedEvent;
import com.soomla.profile.events.social.SocialActionStartedEvent;
import com.soomla.store.BusProvider;
import com.squareup.otto.Subscribe;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

public class EventHandler {
    private static EventHandler mLocalEventHandler;

    public static void initialize() {
        mLocalEventHandler = new EventHandler();

    }

    public EventHandler() {
        BusProvider.getInstance().register(this);
    }

    @Subscribe
    public void onLoginCancelled(LoginCancelledEvent loginCancelledEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLoginCancelled", "");
    }

    @Subscribe
    public void onUserProfileUpdated(UserProfileUpdatedEvent userProfileUpdatedEvent) {
        UserProfile userProfile = userProfileUpdatedEvent.UserProfile;
        UnityPlayer.UnitySendMessage("ProfileEvents", "onUserProfileUpdated", userProfile.toJSONObject().toString());
    }

    @Subscribe
    public void onLoginFailed(LoginFailedEvent loginFailedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLoginFailed", loginFailedEvent.ErrorDescription);
    }

    @Subscribe
    public void onLoginFinished(LoginFinishedEvent loginFinishedEvent) {
        UserProfile userProfile = loginFinishedEvent.UserProfile;
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLoginFinished", userProfile.toJSONObject().toString());
    }

    @Subscribe
    public void onLoginStarted(LoginStartedEvent loginStartedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLoginStarted", loginStartedEvent.Provider.toString());
    }

    @Subscribe
    public void onLogoutFailed(LogoutFailedEvent logoutFailedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLogoutFailed", logoutFailedEvent.ErrorDescription);
    }

    @Subscribe
    public void onLogoutFinished(LogoutFinishedEvent logoutFinishedEvent) {
        UserProfile userProfile = logoutFinishedEvent.UserProfile;
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLogoutFinished", userProfile.toJSONObject().toString());
    }

    @Subscribe
    public void onLogoutStarted(LogoutStartedEvent logoutStartedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLogoutStarted", logoutStartedEvent.Provider.toString());
    }

    @Subscribe
    public void onSocialActionFailed(SocialActionFailedEvent socialActionFailedEvent) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("socialActionType", socialActionFailedEvent.SocialActionType.toString());
            jsonObject.put("errMsg", socialActionFailedEvent.ErrorDescription);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onSocialActionFailed", jsonObject.toString());
    }

    @Subscribe
    public void onSocialActionFinished(SocialActionFinishedEvent socialActionFinishedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onSocialActionFinished", socialActionFinishedEvent.SocialActionType.toString());
    }

    @Subscribe
    public void onSocialActionStarted(SocialActionStartedEvent socialActionStartedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onSocialActionStarted", socialActionStartedEvent.SocialActionType.toString());
    }

}
