package com.soomla.profile.unity;

import com.soomla.BusProvider;
import com.soomla.events.RewardGivenEvent;
import com.soomla.profile.domain.UserProfile;
import com.soomla.profile.events.UserProfileUpdatedEvent;
import com.soomla.profile.events.auth.LoginCancelledEvent;
import com.soomla.profile.events.auth.LoginFailedEvent;
import com.soomla.profile.events.auth.LoginFinishedEvent;
import com.soomla.profile.events.auth.LoginStartedEvent;
import com.soomla.profile.events.auth.LogoutFailedEvent;
import com.soomla.profile.events.auth.LogoutFinishedEvent;
import com.soomla.profile.events.auth.LogoutStartedEvent;
import com.soomla.profile.events.social.GetContactsFailedEvent;
import com.soomla.profile.events.social.GetContactsFinishedEvent;
import com.soomla.profile.events.social.GetContactsStartedEvent;
import com.soomla.profile.events.social.GetFeedFailedEvent;
import com.soomla.profile.events.social.GetFeedFinishedEvent;
import com.soomla.profile.events.social.GetFeedStartedEvent;
import com.soomla.profile.events.social.SocialActionFailedEvent;
import com.soomla.profile.events.social.SocialActionFinishedEvent;
import com.soomla.profile.events.social.SocialActionStartedEvent;
import com.soomla.rewards.BadgeReward;
import com.squareup.otto.Subscribe;
import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;

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

    @Subscribe
    public void onGetContactsFailedEvent(GetContactsFailedEvent getContactsFailedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetContactsFailedEvent", getContactsFailedEvent.ErrorDescription);
    }

    @Subscribe
    public void onGetContactsFinishedEvent(GetContactsFinishedEvent getContactsFinishedEvent) {
        JSONArray jsonArray = new JSONArray();
        List<UserProfile> contacts = getContactsFinishedEvent.Contacts;
        for(UserProfile up : contacts) {
            jsonArray.put(up.toJSONObject());
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetContactsFinishedEvent", jsonArray.toString());
    }

    @Subscribe
    public void onGetContactsStartedEvent(GetContactsStartedEvent getContactsStartedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetContactsStartedEvent", "");
    }

    @Subscribe
    public void onGetFeedFailedEvent(GetFeedFailedEvent getFeedFailedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetFeedFailedEvent", getFeedFailedEvent.ErrorDescription);
    }

    @Subscribe
    public void onGetFeedFinishedEvent(GetFeedFinishedEvent getFeedFinishedEvent) {
        JSONArray jsonArray = new JSONArray();
        List<String> posts = getFeedFinishedEvent.Posts;
        for(String post : posts) {
            jsonArray.put(post);
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetFeedFinishedEvent", jsonArray.toString());
    }

    @Subscribe
    public void onGetFeedStartedEvent(GetFeedStartedEvent getFeedStartedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetFeedStartedEvent", "");
    }

    @Subscribe
    public void onRewardGivenEvent(RewardGivenEvent rewardGivenEvent) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("reward", rewardGivenEvent.Reward.toJSONObject());
            jsonObject.put("isBadge", rewardGivenEvent.Reward instanceof BadgeReward);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onRewardGivenEvent", jsonObject.toString());
    }
}
