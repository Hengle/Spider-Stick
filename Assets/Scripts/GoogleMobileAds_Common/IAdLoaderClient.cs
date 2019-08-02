// class: GoogleMobileAds.Common.IAdLoaderClient
using GoogleMobileAds.Api;
using System;

namespace GoogleMobileAds.Common
{
	public interface IAdLoaderClient
	{
		event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

		void LoadAd(AdRequest request);
	}
}
