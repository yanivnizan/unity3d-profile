#import "SoomlaProfile.h"
#import "UserProfile.h"
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
	
    void pushEventLoginStarted (const char* sProvider) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        [UserProfileEventHandling postLoginStarted:provider];
    }
    void pushEventLoginFinished(const char* sUserProfileJson) {
        NSString *userProfileJson = [NSString stringWithUTF8String:sUserProfileJson];
        UserProfile* userProfile = [[UserProfile alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:userProfileJson]];
        [UserProfileEventHandling postLoginFinished:userProfile];
    }
    void pushEventLoginFailed(const char* sMessage) {
        NSString *message = [NSString stringWithUTF8String:sMessage];
        [UserProfileEventHandling postLoginFailed:message];
    }
    void pushEventLoginCancelled() {
        [UserProfileEventHandling postLoginCancelled];
    }
    void pushEventLogoutStarted(const char* sProvider) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        [UserProfileEventHandling postLoginStarted:provider];
    }
    void pushEventLogoutFinished(const char* sUserProfileJson) {
        NSString *userProfileJson = [NSString stringWithUTF8String:sUserProfileJson];
        UserProfile* userProfile = [[UserProfile alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:userProfileJson]];
        [UserProfileEventHandling postLogoutFinished:userProfile];
    }
    void pushEventLogoutFailed(const char* sMessage) {
        NSString *message = [NSString stringWithUTF8String:sMessage];
        [UserProfileEventHandling postLogoutFailed:message];
    }
    void pushEventSocialActionStarted(const char* sProvider, const char* sActionType) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString* actionType = [NSString stringWithUTF8String:sActionType];
        SocialActionType socialActionType = [SocialActionUtils actionStringToEnum:actionType];
        [UserProfileEventHandling postSocialActionStarted:provider withType:socialActionType];
    }
    void pushEventSocialActionFinished(const char* sProvider, const char* sActionType) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
        NSString* actionType = [NSString stringWithUTF8String:sActionType];
        SocialActionType socialActionType = [SocialActionUtils actionStringToEnum:actionType];
        [UserProfileEventHandling postSocialActionFinished:provider withType:socialActionType];
    }
    void pushEventSocialActionFailed(const char* sProvider, const char* sActionType,  const char* sMessage) {
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
    
    
	int soomlaProfile_GetStoredUserProfile(const char* sProvider, char** json) {
        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
		@try {
            Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
            UserProfile* userProfile = [[SoomlaProfile getInstance] getStoredUserProfileWithProvider:provider];
            *json = AutonomousStringCopy([[SoomlaUtils dictToJsonString:[userProfile toDictionary]] UTF8String]);
		}
		
		@catch (ProviderNotFoundException* e) {
            NSLog(@"Couldn't find a Provider with providerId: %@.", providerIdS);
			return EXCEPTION_PROVIDER_NOT_FOUND;
        }
        @catch (UserProfileNotFoundException* e) {
            NSLog(@"Couldn't find a UserProfile for providerId %@.", providerIdS);
			return EXCEPTION_USER_PROFILE_NOT_FOUND;
        }

		return NO_ERR;
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
    
    void soomlaProfile_GetFeeds(const char* sProvider, const char* sRewardJson) {
//        NSString* providerIdS = [NSString stringWithUTF8String:sProvider];
//        Provider provider = [UserProfileUtils providerStringToEnum:providerIdS];
//        Reward* reward = nil;
//        if(sRewardJson) {
//            NSString* rewardJson = [NSString stringWithUTF8String:sRewardJson];
//            reward = [[Reward alloc] initWithDictionary:[SoomlaUtils jsonStringToDict:rewardJson]];
//        }
//        [[SoomlaProfile getInstance] getFeedsWithProvider:provider andReward:reward];
    }
}