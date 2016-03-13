using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text; 
using System.Text.RegularExpressions;
using System.Diagnostics;

using Medelinked.Core.Response;
using Medelinked.Core.Request;

namespace Medelinked.Core.Client
{
	public static class MedelinkedProviderHttpClient
	{
		//CookieContainer as we need the authentication cookies to be carried across all requests
		private static CookieContainer AuthCookies;

		//Use the same instance of the client for all calls made
		private static HttpClient httpClient;

		//The provider key required for authentication
		private static string ProviderKey = "SampleProviderKey";

		//The core service URL
		private static string ServiceUrl = "https://app.medelinked.com"; 

		private static readonly Regex RxMsAjaxJsonInner = 
			new Regex("^{\\s*\"d\"\\s*:(.*)}$", RegexOptions.Compiled);

		private static readonly Regex RxMsAjaxJsonInnerType = 
			new Regex("\\s*\"__type\"\\s*:\\s*\"[^\"]*\"\\s*,\\s*", RegexOptions.Compiled);

		#region Login

		/// <summary>
		/// Connect as a Provider
		/// </summary>
		/// <param name="ProviderCredentials">Credentials.</param>
		public static async Task<HealthProviders> ConnectAsync(ProviderLoginModel ProviderCredentials)
		{
			try
			{
				
				AuthCookies = new CookieContainer();
				string postBody = "{\"ProviderCredentials\":" + JsonConvert.SerializeObject(ProviderCredentials) + "}"; 
				httpClient = new HttpClient (new HttpClientHandler {
					CookieContainer = AuthCookies, // Use a durable store for authentication cookies
					UseCookies = true
				});

				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/provider/connect", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					HealthProviders obj = JsonConvert.DeserializeObject<HealthProviders>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		
		/// <summary>
		/// Disconnect the session.
		/// </summary>
		public static async void Disconnect()
		{
			//Clear out the cookies
			AuthCookies = null;
			httpClient = null;
		}

		#endregion


		#region Add a Record

		/// <summary>
		/// Add a record
		/// </summary>
		/// <param name="newRecord">Record details</param>
		public static async Task<HealthData> CommitRecord(Record newRecord)
		{
			try
			{
				string postBody = "{\"RecordDetails\":" + JsonConvert.SerializeObject(newRecord) + "}";  
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/provider/addrecordtouseraccount", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					HealthData obj = JsonConvert.DeserializeObject<HealthData>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}
				
			return null;
		}

		/// <summary>
		/// Add a record
		/// </summary>
		/// <param name="newRecord">Record details</param>
		public static async Task<HealthData> CommitRecords(List<Record> newRecords)
		{
			try
			{
				string postBody = "{\"RecordDetails\":" + JsonConvert.SerializeObject(newRecords) + "}";  
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/provider/addrecordstouseraccount", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					HealthData obj = JsonConvert.DeserializeObject<HealthData>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}

			return null;
		}
		
		/// <summary>
		/// Adds an alert against a record type
		/// </summary>
		/// <param name="AlertDetails">alert details</param>
		public static async Task<HealthData> AddRecordAlert(RecordAlert AlertDetails)
		{
			try
			{
				string postBody = "{\"AlertDetails\":" + JsonConvert.SerializeObject(AlertDetails) + "}";  
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/provider/addrecordalert", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					HealthData obj = JsonConvert.DeserializeObject<HealthData>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}

			return null;
		}

		#endregion
		

		#region Get Records for User

		
		/// <summary>
		/// Get the emergency records for the current user
		/// </summary>
		public static async Task<HealthData> GetUserEmergencyRecords(LoginModel UserDetails)
		{
			try
			{
				string postBody = "{\"UserDetails\":" + JsonConvert.SerializeObject(UserDetails) + "}";  
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/provider/getuseremergencyrecord", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					HealthData obj = JsonConvert.DeserializeObject<HealthData>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		#endregion

		

		

		#region Send a message to a user

		/// <summary>
		/// Send a message to a user
		/// </summary>
		public static async Task<UserMessages> SendMessage(UserMessage messageDetail)
		{
			try
			{
				string postBody = "{\"MessageDetails\":" + JsonConvert.SerializeObject(messageDetail) + "}";  

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/provider/sendmessagetouser", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					UserMessages obj = JsonConvert.DeserializeObject<UserMessages>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}			

		#endregion

		
		#region Private Methods

		/// <summary>
		/// Extracts the inner JSON of an MS Ajax 'd' result and 
		/// removes embedded '__type' properties.
		/// </summary>
		/// <param name="json"></param>
		/// <returns>The inner JSON</returns>
		private static string CleanWebScriptJson(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				throw new ArgumentNullException("json");
			}

			Match match = RxMsAjaxJsonInner.Match(json);
			string innerJson = match.Success ? match.Groups[1].Value : json;
			return RxMsAjaxJsonInnerType.Replace(innerJson, string.Empty);
		}

		#endregion
	}		
}

