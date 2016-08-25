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
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

using Newtonsoft.Json.Linq;

namespace TranslationGame
{
	public class TranslateService
	{
		private readonly string _clientId = "Xamarin_TranslationGame";
		private readonly string _clientSecret = "zZ3K4A+mXmju2ClymKe6yYpDr537Xc6y55Nwrsl/hmE=";

		private readonly Uri _dataMarketUri = new Uri("https://datamarket.accesscontrol.windows.net/v2/OAuth2-13");

		private readonly HttpClient _client = new HttpClient();

		public async Task<string> TranslateString(string strSource, string language)
		{
			string auth = await GetAzureDataMarketToken();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
			var requestUri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" +
				System.Net.WebUtility.UrlEncode(strSource) +
				"&to=" + language;
			string strTransText = string.Empty;
			try
			{
				var strTranslated = await _client.GetStringAsync(requestUri);
				var xTranslation = XDocument.Parse(strTranslated);
				strTransText = xTranslation.Root?.FirstNode.ToString();
				if (strTransText == strSource)
					return "";
				else
					return strTransText;
			}
			catch
			{
				// Supress errors
			}
			return strTransText;
		}

		private async Task<string> GetAzureDataMarketToken()
		{
			var properties = new Dictionary<string, string>
			{
				{ "grant_type", "client_credentials" },
				{ "client_id",  _clientId},
				{ "client_secret", _clientSecret },
				{ "scope", "http://api.microsofttranslator.com" }
			};

			var authentication = new FormUrlEncodedContent(properties);
			var dataMarketResponse = await _client.PostAsync(_dataMarketUri, authentication);
			string response;
			if (!dataMarketResponse.IsSuccessStatusCode)
			{
				response = await dataMarketResponse.Content.ReadAsStringAsync();
				var error = Newtonsoft.Json.JsonConvert.DeserializeObject<JToken>(response);
				var err = error.Value<string>("error");
				var msg = error.Value<string>("error_description");
				throw new HttpRequestException($"Azure market place request failed: {err} {msg}");
			}
			response = await dataMarketResponse.Content.ReadAsStringAsync();
			var accessToken = Newtonsoft.Json.JsonConvert.DeserializeObject<DataMarketAccessToken>(response);
			return accessToken.access_token;
		}
	}
}