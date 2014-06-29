package com.soomla.profile.unity;

import com.soomla.profile.SoomlaProfile;
import com.soomla.profile.domain.IProvider;
import com.soomla.profile.domain.UserProfile;
import com.soomla.profile.exceptions.ProviderNotFoundException;
import com.soomla.profile.exceptions.UserProfileNotFoundException;
import com.soomla.rewards.Reward;
import com.unity3d.player.UnityPlayer;

public class UnitySoomlaProfile {

    public static void updateStatus(String providerStr, final String status, String rewardJSON) throws ProviderNotFoundException{
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        Reward reward = Reward.fromJSONString(rewardJSON);

        SoomlaProfile.getInstance().updateStatus(provider, status, reward);
    }

    public static void updateStory(String providerStr, String message, String name,
                                   String caption, String description, String link,
                                   String pictureUrl, String rewardJSON) throws ProviderNotFoundException{
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        Reward reward = Reward.fromJSONString(rewardJSON);

        SoomlaProfile.getInstance().updateStory(provider, message, name, caption, description, link, pictureUrl, reward);
    }

    public static void getContacts(String providerStr, String rewardJSON) throws ProviderNotFoundException{
        IProvider.Provider provider = IProvider.Provider.getEnum(providerStr);
        Reward reward = Reward.fromJSONString(rewardJSON);

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
        Reward reward = Reward.fromJSONString(rewardJSON);

        SoomlaProfile.getInstance().login(UnityPlayer.currentActivity, provider, reward);
    }

    private static String TAG = "SOOMLA UnitySoomlaProfile";
}
