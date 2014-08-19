
#import "UnityProfileEventDispatcher.h"
#import "SoomlaEventHandling.h"
#import "UserProfileEventHandling.h"
#import "UserProfile.h"
#import "SocialActionUtils.h"
#import "Reward.h"
#import "SoomlaUtils.h"

extern "C"{
    
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

@implementation UnityProfileEventDispatcher

- (id) init {
    if (self = [super init]) {
        [UserProfileEventHandling observeAllEventsWithObserver:self withSelector:@selector(handleEvent:)];
    }
    
    return self;
}

@end
