package com.soomla.profile.unity;

import com.soomla.profile.SoomlaProfile;
import com.soomla.profile.data.UserProfileStorage;
import com.soomla.profile.domain.UserProfile;
import com.soomla.profile.exceptions.ProviderNotFoundException;
import com.soomla.profile.exceptions.UserProfileNotFoundException;

import org.json.JSONException;
import org.json.JSONObject;

import static com.soomla.profile.domain.IProvider.Provider;

public class UnitySoomlaProfile {

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

    private static String TAG = "SOOMLA UnitySoomlaProfile";
}
