/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using System;
namespace Soomla.Profile
{
	/// <summary>
	/// A string enumeration of social providers.
	/// </summary>
	public sealed class Provider
	{
		private readonly string name;

		public static readonly Provider FACEBOOK = new Provider ("facebook");
		public static readonly Provider FOURSQUARE = new Provider ("foursquare");
		public static readonly Provider GOOGLE = new Provider ("google");
		public static readonly Provider LINKEDIN = new Provider ("linkedin");
		public static readonly Provider MYSPACE = new Provider ("myspace");
		public static readonly Provider TWITTER = new Provider ("twitter");
		public static readonly Provider YAHOO = new Provider ("yahoo");
		public static readonly Provider SALESFORCE = new Provider ("salesforce");
		public static readonly Provider YAMMER = new Provider ("yammer");
		public static readonly Provider RUNKEEPER = new Provider ("runkeeper");
		public static readonly Provider INSTAGRAM = new Provider ("instagram");
		public static readonly Provider FLICKR = new Provider ("flickr");
		
		private Provider(string name){
			this.name = name;
		}
		
		public override string ToString(){
			return name;
		}

		public static Provider fromString(string providerStr) {
			switch(providerStr) {
			case("facebook"):
				return FACEBOOK;
			case("foursquare"):
				return FOURSQUARE;
			case("google"):
				return GOOGLE;
			case("linkedin"):
				return LINKEDIN;
			case("myspace"):
				return MYSPACE;
			case("twitter"):
				return TWITTER;
			case("yahoo"):
				return YAHOO;
			case("salesforce"):
				return SALESFORCE;
			case("yammer"):
				return YAMMER;
			case("runkeeper"):
				return RUNKEEPER;
			case("instagram"):
				return INSTAGRAM;
			case("flickr"):
				return FLICKR;
			default:
				return null;
			}
		}
	}
}

