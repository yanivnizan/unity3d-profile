package com.soomla.profile.unity;

import android.text.TextUtils;

import com.soomla.profile.SoomlaProfile;
import com.soomla.profile.data.BPJSONConsts;
import com.soomla.profile.domain.IProvider;
import com.soomla.profile.domain.UserProfile;
import com.soomla.profile.domain.rewards.BadgeReward;
import com.soomla.profile.domain.rewards.RandomReward;
import com.soomla.profile.domain.rewards.Reward;
import com.soomla.profile.domain.rewards.SequenceReward;
import com.soomla.profile.domain.rewards.VirtualItemReward;
import com.soomla.profile.exceptions.ProviderNotFoundException;
import com.soomla.profile.exceptions.UserProfileNotFoundException;
import com.soomla.store.StoreUtils;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

public class UnitySoomlaProfile {

    private static Reward rewardFromJSON(String rewardJSON) {
        Reward reward = null;
        try {
            if (!TextUtils.isEmpty(rewardJSON)) {
                JSONObject rewardObj = new JSONObject(rewardJSON);
                String type = rewardObj.getString(BPJSONConsts.BP_TYPE);
                if (type.equals("badge")) {
                    reward = new BadgeReward(rewardObj);
                } else if (type.equals("item")) {
                    reward = new VirtualItemReward(rewardObj);
                } else if (type.equals("random")) {
                    reward = new RandomReward(rewardObj);
                } else if (type.equals("sequence")) {
                    reward = new SequenceReward(rewardObj);
                } else {
                    StoreUtils.LogError(TAG, "Unknown reward type: " + type);
                }
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return reward;
    }

    public static void updateStatus(String providerStr, final String status, String rewardJSON) throws ProviderNotFoundException{
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        Reward reward = rewardFromJSON(rewardJSON);

        SoomlaProfile.getInstance().updateStatus(provider, status, reward);
    }

    public static void updateStory(String providerStr, String message, String name,
                                   String caption, String description, String link,
                                   String pictureUrl, String rewardJSON) throws ProviderNotFoundException{
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        Reward reward = rewardFromJSON(rewardJSON);

        SoomlaProfile.getInstance().updateStory(provider, message, name, caption, description, link, pictureUrl, reward);
    }

    public static void getContacts(String providerStr, String rewardJSON) throws ProviderNotFoundException{
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        Reward reward = rewardFromJSON(rewardJSON);

        SoomlaProfile.getInstance().getContacts(provider, reward);
    }

    public static UserProfile getStoredUserProfile(String providerStr) throws ProviderNotFoundException, UserProfileNotFoundException {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        return SoomlaProfile.getInstance().getStoredUserProfile(provider);
    }

    public static void logout(String providerStr) throws ProviderNotFoundException{
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        SoomlaProfile.getInstance().logout(provider);
    }

    public static void login(String providerStr, String rewardJSON) throws ProviderNotFoundException{
        final IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        Reward reward = rewardFromJSON(rewardJSON);

        SoomlaProfile.getInstance().login(UnityPlayer.currentActivity, provider, reward);
    }

    private static String TAG = "SOOMLA UnitySoomlaProfile";
}
