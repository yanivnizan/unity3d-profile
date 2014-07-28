package com.soomla.profile.unity;

import com.soomla.BusProvider;
import com.soomla.profile.SoomlaProfile;
import com.soomla.profile.data.UserProfileStorage;
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
import com.soomla.profile.exceptions.ProviderNotFoundException;
import com.soomla.profile.exceptions.UserProfileNotFoundException;
import com.soomla.rewards.Reward;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

import static com.soomla.profile.domain.IProvider.Provider;
import static com.soomla.profile.social.ISocialProvider.SocialActionType;

public class UnitySoomlaProfile {

    public static void updateStatus(String providerStr, final String status, String rewardJSON) throws ProviderNotFoundException{
        Provider provider = Provider.getEnum(providerStr);
        Reward reward = Reward.fromJSONString(rewardJSON);

        SoomlaProfile.getInstance().updateStatus(provider, status, reward);
    }

    public static void updateStory(String providerStr, String message, String name,
                                   String caption, String description, String link,
                                   String pictureUrl, String rewardJSON) throws ProviderNotFoundException{
        Provider provider = Provider.getEnum(providerStr);
        Reward reward = Reward.fromJSONString(rewardJSON);

        SoomlaProfile.getInstance().updateStory(provider, message, name, caption, description, link, pictureUrl, reward);
    }

    public static void getContacts(String providerStr, String rewardJSON) throws ProviderNotFoundException{
        Provider provider = Provider.getEnum(providerStr);
        Reward reward = Reward.fromJSONString(rewardJSON);

        SoomlaProfile.getInstance().getContacts(provider, reward);
    }

    public static void uploadImage(String providerStr, String message, String filePath, String rewardJSON) throws ProviderNotFoundException{
        Provider provider = Provider.getEnum(providerStr);
        Reward reward = Reward.fromJSONString(rewardJSON);

        SoomlaProfile.getInstance().uploadImage(provider, message, filePath, reward);
    }

    public static void getFeed(String providerStr, String rewardJSON) throws ProviderNotFoundException{
        Provider provider = Provider.getEnum(providerStr);
        Reward reward = Reward.fromJSONString(rewardJSON);

        SoomlaProfile.getInstance().getFeed(provider, reward);
    }

    public static String getStoredUserProfile(String providerStr) throws ProviderNotFoundException, UserProfileNotFoundException {
        Provider provider = Provider.getEnum(providerStr);
        UserProfile userProfile = SoomlaProfile.getInstance().getStoredUserProfile(provider);
        return userProfile == null ? null : userProfile.toJSONObject().toString();
    }

    public static void storeUserProfile(String userJSON)
            throws ProviderNotFoundException, JSONException {
        JSONObject jsonObject = new JSONObject(userJSON);
        final UserProfile userProfile = new UserProfile(jsonObject);

        UserProfileStorage.setUserProfile(userProfile);
    }

    public static void logout(String providerStr) throws ProviderNotFoundException{
        Provider provider = Provider.getEnum(providerStr);
        SoomlaProfile.getInstance().logout(provider);
    }

    public static void login(String providerStr, String rewardJSON) throws ProviderNotFoundException{
        final Provider provider = Provider.getEnum(providerStr);
        Reward reward = Reward.fromJSONString(rewardJSON);

        SoomlaProfile.getInstance().login(UnityPlayer.currentActivity, provider, reward);
    }


    // events pushed from external provider (Unity FB SDK etc.)

    public static void pushEventLoginStarted(String providerStr) {
        Provider provider = Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LoginStartedEvent(provider));
    }

    public static void pushEventLoginFinished(String userProfileJSON) throws Exception {
        JSONObject jsonObject = new JSONObject(userProfileJSON);
        UserProfile userProfile = new UserProfile(jsonObject);
        BusProvider.getInstance().post(new LoginFinishedEvent(userProfile));
    }

    public static void pushEventLoginFailed(String providerStr, String message) {
        Provider provider = Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LoginFailedEvent(provider, message));
    }

    public static void pushEventLoginCancelled(String providerStr) {
        Provider provider = Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LoginCancelledEvent(provider));
    }

    public static void pushEventLogoutStarted(String providerStr) {
        Provider provider = Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LogoutStartedEvent(provider));
    }

    public static void pushEventLogoutFinished(String providerStr) throws Exception {
        Provider provider = Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LogoutFinishedEvent(provider));
    }

    public static void pushEventLogoutFailed(String providerStr, String message) {
        Provider provider = Provider.getEnum(providerStr);
        BusProvider.getInstance().post(new LogoutFailedEvent(provider, message));
    }

    public static void pushEventSocialActionStarted(String providerStr, String actionTypeStr) {
        Provider provider = Provider.getEnum(providerStr);
        SocialActionType socialActionType = SocialActionType.getEnum(actionTypeStr);
        BusProvider.getInstance().post(new SocialActionStartedEvent(provider, socialActionType));
    }

    public static void pushEventSocialActionFinished(String providerStr, String actionTypeStr) {
        Provider provider = Provider.getEnum(providerStr);
        SocialActionType socialActionType = SocialActionType.getEnum(actionTypeStr);
        BusProvider.getInstance().post(new SocialActionFinishedEvent(provider, socialActionType));
    }

    public static void pushEventSocialActionCancelled(String providerStr, String actionTypeStr) {
        Provider provider = Provider.getEnum(providerStr);
        SocialActionType socialActionType = SocialActionType.getEnum(actionTypeStr);
        BusProvider.getInstance().post(new SocialActionCancelledEvent(provider, socialActionType));
    }

    public static void pushEventSocialActionFailed(String providerStr, String actionTypeStr, String message) {
        Provider provider = Provider.getEnum(providerStr);
        SocialActionType socialActionType = SocialActionType.getEnum(actionTypeStr);
        BusProvider.getInstance().post(new SocialActionFailedEvent(provider, socialActionType, message));
    }

    private static String TAG = "SOOMLA UnitySoomlaProfile";
}
