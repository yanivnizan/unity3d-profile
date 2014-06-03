package com.soomla.profile.unity;

import android.text.TextUtils;

import com.soomla.blueprint.rewards.BadgeReward;
import com.soomla.blueprint.rewards.Reward;
import com.soomla.profile.SoomlaProfile;
import com.soomla.profile.domain.IProvider;
import com.soomla.profile.domain.UserProfile;
import com.soomla.profile.exceptions.ProviderNotFoundException;
import com.soomla.profile.exceptions.UserProfileNotFoundException;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

public class UnitySoomlaProfile {

    public static void updateStatus(String providerStr, final String status, String rewardJSON) throws ProviderNotFoundException{
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        try {
            JSONObject rewardObj = new JSONObject(rewardJSON);
            Reward reward = new BadgeReward(rewardObj);

            SoomlaProfile.getInstance().updateStatus(UnityPlayer.currentActivity, provider, status, reward);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public static UserProfile getUserProfileLocally(String providerStr) throws ProviderNotFoundException, UserProfileNotFoundException {
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        return SoomlaProfile.getInstance().getUserProfileLocally(provider);
    }

    public static void logout(String providerStr) throws ProviderNotFoundException{
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        SoomlaProfile.getInstance().logout(provider);
    }

    public static void login(String providerStr, String rewardJSON) throws ProviderNotFoundException{
        final IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        try {
            Reward reward = null;
            if (!TextUtils.isEmpty(rewardJSON)) {
                JSONObject rewardObj = new JSONObject(rewardJSON);
                reward = new BadgeReward(rewardObj);
            }

            SoomlaProfile.getInstance().login(UnityPlayer.currentActivity, provider, reward);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    private static String TAG = "SOOMLA UnitySoomlaProfile";
}
