//
// Copyright 2016 ArcTouch LLC.
// All rights reserved.
//
// This file, its contents, concepts, methods, behavior, and operation
// (collectively the "Software") are protected by trade secret, patent,
// and copyright laws. The use of the Software is governed by a license
// agreement. Disclosure of the Software to third parties, in any form,
// in whole or in part, is expressly prohibited except as authorized by
// the license agreement.
//
using System;

using Xamarin.Forms;

using AppServiceHelpers;
using AppServiceHelpers.Abstractions;
using Akavache;

namespace TranslationGame
{
	public class App : Application
	{
		public static IEasyMobileServiceClient Client;

		public IBlobCache LocalCache { get; private set; } = BlobCache.LocalMachine;

		public App()
		{
			Client = new EasyMobileServiceClient();
			Client.Initialize("http://xamarintranslation.azurewebsites.net");
			Client.RegisterTable<Word>();
			Client.FinalizeSchema();

			// The root page of your application
			MainPage = new PlayPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

