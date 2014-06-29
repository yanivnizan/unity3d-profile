
#import <Foundation/Foundation.h>

@interface UnityProfileEventDispatcher : NSObject{
    
}
- (id)init;
- (void)handleEvent:(NSNotification*)notification;
+ (void)initialize;

@end
