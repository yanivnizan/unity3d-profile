#import "SoomlaProfile.h"
#import "UserProfile.h"
#import "UserProfileStorage.h"
#import "SocialActionUtils.h"
#import "UserProfileEventHandling.h"
#import "UserProfileNotFoundException.h"
#import "ProviderNotFoundException.h"
#import "UnityProfileCommons.h"
#import "UnityCommons.h"
#import "Reward.h"
#import "SoomlaUtils.h"

extern "C"{
	
    void soomlaProfile_Initialize() {
        [SoomlaProfile usingExternalProvider:YES];
    }
    
	int soomlaProfile_GetStoredUserProfile(const char* sProvider, char** json) {
        NSLog(@"SOOMLA/UNITY soomlaProfile_GetStoredUserProfile");
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
		@try {
            Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
            UserProfile* userProfile = nil;
            if ([SoomlaProfile isUsingExternalProvider]) {
                NSLog(@"SOOMLA/UNITY isUsingExternalProvider[true]");
                userProfile = [UserProfileStorage getUserProfile:provider];
                NSLog(@"SOOMLA/UNITY userProfile:%@", [userProfile debugDescription]);
                if (!userProfile) {
                    return EXCEPTION_USER_PROFILE_NOT_FOUND;
                }
            }
            else {
                NSLog(@"SOOMLA/UNITY isUsingExternalProvider[false]");
                userProfile = [[SoomlaProfile getInstance] getStoredUserProfileWithProvider:provider];
            }
            
            NSDictionary* userDict = [userProfile toDictionary];
            NSLog(@"SOOMLA/UNITY userDict:%@", userDict);
            NSString* userProfileJson = [SoomlaUtils dictToJsonString:userDict];
            *json = Soom_AutonomousStringCopy([userProfileJson UTF8String]);
		}
		
		@catch (ProviderNotFoundException* e) {
            NSLog(@"SOOMLA/UNITY Couldn't find a Provider with providerId: %@.", providerIdS);
			return EXCEPTION_PROVIDER_NOT_FOUND;
        }
        @catch (UserProfileNotFoundException* e) {
            NSLog(@"SOOMLA/UNITY Couldn't find a UserProfile for providerId %@.", providerIdS);
			return EXCEPTION_USER_PROFILE_NOT_FOUND;
        }

		return NO_ERR;
	}
    
    void soomlaProfile_SetStoredUserProfile(const char* json, BOOL notify) {
        NSString* userProfileJson = [NSString stringWithUTF8String:json];
        UserProfile* userProfile = [[UserProfile alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:userProfileJson]];
        [UserProfileStorage setUserProfile:userProfile andNotify:notify];
	}
	   
    void soomlaProfile_OpenAppRatingPage() {
        [[SoomlaProfile getInstance] openAppRatingPage];
    }
    
}