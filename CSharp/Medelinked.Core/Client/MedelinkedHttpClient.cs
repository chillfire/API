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
	public static class MedelinkedHttpClient
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
		/// Login as a user
		/// </summary>
		/// <param name="loginCredentials">Login credentials.</param>
		public static async Task<User> LoginUserAsync(LoginModel UserCredentials)
		{
			try
			{
				if (String.IsNullOrEmpty (UserCredentials.ProviderKey))
					UserCredentials.ProviderKey = ProviderKey;

				AuthCookies = new CookieContainer();
				string postBody = "{\"UserCredentials\":" + JsonConvert.SerializeObject(UserCredentials) + "}"; 
				httpClient = new HttpClient (new HttpClientHandler {
					CookieContainer = AuthCookies, // Use a durable store for authentication cookies
					UseCookies = true
				});

				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/authenticate", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					User obj = JsonConvert.DeserializeObject<User>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Login as a user
		/// </summary>
		/// <param name="loginCredentials">Login credentials.</param>
		public static async Task<User> RegisterUserAsync(Register UserDetails)
		{
			try
			{
				if (String.IsNullOrEmpty (UserDetails.ProviderKey))
					UserDetails.ProviderKey = ProviderKey;

				AuthCookies = new CookieContainer();
				string postBody = "{\"UserDetails\":" + JsonConvert.SerializeObject(UserDetails) + "}"; 
				httpClient = new HttpClient (new HttpClientHandler {
					CookieContainer = AuthCookies, // Use a durable store for authentication cookies
					UseCookies = true
				});

				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/Register", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					User obj = JsonConvert.DeserializeObject<User>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Login as a user
		/// </summary>
		/// <param name="loginCredentials">Login credentials.</param>
		public static async Task<User> AuthenticateTwoFactorCodeAsync(string TwoFactorCode)
		{
			try
			{

				string postBody = "{\"TwoFactorCode\":\"" + TwoFactorCode + "\"}"; 
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/authenticatetwofactorcode", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					User obj = JsonConvert.DeserializeObject<User>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Login as a user using the Facebook authentication token
		/// </summary>
		/// <param name="AccessToken">Facebook Access Token</param>
		public static async Task<User> LoginUserWithFacebook(string AccessToken)
		{
			try
			{
				//Initialise the HTTP client
				AuthCookies = new CookieContainer();
				httpClient = new HttpClient (new HttpClientHandler {
					CookieContainer = AuthCookies, // Use a durable store for authentication cookies
					UseCookies = true
				});

				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 

				string postBody = "{\"AccessToken\":\"" + AccessToken + "\"}"; 
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/authenticatewithfacebook", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					User obj = JsonConvert.DeserializeObject<User>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}
				
			return null;
		}

		/// <summary>
		/// Logs outs the user.
		/// </summary>
		public static async void LogoutUser()
		{
			//Log Out on the server first
			var user = await LogOutAsync();

			if (user != null && user.Message == "OK") {
				Debug.Write ("User logged out");
			}

			//Clear out the cookies
			AuthCookies = null;
			httpClient = null;
		}

		#endregion


		#region Change Password

		/// <summary>
		/// Login as a user
		/// </summary>
		/// <param name="loginCredentials">Login credentials.</param>
		public static async Task<User> ChangePasswordInAppAsync(ChangePasswordModel PasswordDetails)
		{
			try
			{
				if (String.IsNullOrEmpty (PasswordDetails.ProviderID))
					PasswordDetails.ProviderID = ProviderKey;

				string postBody = "{\"PasswordDetails\":" + JsonConvert.SerializeObject(PasswordDetails) + "}"; 
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/changepasswordwhenloggedin", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					User obj = JsonConvert.DeserializeObject<User>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Log Out as a user
		/// </summary>
		/// <param name="loginCredentials">Login credentials.</param>
		public static async Task<User> LogOutAsync()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/logout");

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					User obj = JsonConvert.DeserializeObject<User>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}
				
			return null;
		}

		/// <summary>
		/// Login as a user
		/// </summary>
		/// <param name="loginCredentials">Login credentials.</param>
		public static async Task<User> ChangePasswordAsync(ChangePasswordModel PasswordDetails)
		{
			try
			{
				if (String.IsNullOrEmpty (PasswordDetails.ProviderID))
					PasswordDetails.ProviderID = ProviderKey;

				AuthCookies = new CookieContainer();
				string postBody = "{\"PasswordDetails\":" + JsonConvert.SerializeObject(PasswordDetails) + "}"; 
				httpClient = new HttpClient ();

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/changepassword", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					User obj = JsonConvert.DeserializeObject<User>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
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
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/addrecord", new StringContent(postBody, Encoding.UTF8, "application/json"));

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
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/addrecords", new StringContent(postBody, Encoding.UTF8, "application/json"));

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
		/// Deletes a record
		/// </summary>
		/// <param name="ExistingRecord">Record details</param>
		public static async Task<HealthData> DeleteRecord(Record ExistingRecord)
		{
			try
			{
				string postBody = "{\"RecordDetails\":" + JsonConvert.SerializeObject(ExistingRecord) + "}";  
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/deleterecord", new StringContent(postBody, Encoding.UTF8, "application/json"));

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
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/addrecordalert", new StringContent(postBody, Encoding.UTF8, "application/json"));

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
		/// Add a record using the new AutoDetect feature
		/// </summary>
		/// <param name="ImageData">Image to parse</param>
		public static async Task<HealthData> UseAutoDetect(string ImageData)
		{
			try
			{
				string postBody = "{\"ImageData\":\"" + ImageData + "\"}"; 

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/useautodetect", new StringContent(postBody, Encoding.UTF8, "application/json"));

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

		#region Delete a file against a Record

		/// <summary>
		/// Add a record
		/// </summary>
		/// <param name="newRecord">Record details</param>
		public static async Task<HealthData> RemoveFile(FileDeleteModel FileDetails)
		{
			try
			{
				string postBody = "{\"FileDeleteRequest\":" + JsonConvert.SerializeObject(FileDetails) + "}";  
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/removefile", new StringContent(postBody, Encoding.UTF8, "application/json"));

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
		/// Get the health feed for the user
		/// </summary>
		public static async Task<HealthFeed> GetHealthFeed()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/healthfeed");

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					HealthFeed obj = JsonConvert.DeserializeObject<HealthFeed>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Get the records for the current user
		/// </summary>
		public static async Task<HealthData> GetHealthRecords()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/records");

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
		/// Get all of the records for the current user - this includes new record types
		/// </summary>
		public static async Task<HealthData> GetAllHealthRecords()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/mycompleterecord");

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
		/// Get the records alerts.
		/// </summary>
		/// <returns>The alerts.</returns>
		public static async Task<RecordAlerts> GetRecordAlerts()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/recordalerts");

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					RecordAlerts obj = JsonConvert.DeserializeObject<RecordAlerts>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Get the records for the current user
		/// </summary>
		public static async Task<byte[]> GetFileAsync(string RecordID, string Instance)
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/records/file/" + RecordID + "/" + Instance);

				if (msg.IsSuccessStatusCode)
				{
					var img = msg.Content.ReadAsByteArrayAsync();
					return img.Result;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}
				
			return null;
		}

		/// <summary>
		/// Get the records for the current user
		/// </summary>
		public static async Task<byte[]> GetFileAsync(string URL)
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(URL);

				if (msg.IsSuccessStatusCode)
				{
					var img = msg.Content.ReadAsByteArrayAsync();
					return img.Result;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}

			return null;
		}
		#endregion

        #region Symptom Checker

		/// <summary>
		/// Get the symptoms for the symptom checker
		/// </summary>
		public static async Task<List<HealthItem>> GetSymptomList()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/listsymptoms");

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					List<HealthItem> obj = JsonConvert.DeserializeObject<List<HealthItem>>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Check symptoms
		/// </summary>
		/// <param name="Symptoms">Symptoms to check</param>
		public static async Task<SymptomDiagnoses> CheckSymptoms(SymptomsToCheck Symptoms)
		{
			try
			{
				string postBody = "{\"SymptomDetails\":" + JsonConvert.SerializeObject(Symptoms) + "}";  
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/diagnosesymptoms", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					SymptomDiagnoses obj = JsonConvert.DeserializeObject<SymptomDiagnoses>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}

			return null;
		}

		/// <summary>
		/// Get details of the diagnosis
		/// </summary>
		/// <param name="DiagnosisID">Image to parse</param>
		public static async Task<DiagnosisDetail> GetDiagnosisDetails(string DiagnosisID)
		{
			try
			{
				string postBody = "{\"DiagnosisID\":\"" + DiagnosisID + "\"}"; 

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/diagnosisinformation", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					DiagnosisDetail obj = JsonConvert.DeserializeObject<DiagnosisDetail>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}

			return null;
		}

		#endregion
        
		#region Share records with a provider

		/// <summary>
		/// Share records with a provider
		/// </summary>
		public static async Task<RecordShare> SharingRequest(SharingRequest shareInfo)
		{
			try
			{
				string postBody = "{\"ShareRequest\":" + JsonConvert.SerializeObject(shareInfo) + "}";  

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/share", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					RecordShare obj = JsonConvert.DeserializeObject<RecordShare>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}			

		#endregion

		#region Friends and Family Functionality 

		/// <summary>
		/// Get the friends for the current user
		/// </summary>
		public static async Task<UserFriends> GetFriends()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/friends");

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					UserFriends obj = JsonConvert.DeserializeObject<UserFriends>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Get Friend Requests for the current user
		/// </summary>
		public static async Task<UserFriends> GetFriendRequests()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/friendrequests");

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					UserFriends obj = JsonConvert.DeserializeObject<UserFriends>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Send a friend request from the current user
		/// </summary>
		public static async Task<UserFriends> SendFriendRequest(UserFriend FriendDetails)
		{
			try
			{
				string postBody = "{\"FriendDetails\":" + JsonConvert.SerializeObject(FriendDetails) + "}";  

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/sendfriendrequest", new StringContent(postBody, Encoding.UTF8, "application/json"));


				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					UserFriends obj = JsonConvert.DeserializeObject<UserFriends>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Accept a friend request sent to me
		/// </summary>
		public static async Task<UserFriends> AcceptFriendRequest(UserFriend FriendDetails)
		{
			try
			{

				FriendDetails.DateConnected = null;
				FriendDetails.DateConnectionRequested = null;
				
				string postBody = "{\"FriendDetails\":" + JsonConvert.SerializeObject(FriendDetails) + "}";  

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/acceptfriendrequest", new StringContent(postBody, Encoding.UTF8, "application/json"));


				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					UserFriends obj = JsonConvert.DeserializeObject<UserFriends>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Accept a friend request sent to me
		/// </summary>
		public static async Task<UserFriends> RejectFriendRequest(UserFriend FriendDetails)
		{
			try
			{
				string postBody = "{\"FriendDetails\":" + JsonConvert.SerializeObject(FriendDetails) + "}";  

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/rejectfriendrequest", new StringContent(postBody, Encoding.UTF8, "application/json"));


				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					UserFriends obj = JsonConvert.DeserializeObject<UserFriends>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		#endregion

		#region Get Providers for User

		/// <summary>
		/// Get the health providers for the current user
		/// </summary>
		public static async Task<HealthProviders> GetHealthProviders()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/healthproviders");

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
		/// Get the records for the current user
		/// </summary>
		public static async Task<HealthProviders> FindHealthProvider(ProviderSearchModel FilterCriteria)
		{
			try
			{
				string postBody = "{\"FilterCriteria\":" + JsonConvert.SerializeObject(FilterCriteria) + "}";  

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/findhealthproviders", new StringContent(postBody, Encoding.UTF8, "application/json"));

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

		#endregion

		#region Add Provider to User account

		/// <summary>
		/// Get the records for the current user
		/// </summary>
		public static async Task<HealthProviders> ConnectWithHealthProvider(HealthProvider ProviderDetails)
		{
			try
			{
				string postBody = "{\"NewProvider\":" + JsonConvert.SerializeObject(ProviderDetails) + "}";  

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/connectwithhealthprovider", new StringContent(postBody, Encoding.UTF8, "application/json"));

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

		#endregion

		#region Health Snapshot functionality

		/// <summary>
		/// Get a particular health snapshot detail
		/// </summary>
		public static async Task<HealthSnapshotData> MyHealthSnapshot(Guid SnapshotID)
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/myhealthsnapshot?SnapshotID=" + SnapshotID.ToString());

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					HealthSnapshotData obj = JsonConvert.DeserializeObject<HealthSnapshotData>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		/// <summary>
		/// Add a new Health Snapshot
		/// </summary>
		public static async Task<HealthSnapshotData> AddHealthSnapshot(HealthSnapshotModel NewHealthSnapshot)
		{
			try
			{
				string postBody = "{\"NewHealthSnapshot\":" + JsonConvert.SerializeObject(NewHealthSnapshot) + "}";  

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/addhealthsnapshot", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					HealthSnapshotData obj = JsonConvert.DeserializeObject<HealthSnapshotData>(str);
					return obj;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}


			return null;
		}

		#endregion

		#region Get Messages for User

		/// <summary>
		/// Get the health providers for the current user
		/// </summary>
		public static async Task<UserMessages> GetUserMessages()
		{
			try
			{
				HttpResponseMessage msg = await httpClient.GetAsync(ServiceUrl + @"/api/user/messages");

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

		#region Send a message to a provider

		/// <summary>
		/// Share records with a provider
		/// </summary>
		public static async Task<UserMessages> SendMessage(UserMessage messageDetail)
		{
			try
			{
				string postBody = "{\"MessageDetails\":" + JsonConvert.SerializeObject(messageDetail) + "}";  

				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/sendmessage", new StringContent(postBody, Encoding.UTF8, "application/json"));

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

		#region Execute Commands & Queries

		/// <summary>
		/// Execute a command
		/// </summary>
		/// <param name="newRecord">Record details</param>
		public static async Task<string> ExecuteCommand(CommandRequest CommandDetails)
		{
			try
			{
				string postBody = "{\"CommandDetails\":" + JsonConvert.SerializeObject(CommandDetails) + "}";  
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/executecommand", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					return str;
				}
			}
			catch (Exception ex) {
				Debug.Write (ex.ToString ());
			}

			return null;
		}

		/// <summary>
		/// Execute a command
		/// </summary>
		/// <param name="newRecord">Record details</param>
		public static async Task<QueryResults> ExecuteQuery(QueryRequest QueryDetails)
		{
			try
			{
				string postBody = "{\"QueryDetails\":" + JsonConvert.SerializeObject(QueryDetails) + "}";  
				HttpResponseMessage msg = await httpClient.PostAsync(ServiceUrl + @"/api/user/executequery", new StringContent(postBody, Encoding.UTF8, "application/json"));

				if (msg.IsSuccessStatusCode)
				{
					String str = await msg.Content.ReadAsStringAsync();
					str = CleanWebScriptJson (str);
					QueryResults obj = JsonConvert.DeserializeObject<QueryResults>(str);
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

