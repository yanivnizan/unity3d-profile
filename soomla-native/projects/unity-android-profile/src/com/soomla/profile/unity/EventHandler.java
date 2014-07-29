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
import com.soomla.profile.events.social.SocialActionCancelledEvent;
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
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLoginCancelled", loginCancelledEvent.Provider.toString());
    }

    @Subscribe
    public void onUserProfileUpdated(UserProfileUpdatedEvent userProfileUpdatedEvent) {
        UserProfile userProfile = userProfileUpdatedEvent.UserProfile;
        UnityPlayer.UnitySendMessage("ProfileEvents", "onUserProfileUpdated", userProfile.toJSONObject().toString());
    }

    @Subscribe
    public void onLoginFailed(LoginFailedEvent loginFailedEvent) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", loginFailedEvent.Provider.toString());
            jsonObject.put("errMsg", loginFailedEvent.ErrorDescription);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        UnityPlayer.UnitySendMessage("ProfileEvents", "onLoginFailed", jsonObject.toString());
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
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", logoutFailedEvent.Provider.toString());
            jsonObject.put("errMsg", logoutFailedEvent.ErrorDescription);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        UnityPlayer.UnitySendMessage("ProfileEvents", "onLogoutFailed", jsonObject.toString());
    }

    @Subscribe
    public void onLogoutFinished(LogoutFinishedEvent logoutFinishedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLogoutFinished", logoutFinishedEvent.Provider.toString());
    }

    @Subscribe
    public void onLogoutStarted(LogoutStartedEvent logoutStartedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onLogoutStarted", logoutStartedEvent.Provider.toString());
    }

    @Subscribe
    public void onSocialActionFailed(SocialActionFailedEvent socialActionFailedEvent) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", socialActionFailedEvent.Provider.toString());
            jsonObject.put("socialActionType", socialActionFailedEvent.SocialActionType.toString());
            jsonObject.put("errMsg", socialActionFailedEvent.ErrorDescription);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onSocialActionFailed", jsonObject.toString());
    }

    @Subscribe
    public void onSocialActionFinished(SocialActionFinishedEvent socialActionFinishedEvent) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", socialActionFinishedEvent.Provider.toString());
            jsonObject.put("socialActionType", socialActionFinishedEvent.SocialActionType.toString());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onSocialActionFinished", jsonObject.toString());
    }

    @Subscribe
    public void onSocialActionStarted(SocialActionStartedEvent socialActionStartedEvent) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", socialActionStartedEvent.Provider.toString());
            jsonObject.put("socialActionType", socialActionStartedEvent.SocialActionType.toString());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onSocialActionStarted", jsonObject.toString());
    }

    @Subscribe
    public void onSocialActionCancelled(SocialActionCancelledEvent socialActionCancelledEvent) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", socialActionCancelledEvent.Provider.toString());
            jsonObject.put("socialActionType", socialActionCancelledEvent.SocialActionType.toString());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onSocialActionCancelled", jsonObject.toString());
    }

    @Subscribe
    public void onGetContactsFailedEvent(GetContactsFailedEvent getContactsFailedEvent) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", getContactsFailedEvent.Provider.toString());
            jsonObject.put("errMsg", getContactsFailedEvent.ErrorDescription);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetContactsFailed", jsonObject.toString());
    }

    @Subscribe
    public void onGetContactsFinishedEvent(GetContactsFinishedEvent getContactsFinishedEvent) {
        JSONArray jsonArray = new JSONArray();
        List<UserProfile> contacts = getContactsFinishedEvent.Contacts;
        for(UserProfile up : contacts) {
            jsonArray.put(up.toJSONObject());
        }

        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", getContactsFinishedEvent.Provider.toString());
            jsonObject.put("contacts", contacts);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetContactsFinished", jsonObject.toString());
    }

    @Subscribe
    public void onGetContactsStartedEvent(GetContactsStartedEvent getContactsStartedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetContactsStarted", getContactsStartedEvent.Provider.toString());
    }

    @Subscribe
    public void onGetFeedFailedEvent(GetFeedFailedEvent getFeedFailedEvent) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", getFeedFailedEvent.Provider.toString());
            jsonObject.put("errMsg", getFeedFailedEvent.ErrorDescription);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetFeedFailed", jsonObject.toString());
    }

    @Subscribe
    public void onGetFeedFinishedEvent(GetFeedFinishedEvent getFeedFinishedEvent) {
        JSONArray jsonArray = new JSONArray();
        List<String> posts = getFeedFinishedEvent.Posts;
        for(String post : posts) {
            jsonArray.put(post);
        }

        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("provider", getFeedFinishedEvent.Provider.toString());
            jsonObject.put("posts", posts);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetFeedFinished", jsonObject.toString());
    }

    @Subscribe
    public void onGetFeedStartedEvent(GetFeedStartedEvent getFeedStartedEvent) {
        UnityPlayer.UnitySendMessage("ProfileEvents", "onGetFeedStartedEvent", getFeedStartedEvent.Provider.toString());
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
        UnityPlayer.UnitySendMessage("ProfileEvents", "onRewardGiven", jsonObject.toString());
    }
}
