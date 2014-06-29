#!/usr/bin/python

from mod_pbxproj import *
from os import path, listdir
from shutil import copytree
import sys

script_dir = os.path.dirname(sys.argv[0])
build_path = sys.argv[1]

print ("script_dir:{0}".format(script_dir))
print ("build_path:{0}".format(build_path))

frameworks = [
              'System/Library/Frameworks/Security.framework',
              'usr/lib/libsqlite3.0.dylib',
              'System/Library/Frameworks/StoreKit.framework'
              ]

weak_frameworks = [

]

# hopefully build_tools/../../../[Soomla]/Assets/Plugins/iOS
fb_framework_dir = path.join(script_dir,'..','..','..','Plugins','iOS')
fb_framework = path.join(fb_framework_dir, 'FacebookSDK.framework')

print ("fb_framework:{0}".format(fb_framework))

pbx_file_path = sys.argv[1] + '/Unity-iPhone.xcodeproj/project.pbxproj'
pbx_object = XcodeProject.Load(pbx_file_path)

pbx_object.add_framework_search_paths([path.abspath(fb_framework_dir)])
#pbx_object.add_header_search_paths([path.abspath(fb_framework)])
pbx_object.add_file(path.abspath(fb_framework), tree='SOURCE_ROOT', weak=True)

for framework in frameworks:
    pbx_object.add_file(framework, tree='SDKROOT')

for framework in weak_frameworks:
    pbx_object.add_file(framework, tree='SDKROOT', weak=True)

pbx_object.add_other_ldflags('-ObjC')

pbx_object.save()

# install Facebook plist settings
fb_app_id = '6037895039' # todo: read from Unity settings panel
info_plist_path = os.path.join(build_path, 'Info.plist')

elements_to_add = '''
    <key>FacebookAppId</key>
    <string>%s</string>
    <key>CFBundleURLTypes</key>
    <array>
    <dict>
    <key>CFBundleURLSchemes</key>
    <array>
    <string>fb%s</string>
    </array>
    </dict>
    </array>
    <key>CFBundleLocalizations</key>
    <array>
    <string>en</string>
    <string>ja</string>
    </array>
    ''' % (fb_app_id, fb_app_id)

with open(info_plist_path, "r") as in_file:
    plist = in_file.read()
    
print("plist:{0}".format(plist))
new_plist = plist.replace('<key>', elements_to_add + '<key>', 1)
print("plist-new:{0}".format(new_plist))
    
with open(info_plist_path, "w") as out_file:
    out_file.write(new_plist)
