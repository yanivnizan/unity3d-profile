#import "SoomlaProfile.h"
#import "UserProfile.h"
#import "UserProfileStorage.h"
#import "SocialActionUtils.h"
#import "UserProfileEventHandling.h"
#import "UserProfileNotFoundException.h"
#import "ProviderNotFoundException.h"
#import "UnityCommons.h"
#import "Reward.h"
#import "SoomlaUtils.h"

char* AutonomousStringCopy (const char* string)
{
    if (string == NULL)
       return NULL;

    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C"{
	
    void soomlaProfile_Initialize(bool usingUnityProvider) {
        [SoomlaProfile usingExternalProvider:usingUnityProvider];
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
            *json = AutonomousStringCopy([userProfileJson UTF8String]);
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
	
	void soomlaProfile_Login(const char* sProvider, const char* sRewardJson) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        Reward* reward = nil;
        if(sRewardJson) {
            NSString* rewardJson = [NSString stringWithUTF8String:sRewardJson];
            reward = [[Reward alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:rewardJson]];
        }
        [[SoomlaProfile getInstance] loginWithProvider:provider andReward:reward];
	}
	
	void soomlaProfile_Logout(const char* sProvider) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        [[SoomlaProfile getInstance] logoutWithProvider:provider];
    }
	
	void soomlaProfile_UpdateStatus(const char* sProvider, const char* sStatus, const char* sRewardJson) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString* status = [NSString stringWithUTF8String:sStatus];
        Reward* reward = nil;
        if(sRewardJson) {
            NSString* rewardJson = [NSString stringWithUTF8String:sRewardJson];
            reward = [[Reward alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:rewardJson]];
        }
        [[SoomlaProfile getInstance] updateStatusWithProvider:provider andStatus:status andReward:reward];
	}
    
    void soomlaProfile_UpdateStory(const char* sProvider, const char* sStatus,
                                   const char* sMsg,
                                   const char* sName,
                                   const char* sCaption,
                                   const char* sDesc,
                                   const char* sLink,
                                   const char* sPicUrl,
                                   const char* sRewardJson) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString* msg = [NSString stringWithUTF8String:sMsg];
        NSString* name = [NSString stringWithUTF8String:sName];
        NSString* caption = [NSString stringWithUTF8String:sCaption];
        NSString* desc = [NSString stringWithUTF8String:sDesc];
        NSString* link = [NSString stringWithUTF8String:sLink];
        NSString* picUrl = [NSString stringWithUTF8String:sPicUrl];
        Reward* reward = nil;
        if(sRewardJson) {
            NSString* rewardJson = [NSString stringWithUTF8String:sRewardJson];
            reward = [[Reward alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:rewardJson]];
        }
        [[SoomlaProfile getInstance] updateStoryWithProvider:provider
                                                  andMessage:msg
                                                     andName:name
                                                  andCaption:caption
                                              andDescription:desc
                                                     andLink:link
                                                  andPicture:picUrl
                                                   andReward:reward];
	}
    
    void soomlaProfile_UploadImage(const char* sProvider, const char* sMsg, const char* sFilePath, const char* sRewardJson) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString* msg = [NSString stringWithUTF8String:sMsg];
        NSString* filePath = [NSString stringWithUTF8String:sFilePath];
        Reward* reward = nil;
        if(sRewardJson) {
            NSString* rewardJson = [NSString stringWithUTF8String:sRewardJson];
            reward = [[Reward alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:rewardJson]];
        }
        [[SoomlaProfile getInstance] uploadImageWithProvider:provider andMessage:msg andFilePath:filePath andReward:reward];
    }
    
    void soomlaProfile_GetContacts(const char* sProvider, const char* sRewardJson) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        Reward* reward = nil;
        if(sRewardJson) {
            NSString* rewardJson = [NSString stringWithUTF8String:sRewardJson];
            reward = [[Reward alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:rewardJson]];
        }
        [[SoomlaProfile getInstance] getContactsWithProvider:provider andReward:reward];
    }
    
    void soomlaProfile_GetFeed(const char* sProvider, const char* sRewardJson) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        Reward* reward = nil;
        if(sRewardJson) {
            NSString* rewardJson = [NSString stringWithUTF8String:sRewardJson];
            reward = [[Reward alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:rewardJson]];
        }
        [[SoomlaProfile getInstance] getFeedWithProvider:provider andReward:reward];
    }
    
    void soomlaProfile_OpenAppRatingPage() {
        [[SoomlaProfile getInstance] openAppRatingPage];
    }
    
    // events pushed from external provider (Unity FB SDK etc.)
    
    void soomlaProfile_PushEventLoginStarted (const char* sProvider) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        [UserProfileEventHandling postLoginStarted:provider];
    }
    void soomlaProfile_PushEventLoginFinished(const char* sUserProfileJson) {
        NSString *userProfileJson = [NSString stringWithUTF8String:sUserProfileJson];
        UserProfile* userProfile = [[UserProfile alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:userProfileJson]];
        [UserProfileEventHandling postLoginFinished:userProfile];
    }
    void soomlaProfile_PushEventLoginFailed(const char* sProvider, const char* sMessage) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString *message = [NSString stringWithUTF8String:sMessage];
        [UserProfileEventHandling postLoginFailed:provider withMessage:message];
    }
    void soomlaProfile_PushEventLoginCancelled(const char* sProvider) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        [UserProfileEventHandling postLoginCancelled:provider];
    }
    void soomlaProfile_PushEventLogoutStarted(const char* sProvider) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        [UserProfileEventHandling postLoginStarted:provider];
    }
    void soomlaProfile_PushEventLogoutFinished(const char* sProvider) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        [UserProfileEventHandling postLogoutFinished:provider];
    }
    void soomlaProfile_PushEventLogoutFailed(const char* sProvider, const char* sMessage) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString *message = [NSString stringWithUTF8String:sMessage];
        [UserProfileEventHandling postLogoutFailed:provider withMessage:message];
    }
    void soomlaProfile_PushEventSocialActionStarted(const char* sProvider, const char* sActionType) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString* actionType = [NSString stringWithUTF8String:sActionType];
        SocialActionType socialActionType = [SocialActionUtils actionStringToEnum:actionType];
        [UserProfileEventHandling postSocialActionStarted:provider withType:socialActionType];
    }
    void soomlaProfile_PushEventSocialActionFinished(const char* sProvider, const char* sActionType) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString* actionType = [NSString stringWithUTF8String:sActionType];
        SocialActionType socialActionType = [SocialActionUtils actionStringToEnum:actionType];
        [UserProfileEventHandling postSocialActionFinished:provider withType:socialActionType];
    }
    void soomlaProfile_PushEventSocialActionCancelled(const char* sProvider, const char* sActionType) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString* actionType = [NSString stringWithUTF8String:sActionType];
        SocialActionType socialActionType = [SocialActionUtils actionStringToEnum:actionType];
        [UserProfileEventHandling postSocialActionCancelled:provider withType:socialActionType];
    }
    void soomlaProfile_PushEventSocialActionFailed(const char* sProvider, const char* sActionType,  const char* sMessage) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString* actionType = [NSString stringWithUTF8String:sActionType];
        NSString *message = [NSString stringWithUTF8String:sMessage];
        SocialActionType socialActionType = [SocialActionUtils actionStringToEnum:actionType];
        [UserProfileEventHandling postSocialActionFailed:provider withType:socialActionType withMessage:message];
    }
    
    //    void pushEventGetContactsStarted(enum SocialActionType socialActionType) {
    //
    //    }
    //    void pushEventGetContactsFinished(enum SocialActionType socialActionType, const char** contacts) {
    //
    //    }
    //    void pushEventGetContactsFailed(enum SocialActionType socialActionType, const char* message) {
    //        
    //    }
}