
#import "UnityProfileEventDispatcher.h"
#import "SoomlaEventHandling.h"
#import "UserProfileEventHandling.h"
#import "UserProfile.h"
#import "SocialActionUtils.h"
#import "Reward.h"
#import "SoomlaUtils.h"

@implementation UnityProfileEventDispatcher

+ (void)initialize {
    static UnityProfileEventDispatcher* instance = nil;
    if (!instance) {
        instance = [[UnityProfileEventDispatcher alloc] init];
    }
}

- (id) init {
    if (self = [super init]) {
        [UserProfileEventHandling observeAllEventsWithObserver:self withSelector:@selector(handleEvent:)];
    }
    
    return self;
}

- (void)handleEvent:(NSNotification*)notification{
    
//    [[NSNotificationCenter defaultCenter] addObserver:observer selector:selector name:EVENT_BP_REWARD_GIVEN object:nil];
    
    // LOGIN
	if ([notification.name isEqualToString:EVENT_UP_LOGIN_STARTED]) {
        NSDictionary* userInfo = [notification userInfo];
        Provider provider = (Provider)[[userInfo objectForKey:DICT_ELEMENT_PROVIDER] intValue];
        NSString* providerId = [UserProfileUtils providerEnumToString:provider];
        UnitySendMessage("ProfileEvents", "onLoginStarted", [providerId UTF8String]);
	}
	else if ([notification.name isEqualToString:EVENT_UP_LOGIN_FINISHED]) {
        NSDictionary* userInfo = [notification userInfo];
//        Provider provider = (Provider)[[userInfo objectForKey:DICT_ELEMENT_PROVIDER] intValue];
//        NSString* providerId = [UserProfileUtils providerEnumToString:provider];
        UserProfile* userProfile = [userInfo objectForKey:DICT_ELEMENT_USER_PROFILE];
        NSString* userProfileJson = [SoomlaUtils dictToJsonString:[userProfile toDictionary]];
	    UnitySendMessage("ProfileEvents", "onLoginFinished", [userProfileJson UTF8String]);
	}
	else if ([notification.name isEqualToString:EVENT_UP_LOGIN_CANCELLED]) {
        NSDictionary* userInfo = [notification userInfo];
        Provider provider = (Provider)[[userInfo objectForKey:DICT_ELEMENT_PROVIDER] intValue];
        NSString* providerId = [UserProfileUtils providerEnumToString:provider];
	    UnitySendMessage("ProfileEvents", "onLoginCancelled", [providerId UTF8String]);
	}
    else if ([notification.name isEqualToString:EVENT_UP_LOGIN_FAILED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* msg = [userInfo objectForKey:DICT_ELEMENT_MESSAGE];
//        Provider provider = (Provider)[[userInfo objectForKey:DICT_ELEMENT_PROVIDER] intValue];
//        NSString* providerId = [UserProfileUtils providerEnumToString:provider];
	    UnitySendMessage("ProfileEvents", "onLoginFailed", [msg UTF8String]);
	}
    // LOGOUT
    else if ([notification.name isEqualToString:EVENT_UP_LOGOUT_STARTED]) {
        NSDictionary* userInfo = [notification userInfo];
        Provider provider = (Provider)[[userInfo objectForKey:DICT_ELEMENT_PROVIDER] intValue];
        NSString* providerId = [UserProfileUtils providerEnumToString:provider];
        UnitySendMessage("ProfileEvents", "onLogoutStarted", [providerId UTF8String]);
	}
	else if ([notification.name isEqualToString:EVENT_UP_LOGOUT_FINISHED]) {
        NSDictionary* userInfo = [notification userInfo];
        UserProfile* userProfile = [userInfo objectForKey:DICT_ELEMENT_USER_PROFILE];
        NSString* userProfileJson = [SoomlaUtils dictToJsonString:[userProfile toDictionary]];
	    UnitySendMessage("ProfileEvents", "onLogoutFinished", [userProfileJson UTF8String]);
	}
    else if ([notification.name isEqualToString:EVENT_UP_LOGOUT_FAILED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* msg = [userInfo objectForKey:DICT_ELEMENT_MESSAGE];
        //        Provider provider = (Provider)[[userInfo objectForKey:DICT_ELEMENT_PROVIDER] intValue];
        //        NSString* providerId = [UserProfileUtils providerEnumToString:provider];
	    UnitySendMessage("ProfileEvents", "onLogoutFailed", [msg UTF8String]);
	}
    // USER_PROFILE_UPDATED
    else if ([notification.name isEqualToString:EVENT_UP_USER_PROFILE_UPDATED]) {
        NSDictionary* userInfo = [notification userInfo];
        UserProfile* userProfile = [userInfo objectForKey:DICT_ELEMENT_USER_PROFILE];
        NSString* userProfileJson = [SoomlaUtils dictToJsonString:[userProfile toDictionary]];
	    UnitySendMessage("ProfileEvents", "onUserProfileUpdated", [userProfileJson UTF8String]);
	}
    // SOCIAL_ACTION
    else if ([notification.name isEqualToString:EVENT_UP_SOCIAL_ACTION_STARTED]) {
        NSDictionary* userInfo = [notification userInfo];
        SocialActionType socialActionType = (SocialActionType)[[userInfo objectForKey:DICT_ELEMENT_SOCIAL_ACTION_TYPE] intValue];
        NSString* actionName = [SocialActionUtils actionEnumToString:socialActionType];
        UnitySendMessage("ProfileEvents", "onSocialActionStarted", [actionName UTF8String]);
	}
	else if ([notification.name isEqualToString:EVENT_UP_SOCIAL_ACTION_FINISHED]) {
        NSDictionary* userInfo = [notification userInfo];
        SocialActionType socialActionType = (SocialActionType)[[userInfo objectForKey:DICT_ELEMENT_SOCIAL_ACTION_TYPE] intValue];
        NSString* actionName = [SocialActionUtils actionEnumToString:socialActionType];
        UnitySendMessage("ProfileEvents", "onSocialActionFinished", [actionName UTF8String]);
	}
    else if ([notification.name isEqualToString:EVENT_UP_SOCIAL_ACTION_FAILED]) {
        NSDictionary* userInfo = [notification userInfo];
        SocialActionType socialActionType = (SocialActionType)[[userInfo objectForKey:DICT_ELEMENT_SOCIAL_ACTION_TYPE] intValue];
        NSString* actionName = [SocialActionUtils actionEnumToString:socialActionType];
        NSString* msg = [userInfo objectForKey:DICT_ELEMENT_MESSAGE];
        NSDictionary* args = @{@"socialActionType":actionName, @"errMsg":msg};
	    UnitySendMessage("ProfileEvents", "onSocialActionFailed", [[SoomlaUtils dictToJsonString:args] UTF8String]);
	}
    // GET_CONTACTS
    else if ([notification.name isEqualToString:EVENT_UP_GET_CONTACTS_STARTED]) {
        UnitySendMessage("ProfileEvents", "onGetContactsStarted", "");
	}
	else if ([notification.name isEqualToString:EVENT_UP_GET_CONTACTS_FINISHED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSArray* contacts = [userInfo objectForKey:DICT_ELEMENT_CONTACTS];
        NSString* contactsJson = [SoomlaUtils arrayToJsonString:contacts];
        UnitySendMessage("ProfileEvents", "onGetContactsFinished", [contactsJson UTF8String]);
	}
    else if ([notification.name isEqualToString:EVENT_UP_GET_CONTACTS_FAILED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* msg = [userInfo objectForKey:DICT_ELEMENT_MESSAGE];
	    UnitySendMessage("ProfileEvents", "onGetContactsFailed", [msg UTF8String]);
	}
}

@end
